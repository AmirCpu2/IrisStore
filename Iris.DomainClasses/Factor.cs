using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public class Factor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        public Guid PublicId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string RefId { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;

        public FactorStatus Status { get; set; } = FactorStatus.Paying;

        public ApplicationUser User { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<FactorProduct> Products { get; set; }
    }

    public enum FactorStatus
    {
        [Description("لغو شده")]
        Cancelled = -1,
        [Description("در حال پرداخت")]
        Paying = 0,
        [Description("در انتظار پرداخت")]
        AwaitingPayment = 1,
        [Description("پرداخت شده")]
        Paid = 2,
        [Description("آماده سازی سفارش")]
        OrderPreparation = 3,
        [Description("خروج از مرکز پردازش")]
        ExitProcessingCenter = 4,
        [Description("تحویل به پست")]
        PostDelivery = 5,
        [Description("مرکز مبادلات پست")]
        PostProcessingCenter = 6,
        [Description("تحویل شده")]
        Delivered = 7
    }
}
