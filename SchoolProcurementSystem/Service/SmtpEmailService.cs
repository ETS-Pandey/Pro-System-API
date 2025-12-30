using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Api.Dtos;
using SchoolProcurement.Api.Service.Interface;
using SchoolProcurement.Domain.Entities;
using SchoolProcurement.Infrastructure.Persistence;
using SchoolProcurement.Infrastructure.Security;
using System.Net;
using System.Net.Mail;

namespace SchoolProcurement.Api.Service
{
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly SchoolDbContext _db;
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly ICurrentUserService _currentUser;

        public SmtpEmailService(SchoolDbContext db, ILogger<SmtpEmailService> logger, ICurrentUserService currentUser)
        {
            _db = db;
            _logger = logger;
            _currentUser = currentUser;
        }

        /// <summary>
        /// Sends an email using SMTP settings loaded from DB for the given branchId.
        /// If branchId is null, uses current user's branch. If no branch settings found, looks for BranchID=0 (global).
        /// Logs the attempt into EmailLog.
        /// </summary>
        public async Task SendEmailAsync(EmailMessage message, int? branchId = null, CancellationToken ct = default)
        {
            //if (message == null) throw new ArgumentNullException(nameof(message));
            //if (!message.To.Any()) throw new ArgumentException("At least one recipient is required.");

            branchId ??= _currentUser.UserBranchId ?? 0; // default branch if current user not available

            // Load SMTP settings for this branch (prefer branch-specific then global)
            var smtp = await _db.EmailSetupDetails
                .Where(e => e.BranchID == branchId || e.BranchID == 0)
                .OrderByDescending(e => e.BranchID) // branch-specific first (assuming branchID>0)
                .FirstOrDefaultAsync(ct);
                        
            var log = new EmailLog
            {
                BranchID = branchId,
                Name = message.Subject,
                Subject = message.Subject,
                Body = message.Body,
                ToEmail = string.Join(",", message.To.Distinct()),
                FromEmail = string.IsNullOrWhiteSpace(message.FromAddress) ? smtp.FromEmail : message.FromAddress,
                TryCount = 0,
                IsSent = false,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _db.EmailLogs.Add(log);
            await _db.SaveChangesAsync(ct); // so log.ID exists

            if (smtp == null)
            {
                log.IsSent = false;
                log.SentDate = DateTime.UtcNow;
                log.ErrorMessage = "SMTP configuration not found.";
                log.UpdatedAt = DateTime.UtcNow;
                log.UpdatedBy = _currentUser.UserId;

                _db.EmailLogs.Update(log);
                await _db.SaveChangesAsync(ct);

                // No SMTP config available — write a failed log entry
                await WriteLogAsync(branchId, message, isSent: false, tryCount: 0, errorMessage: "No SMTP configuration found for branch.", ct: ct);
            }

            try
            {
                using var mailMessage = BuildMailMessage(message, smtp);
                using var smtpClient = new SmtpClient(smtp.Host, smtp.Port)
                {
                    EnableSsl = smtp.EnableSsl
                };

                if (!smtp.UseDefaultCredentials && !string.IsNullOrWhiteSpace(smtp.UserName))
                {
                    smtpClient.Credentials = new NetworkCredential(smtp.UserName, smtp.Password);
                }

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // send
                await smtpClient.SendMailAsync(mailMessage, ct);

                // success: update log
                log.TryCount += 1;
                log.IsSent = true;
                log.SentDate = DateTime.UtcNow;
                log.ErrorMessage = null;
                log.UpdatedAt = DateTime.UtcNow;
                log.UpdatedBy = _currentUser.UserId;

                _db.EmailLogs.Update(log);
                await _db.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {To} using branch {Branch}", log.ToEmail, branchId);

                log.TryCount += 1;
                log.IsSent = false;
                log.ErrorMessage = ex.Message.Truncate(1900); // optional helper to fit column
                log.UpdatedAt = DateTime.UtcNow;
                log.UpdatedBy = _currentUser.UserId;

                _db.EmailLogs.Update(log);
                await _db.SaveChangesAsync(ct);
            }
        }

        private MailMessage BuildMailMessage(EmailMessage message, EmailSetupDetail smtp)
        {
            var fromAddr = new MailAddress(string.IsNullOrWhiteSpace(message.FromAddress) ? smtp.FromEmail : message.FromAddress,
                                           message.FromName ?? smtp.FromEmail);

            var mail = new MailMessage
            {
                From = fromAddr,
                Subject = message.Subject,
                IsBodyHtml = message.IsBodyHtml,
                Body = message.Body
            };

            foreach (var to in message.To.Distinct()) mail.To.Add(to);
            foreach (var cc in message.Cc.Distinct()) mail.CC.Add(cc);
            foreach (var bcc in message.Bcc.Distinct()) mail.Bcc.Add(bcc);

            foreach (var att in message.Attachments)
            {
                var ms = new MemoryStream(att.Content);
                var attachment = new Attachment(ms, att.FileName);
                mail.Attachments.Add(attachment);
            }

            return mail;
        }

        private async Task WriteLogAsync(int? branchId, EmailMessage msg, bool isSent, int tryCount, string? errorMessage, CancellationToken ct = default)
        {
            var log = new EmailLog
            {
                BranchID = branchId,
                Name = msg.Subject,
                Subject = msg.Subject,
                Body = msg.Body,
                ToEmail = string.Join(",", msg.To.Distinct()),
                FromEmail = msg.FromAddress,
                TryCount = tryCount,
                IsSent = isSent,
                ErrorMessage = errorMessage,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.UtcNow
            };
            _db.EmailLogs.Add(log);
            await _db.SaveChangesAsync(ct);
        }
    }

    public static class StringExtensions
    {
        public static string Truncate(this string? value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
