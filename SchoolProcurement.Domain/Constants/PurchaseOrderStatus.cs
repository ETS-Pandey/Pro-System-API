using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Constants
{
    public static class PurchaseOrderStatus
    {
        public const string Open = "Open";
        public const string PartiallyReceived = "PartiallyReceived";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }

}
