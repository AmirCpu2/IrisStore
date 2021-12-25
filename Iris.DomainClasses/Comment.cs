using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public partial class Comment
    {
        
        public Comment()
        {
            //ChildernComment = new HashSet<Comment>();
        }

        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public Nullable<int> UserId { get; set; }

        public DateTime? RegesterDate { get; set; }

        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEdited { get; set; }

        public string TextEdit { get; set; }

        public Nullable<int> ParentId { get; set; }

        public DateTime? EditedDate { get; set; }

        public bool IsQuestionAnswer { get; set; }

        public int ProductId { get; set; }

        //New Rates
        public Nullable<int> ConstructionQualityRate { get; set; }

        public Nullable<int> WorthBuyingRate { get; set; }

        public Nullable<int> InnovationRate { get; set; }

        public Nullable<int> FeaturesRate { get; set; }

        public Nullable<int> EaseOfUseRate { get; set; }

        public Nullable<int> DesignRate { get; set; }

        public Nullable<int> SuggestStatus { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Comment> ChildernComment { get; set; }

        public virtual Comment Parentcomment { get; set; }

        public virtual Product Product { get; set; }
    }

}
