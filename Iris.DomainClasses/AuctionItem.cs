using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public class AuctionItem
    {

        public AuctionItem()
        {
            this.BidHistories = new HashSet<BidHistory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        [Required]
        public int PostedByUserId { get; set; }

        [Required]
        public DateTime CrateDate { get; set; }
        
        public DateTime? EditDate { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime StopDate { get; set; }

        public string ImageName { get; set; }
        
        public string ImageAddress { get; set; }

        public bool IsDelete { get; set; }
        
        public bool IsEdit { get; set; }

        [Required]
        public decimal MiniPrice { get; set; }

        [Required]
        public decimal MaxPrice { get; set; }

        public int? WinUserId { get; set; }


        public virtual ApplicationUser WinUser { get; set; }

        public virtual ApplicationUser PostedByUser { get; set; }

        public virtual ICollection<BidHistory> BidHistories { get; set; }

    }
}
