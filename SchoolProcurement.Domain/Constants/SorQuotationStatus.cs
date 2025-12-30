using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Constants
{
    public static class SorQuotationItemStatus
    {
        public const string Submitted = "Submitted";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
    }

    public static class SorQuotationStatus
    {
        public const string Invited = "Invited";
        public const string Submitted = "Submitted";
        public const string PartiallyApproved = "PartiallyApproved";
        public const string FullyApproved = "FullyApproved";
    }

}
