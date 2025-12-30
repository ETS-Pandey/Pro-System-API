using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Domain.Entities
{
    public class BranchBudgetTransaction
    {
        public int ID { get; set; }
        public int BranchID { get; set; }
        public int? RelatedSORID { get; set; }
        public string TransactionType { get; set; } = default!; // "Debit","Credit","Adjustment"
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public string? Note { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public Branch? Branch { get; set; }
    }
}
