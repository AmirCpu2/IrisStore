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
    public class CoinFactor
    {

        [DisplayName("شناسه یکتا")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [DisplayName("نوع بسته")]
        public int CoinPacketId { get; set; }

        [DisplayName("تاریخ خرید")]
        public DateTime BuyDate { get; set; } = DateTime.Now;

        [DisplayName("وضعیت خرید")]
        public int StatusId { get; set; }

        [DisplayName("شناسه پرداخت")]
        public string RefId { get; set; }

        [DisplayName("شناسه کابر")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        
        public virtual ApplicationUser User { get; set; }
    }
}
