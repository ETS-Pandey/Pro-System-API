using Microsoft.EntityFrameworkCore;
using SchoolProcurement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Infrastructure.Persistence
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options) { }

        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<AuditLogDetail> AuditLogDetails { get; set; } = null!;
        public DbSet<MasterDetail> MasterDetails { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductStock> ProductStocks { get; set; } = null!;
        public DbSet<ServiceOrderRequest> ServiceOrderRequests { get; set; } = null!;
        public DbSet<ServiceOrderRequestItem> ServiceOrderRequestItems { get; set; } = null!;
        public DbSet<ServiceOrderRequestAttachment> ServiceOrderRequestAttachments { get; set; } = null!;
        public DbSet<ServiceOrderRequestAssignment> ServiceOrderRequestAssignments { get; set; } = null!;
        public DbSet<BranchBudgetTransaction> BranchBudgetTransactions { get; set; } = null!;
        public DbSet<ContactDetail> ContactDetails { get; set; } = null!;
        public DbSet<EmailSetupDetail> EmailSetupDetails { get; set; } = null!;
        public DbSet<EmailLog> EmailLogs { get; set; } = null!;
        public DbSet<SorContactMapping> SorContactMappings => Set<SorContactMapping>();
        public DbSet<SorContactMappingItem> SorContactMappingItems => Set<SorContactMappingItem>();
        public DbSet<SorContactMappingAttachment> SorContactMappingAttachments => Set<SorContactMappingAttachment>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
        public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
        public DbSet<PurchaseOrderReceipt> PurchaseOrderReceipts => Set<PurchaseOrderReceipt>();
        public DbSet<PurchaseOrderReceiptItem> PurchaseOrderReceiptItems => Set<PurchaseOrderReceiptItem>();
        public DbSet<PurchaseOrderPayment> PurchaseOrderPayments => Set<PurchaseOrderPayment>();
        public DbSet<SorChat> SorChats => Set<SorChat>();
        public DbSet<SorChatAttachment> SorChatAttachments => Set<SorChatAttachment>();
        public DbSet<UserNotification> UserNotifications => Set<UserNotification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Branch
            modelBuilder.Entity<Branch>(b =>
            {
                b.ToTable("Branches");

                b.HasKey(x => x.ID);

                b.Property(x => x.ID)
                 .ValueGeneratedOnAdd();

                b.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(200);

                b.Property(x => x.MobileNo)
                 .HasMaxLength(20);

                b.Property(x => x.Website)
                 .HasMaxLength(200);

                b.Property(x => x.Address)
                 .HasMaxLength(500);

                b.Property(x => x.Budget).HasColumnType("decimal(18,2)").HasDefaultValue(0m);

                b.Property(x => x.IsDelete)
                 .HasDefaultValue(false);

                b.Property(x => x.CreatedDate)
                 .HasColumnType("datetime2(3)")
                 .HasDefaultValueSql("SYSUTCDATETIME()");

                b.Property(x => x.UpdatedDate)
                 .HasColumnType("datetime2(3)");

                // Optional but useful index
                b.HasIndex(x => x.Name)
                 .HasDatabaseName("IX_Branches_Name")
                 .HasFilter("[IsDelete] = 0");
            });
            #endregion

            #region Role
            modelBuilder.Entity<Role>(r =>
            {
                r.ToTable("Roles");

                r.HasKey(x => x.ID);

                r.Property(x => x.ID)
                 .ValueGeneratedOnAdd();

                r.Property(x => x.Name)
                 .IsRequired()
                 .HasMaxLength(100);

                r.Property(x => x.CreatedDate)
                 .HasColumnType("datetime2(3)")
                 .HasDefaultValueSql("SYSUTCDATETIME()");

                r.HasIndex(x => x.Name)
                 .IsUnique()
                 .HasDatabaseName("UX_Roles_Name");
            });
            #endregion

            #region User
            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("Users");

                u.HasKey(x => x.ID);

                u.Property(x => x.ID)
                 .ValueGeneratedOnAdd();

                u.Property(x => x.RoleID)
                 .IsRequired();

                u.Property(x => x.FirstName)
                 .IsRequired()
                 .HasMaxLength(150);

                u.Property(x => x.MiddleName)
                 .HasMaxLength(150);

                u.Property(x => x.LastName)
                 .IsRequired()
                 .HasMaxLength(150);

                u.Property(x => x.Email)
                 .IsRequired()
                 .HasMaxLength(150);

                u.Property(x => x.Password)
                 .IsRequired()
                 .HasMaxLength(500);

                u.Property(x => x.Saltkey)
                 .IsRequired()
                 .HasMaxLength(200);

                u.Property(x => x.UniqueKey)
                 .IsRequired()
                 .HasMaxLength(200);

                u.Property(x => x.IsDelete)
                 .HasDefaultValue(false);

                u.Property(x => x.CreatedDate)
                 .HasColumnType("datetime2(3)")
                 .HasDefaultValueSql("SYSUTCDATETIME()");

                u.Property(x => x.UpdatedDate)
                 .HasColumnType("datetime2(3)");

                // Relationship: User -> Role
                u.HasOne(x => x.Role)
                 .WithMany(r => r.Users)                // make sure Role has ICollection<User> Users
                 .HasForeignKey(x => x.RoleID)
                 .HasConstraintName("FK_Users_Role")
                 .OnDelete(DeleteBehavior.NoAction);    // matches SQL (no ON DELETE)

                // Relationship: User -> Branch
                u.HasOne(u => u.Branch)
                  .WithMany()
                  .HasForeignKey(u => u.BranchID)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);

                // Unique email for non-deleted users
                u.HasIndex(x => x.Email)
                 .IsUnique()
                 .HasDatabaseName("UX_Users_Email")
                 .HasFilter("[IsDelete] = 0");

                //// Helpful index
                //u.HasIndex(x => x.BranchID)
                // .HasDatabaseName("IX_Users_Branch");
            });
            #endregion

            #region AuditLog
            modelBuilder.Entity<AuditLog>(a =>
            {
                a.ToTable("AuditLogs");

                a.HasKey(x => x.AuditId);

                a.Property(x => x.AuditId)
                 .ValueGeneratedOnAdd();

                a.Property(x => x.CorrelationId)
                 .IsRequired();

                a.Property(x => x.EventTime)
                 .HasColumnType("datetime2(3)")
                 .HasDefaultValueSql("SYSUTCDATETIME()");

                a.Property(x => x.EventType)
                 .IsRequired()
                 .HasMaxLength(100);

                a.Property(x => x.UserName)
                 .HasMaxLength(256);

                a.Property(x => x.Source)
                 .HasMaxLength(200);

                a.Property(x => x.Route)
                 .HasMaxLength(400);

                a.Property(x => x.HttpMethod)
                 .HasMaxLength(10);

                a.Property(x => x.ClientIp)
                 .HasMaxLength(45);

                a.Property(x => x.Summary)
                 .HasMaxLength(1000);

                a.Property(x => x.Extra);

                a.HasMany(x => x.Details)
                 .WithOne(d => d.AuditLogs)
                 .HasForeignKey(d => d.AuditId)
                 .HasConstraintName("FK_AuditLogDetails_AuditLogs")
                 .OnDelete(DeleteBehavior.Cascade);

                a.HasIndex(x => x.EventTime)
                 .HasDatabaseName("IX_AuditLog_EventTime");

                a.HasIndex(x => x.UserId)
                 .HasDatabaseName("IX_AuditLog_UserId");

                a.HasIndex(x => new { x.CorrelationId, x.EventType })
                 .HasDatabaseName("IX_AuditLog_Correlation_EventType");
            });
            #endregion

            #region AuditLogDetail
            modelBuilder.Entity<AuditLogDetail>(d =>
            {
                d.ToTable("AuditLogDetails");

                d.HasKey(x => x.DetailId);

                d.Property(x => x.DetailId)
                 .ValueGeneratedOnAdd();

                d.Property(x => x.EntityName)
                 .IsRequired()
                 .HasMaxLength(200);

                d.Property(x => x.PrimaryKey)
                 .HasMaxLength(200);

                d.Property(x => x.Operation)
                 .IsRequired()
                 .HasMaxLength(20);

                d.Property(x => x.PropertyName)
                 .HasMaxLength(200);

                // relationship defined on AuditLog config above, but you can define here instead if you prefer:
                d.HasOne(x => x.AuditLogs)
                 .WithMany(a => a.Details)
                 .HasForeignKey(x => x.AuditId)
                 .HasConstraintName("FK_AuditLogDetails_AuditLogs")
                 .OnDelete(DeleteBehavior.Cascade);

                d.HasIndex(x => x.AuditId)
                 .HasDatabaseName("IX_AuditLogDetail_AuditId");

                d.HasIndex(x => x.EntityName)
                 .HasDatabaseName("IX_AuditLogDetail_Entity");
            });

            #endregion

            #region MasterDetail
            modelBuilder.Entity<MasterDetail>(m =>
            {
                m.ToTable("MasterDetails");

                m.HasKey(x => x.ID);

                m.Property(x => x.Category);
                m.Property(x => x.Name);
                m.Property(x => x.OtherName);
                m.Property(x => x.Description);

                m.Property(x => x.IsDeleted).HasDefaultValue(false);
                m.Property(x => x.CreatedDate);
                m.Property(x => x.UpdatedDate);

                // Self-referencing FK
                m.HasOne(x => x.Parent)
                 .WithMany(x => x.Children)
                 .HasForeignKey(x => x.ParentID)
                 .OnDelete(DeleteBehavior.NoAction);

                // Useful indexes
                m.HasIndex(x => x.Category);
                m.HasIndex(x => x.IsDeleted);
            });
            #endregion

            #region Product
            modelBuilder.Entity<Product>(p =>
            {
                p.ToTable("Products");
                p.HasKey(x => x.ID);

                p.Property(x => x.Name).IsRequired().HasMaxLength(200);
                p.Property(x => x.Description).HasColumnType("nvarchar(max)");
                p.Property(x => x.SalesPrice).HasColumnType("decimal(18,2)").HasDefaultValue(0.00m);
                p.Property(x => x.PurchasePrice).HasColumnType("decimal(18,2)").HasDefaultValue(0.00m);
                p.Property(x => x.IsDeleted).HasDefaultValue(false);
                p.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");

                // Relationship: Product.Category -> MasterDetails (CategoryID)
                p.HasOne(x => x.Category)
                 .WithMany()                      // if MasterDetail has a collection change accordingly
                 .HasForeignKey(x => x.CategoryID)
                 .HasConstraintName("FK_Products_MasterDetails_Category")
                 .OnDelete(DeleteBehavior.NoAction);

                // Relationship: Product.UnitType -> MasterDetails (UnitTypeID)
                p.HasOne(x => x.UnitType)
                 .WithMany()
                 .HasForeignKey(x => x.UnitTypeID)
                 .HasConstraintName("FK_Products_MasterDetails_UnitType")
                 .OnDelete(DeleteBehavior.NoAction);

                // Indexes
                p.HasIndex(x => x.Name).HasDatabaseName("IX_Products_Name").HasFilter("[IsDeleted] = 0");
                p.HasIndex(x => x.CategoryID).HasDatabaseName("IX_Products_Category").HasFilter("[IsDeleted] = 0");
                p.HasIndex(x => x.UnitTypeID).HasDatabaseName("IX_Products_UnitType").HasFilter("[IsDeleted] = 0");
            });
            #endregion

            #region ProductStock
            modelBuilder.Entity<ProductStock>(ps =>
            {
                ps.ToTable("ProductStocks");
                ps.HasKey(x => x.ID);

                ps.Property(x => x.ProductID).IsRequired();
                ps.Property(x => x.BranchID).IsRequired();

                // Quantity and reserved quantities with decimal precision
                ps.Property(x => x.Quantity).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                ps.Property(x => x.ReservedQty).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                ps.Property(x => x.ReorderLevel).HasColumnType("decimal(18,2)").IsRequired(false);

                ps.Property(x => x.IsDeleted).HasDefaultValue(false);
                ps.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");
                ps.Property(x => x.UpdatedDate).IsRequired(false);

                // Relationships (FKs)
                ps.HasOne(x => x.Product)
                  .WithMany() // if Product has a collection, change to .WithMany(p => p.Stocks)
                  .HasForeignKey(x => x.ProductID)
                  .HasConstraintName("FK_ProductStocks_Product")
                  .OnDelete(DeleteBehavior.Restrict);

                ps.HasOne(x => x.Branch)
                  .WithMany() // if Branch has a collection, change to .WithMany(b => b.ProductStocks)
                  .HasForeignKey(x => x.BranchID)
                  .HasConstraintName("FK_ProductStocks_Branch")
                  .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint to ensure one row per product per branch
                ps.HasIndex(x => new { x.ProductID, x.BranchID })
                  .IsUnique()
                  .HasDatabaseName("UX_ProductStocks_Product_Branch");

                // Useful indexes for lookups and reports
                ps.HasIndex(x => x.ProductID).HasDatabaseName("IX_ProductStocks_ProductID");
                ps.HasIndex(x => x.BranchID).HasDatabaseName("IX_ProductStocks_BranchID");
                ps.HasIndex(x => x.ReorderLevel).HasDatabaseName("IX_ProductStocks_ReorderLevel").HasFilter("[IsDeleted] = 0");

                // Optional: apply global query filter for soft deletes
                // Uncomment the following line to automatically exclude IsDeleted == true rows from queries:
                // ps.HasQueryFilter(x => !x.IsDeleted);
            });
            #endregion

            #region ServiceOrderRequest (SOR)
            modelBuilder.Entity<ServiceOrderRequest>(s =>
            {
                s.ToTable("ServiceOrderRequests");
                s.HasKey(x => x.ID);

                s.Property(x => x.UniqueString).IsRequired().HasMaxLength(100);
                s.Property(x => x.PurposeDescription).HasColumnType("nvarchar(max)");
                s.Property(x => x.AdditionalJustification).HasColumnType("nvarchar(max)");
                s.Property(x => x.RequiredByDate).IsRequired(false);
                s.Property(x => x.Status).IsRequired().HasMaxLength(50).HasDefaultValue("New");
                s.Property(x => x.IsDeleted).HasDefaultValue(false);
                s.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");

                s.HasOne(x => x.Branch)
                 .WithMany()
                 .HasForeignKey(x => x.BranchID)
                 .HasConstraintName("FK_SOR_Branch")
                 .OnDelete(DeleteBehavior.Restrict);

                s.HasOne(x => x.Department)
                 .WithMany()
                 .HasForeignKey(x => x.DepartmentID)
                 .HasConstraintName("FK_SOR_Department_MasterDetails")
                 .OnDelete(DeleteBehavior.Restrict);

                s.HasOne(x => x.UrgencyLevel)
                 .WithMany()
                 .HasForeignKey(x => x.UrgencyLevelID)
                 .HasConstraintName("FK_SOR_Urgency_MasterDetails")
                 .OnDelete(DeleteBehavior.Restrict);

                s.HasOne(x => x.CurrentAssignedUser)
                 .WithMany()
                 .HasForeignKey(x => x.CurrentAssignedUserID)
                 .HasConstraintName("FK_SOR_CurrentAssignedUser")
                 .OnDelete(DeleteBehavior.Restrict);

                s.HasIndex(x => x.BranchID).HasDatabaseName("IX_SOR_BranchID");
                s.HasIndex(x => x.Status).HasDatabaseName("IX_SOR_Status");
            });
            #endregion

            #region ServiceOrderRequestItem
            modelBuilder.Entity<ServiceOrderRequestItem>(it =>
            {
                it.ToTable("ServiceOrderRequestItems");
                it.HasKey(x => x.ID);

                it.Property(x => x.Quantity).HasColumnType("decimal(18,2)").HasDefaultValue(0m);
                it.Property(x => x.EstimatedCost).HasColumnType("decimal(18,2)").IsRequired(false);
                it.Property(x => x.TechnicalSpecifications).HasColumnType("nvarchar(max)");
                it.Property(x => x.IsDeleted).HasDefaultValue(false);
                it.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");

                it.HasOne(x => x.SOR)
                  .WithMany(s => s.Items)
                  .HasForeignKey(x => x.SORID)
                  .HasConstraintName("FK_SORItem_SOR")
                  .OnDelete(DeleteBehavior.Cascade);

                it.HasOne(x => x.Product)
                  .WithMany()
                  .HasForeignKey(x => x.ProductID)
                  .HasConstraintName("FK_SORItem_Product")
                  .OnDelete(DeleteBehavior.Restrict);

                it.HasOne(x => x.UnitType)
                  .WithMany()
                  .HasForeignKey(x => x.UnitTypeID)
                  .HasConstraintName("FK_SORItem_UnitType_MasterDetails")
                  .OnDelete(DeleteBehavior.Restrict);

                it.HasIndex(x => x.SORID).HasDatabaseName("IX_SORItem_SORID");
                it.HasIndex(x => x.ProductID).HasDatabaseName("IX_SORItem_ProductID");
            });
            #endregion

            #region ServiceOrderRequestAttachment
            modelBuilder.Entity<ServiceOrderRequestAttachment>(a =>
            {
                a.ToTable("ServiceOrderRequestAttachments");
                a.HasKey(x => x.ID);

                a.Property(x => x.FileName).IsRequired().HasMaxLength(500);
                a.Property(x => x.FilePath).IsRequired().HasMaxLength(2000);
                a.Property(x => x.IsDeleted).HasDefaultValue(false);
                a.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");

                a.HasOne(x => x.SOR)
                 .WithMany(s => s.Attachments)
                 .HasForeignKey(x => x.SORID)
                 .HasConstraintName("FK_SORAttach_SOR")
                 .OnDelete(DeleteBehavior.Cascade);

                a.HasIndex(x => x.SORID).HasDatabaseName("IX_SORAttach_SORID");
            });
            #endregion

            #region ServiceOrderRequestAssignment
            modelBuilder.Entity<ServiceOrderRequestAssignment>(asg =>
            {
                asg.ToTable("ServiceOrderRequestAssignment");
                asg.HasKey(x => x.ID);

                asg.Property(x => x.Note).HasMaxLength(1000);
                asg.Property(x => x.IsDeleted).HasDefaultValue(false);
                asg.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");

                asg.HasOne(x => x.SOR)
                   .WithMany(s => s.Assignments)
                   .HasForeignKey(x => x.SORID)
                   .HasConstraintName("FK_SORAssign_SOR")
                   .OnDelete(DeleteBehavior.Cascade);

                asg.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserID)
                   .HasConstraintName("FK_SORAssign_User")
                   .OnDelete(DeleteBehavior.Restrict);

                asg.HasIndex(x => x.SORID).HasDatabaseName("IX_SORAssign_SORID");
                asg.HasIndex(x => x.UserID).HasDatabaseName("IX_SORAssign_UserID");
            });
            #endregion

            #region BranchBudgetTransaction
            modelBuilder.Entity<BranchBudgetTransaction>(t =>
            {
                t.ToTable("BranchBudgetTransactions");
                t.HasKey(x => x.ID);

                t.Property(x => x.TransactionType).IsRequired().HasMaxLength(20);
                t.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
                t.Property(x => x.BalanceAfter).HasColumnType("decimal(18,2)").IsRequired();
                t.Property(x => x.Note).HasMaxLength(1000).IsRequired(false);
                t.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");

                t.HasOne(x => x.Branch)
                 .WithMany() // optionally change to .WithMany(b => b.BudgetTransactions)
                 .HasForeignKey(x => x.BranchID)
                 .HasConstraintName("FK_BranchBudgetTransactions_Branch")
                 .OnDelete(DeleteBehavior.Restrict);

                t.HasOne<Domain.Entities.ServiceOrderRequest>() // navigation not required
                 .WithMany()
                 .HasForeignKey(x => x.RelatedSORID)
                 .HasConstraintName("FK_BranchBudgetTransactions_SOR")
                 .OnDelete(DeleteBehavior.Restrict);

                t.HasIndex(x => x.BranchID).HasDatabaseName("IX_BranchBudgetTransactions_BranchID");
                t.HasIndex(x => x.RelatedSORID).HasDatabaseName("IX_BranchBudgetTransactions_SORID");
            });
            #endregion

            #region ContactDetail
            modelBuilder.Entity<ContactDetail>(c =>
            {
                c.ToTable("ContactDetails");
                c.HasKey(x => x.ID);

                c.Property(x => x.BranchID).IsRequired();

                c.HasOne(x => x.Branch)
                 .WithMany()
                 .HasForeignKey(x => x.BranchID)
                 .HasConstraintName("FK_ContactDetails_Branch")
                 .OnDelete(DeleteBehavior.Restrict);

                c.Property(x => x.UniqueString)
                   .HasMaxLength(100)
                   .IsRequired(false);

                c.Property(x => x.Name).IsRequired().HasMaxLength(250);

                c.Property(x => x.IsDeleted).HasDefaultValue(false);

                c.HasIndex(x => new { x.BranchID }).HasDatabaseName("IX_ContactDetails_BranchID");
            });
            #endregion

            #region EmailSetupDetail
            modelBuilder.Entity<EmailSetupDetail>(b =>
            {
                b.ToTable("EmailSetupDetails");
                b.HasKey(x => x.ID);

                b.Property(x => x.BranchID).IsRequired();
                b.Property(x => x.FromEmail).IsRequired().HasMaxLength(320);
                b.Property(x => x.Host).IsRequired().HasMaxLength(500);
                b.Property(x => x.Port).IsRequired();
                b.Property(x => x.EnableSsl).IsRequired();
                b.Property(x => x.UseDefaultCredentials).IsRequired();
                b.Property(x => x.UserName).HasMaxLength(500).IsRequired(false);
                b.Property(x => x.Password).HasMaxLength(1000).IsRequired(false);

                b.Property(x => x.CreatedDate).HasDefaultValueSql("SYSUTCDATETIME()");
                b.HasIndex(x => x.BranchID).HasDatabaseName("IX_EmailSetupDetails_BranchID");
            });
            #endregion

            #region EmailLog
            modelBuilder.Entity<EmailLog>(b =>
            {
                b.ToTable("EmailLogs");
                b.HasKey(x => x.ID);

                b.Property(x => x.BranchID).IsRequired(false);
                b.Property(x => x.Name).HasMaxLength(250).IsRequired(false);
                b.Property(x => x.Subject).HasMaxLength(500).IsRequired(false);
                b.Property(x => x.Body).HasColumnType("nvarchar(max)").IsRequired(false);
                b.Property(x => x.ToEmail).HasMaxLength(1000).IsRequired(false);
                b.Property(x => x.FromEmail).HasMaxLength(320).IsRequired(false);
                b.Property(x => x.SentDate).IsRequired(false);
                b.Property(x => x.TryCount).HasDefaultValue(0);
                b.Property(x => x.IsSent).HasDefaultValue(false);
                b.Property(x => x.ErrorMessage).HasMaxLength(2000).IsRequired(false);
                b.Property(x => x.IsDelete).HasDefaultValue(false);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                b.HasIndex(x => x.BranchID).HasDatabaseName("IX_EmailLogs_BranchID");
                b.HasIndex(x => x.IsSent).HasDatabaseName("IX_EmailLogs_IsSent");
            });
            #endregion

            #region SorContactMapping
            modelBuilder.Entity<SorContactMapping>()
            .HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SorContactMapping>()
                .HasOne(x => x.Contact)
                .WithMany()
                .HasForeignKey(x => x.ContactID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SorContactMapping>()
                .HasOne(x => x.SOR)
                .WithMany()
                .HasForeignKey(x => x.SORID)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region SorContactMappingAttachment
            modelBuilder.Entity<SorContactMappingAttachment>()
                .HasOne(x => x.SorContactMapping)
                .WithMany(x => x.Attachments)
                .HasForeignKey(x => x.SorContactMappingID);
            #endregion

            #region SorContactMappingItem
            modelBuilder.Entity<SorContactMappingItem>()
                .HasOne(x => x.SorContactMapping)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.SorContactMappingID);
            #endregion

            #region PurchaseOrder
            modelBuilder.Entity<PurchaseOrder>(e =>
            {
                e.ToTable("PurchaseOrders");

                e.HasIndex(x => x.PONumber).IsUnique();
                e.Property(x => x.Status).HasMaxLength(50);
            });

            modelBuilder.Entity<PurchaseOrderItem>(e =>
            {
                e.ToTable("PurchaseOrderItems");

                e.HasOne(x => x.PurchaseOrder)
                    .WithMany(p => p.Items)
                    .HasForeignKey(x => x.PurchaseOrderID);
            });

            modelBuilder.Entity<PurchaseOrderPayment>(e =>
            {
                e.ToTable("PurchaseOrderPayments");

                e.HasOne(x => x.PurchaseOrder)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(x => x.PurchaseOrderID);
            });

            #endregion

            #region SorChat
            modelBuilder.Entity<SorChat>(entity =>
            {
                entity.ToTable("SORChats");

                entity.HasKey(x => x.ID);

                entity.HasOne(x => x.ParentChat)
                    .WithMany(x => x.Replies)
                    .HasForeignKey(x => x.ParentChatID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SenderUser)
                    .WithMany()
                    .HasForeignKey(x => x.SenderUserID)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(x => x.Attachments)
                    .WithOne(a => a.SorChat)
                    .HasForeignKey(a => a.SORChatID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

            #region SorChatAttachment

            modelBuilder.Entity<SorChatAttachment>(entity =>
            {
                entity.ToTable("SORChatAttachments");

                entity.HasKey(e => e.ID);

                entity.Property(e => e.FileName)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(e => e.ContentType)
                    .HasMaxLength(100);
            });
            #endregion

            #region UserNotification

            modelBuilder.Entity<UserNotification>(e =>
            {
                e.ToTable("UserNotifications");
                e.HasKey(x => x.ID);

                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);

                e.Property(x => x.Title).HasMaxLength(200).IsRequired();
                e.Property(x => x.NotificationType).HasMaxLength(50).IsRequired();
                e.Property(x => x.EntityType).HasMaxLength(50);
                e.Property(x => x.RedirectUrl).HasMaxLength(500);
            });

            #endregion
        }
    }
}
