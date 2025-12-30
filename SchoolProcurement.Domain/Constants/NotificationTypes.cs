using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Infrastructure.Constants
{
    public static class NotificationTypes
    {
        public const string Invited = "Invited";

        public const string SorAssigned = "SOR_ASSIGNED";
        public const string SorApproved = "SOR_APPROVED";
        public const string SorRejected = "SOR_REJECTED";

        public const string QuotationSubmitted = "QUOTATION_SUBMITTED";
        public const string QuotationApproved = "QUOTATION_APPROVED";

        public const string PurchaseOrderCreated = "PO_CREATED";
    }
}

