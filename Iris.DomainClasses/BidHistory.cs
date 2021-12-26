using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public class BidHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        
        [Required]
        [ForeignKey("AuctionItem")]
        public int AuctionItemId { get; set; }

        public DateTime BidDate { get; set; }

        public decimal BidPrice { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual AuctionItem AuctionItem { get; set; }
    }
}
