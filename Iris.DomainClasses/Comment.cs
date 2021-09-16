using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public partial class Comment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Comment()
        {
            Comment1 = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int UserId { get; set; }

        public DateTime? RegesterDate { get; set; }

        public int status { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEdited { get; set; }

        public string TextEdit { get; set; }

        public int ParentId { get; set; }

        public DateTime? EditedDate { get; set; }

        public bool IsQuestionAnswer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comment1 { get; set; }

        public virtual Comment Comment2 { get; set; }

        public virtual ProductQuestionAnswer ProductQuestionAnswer { get; set; }

        public virtual Post Post { get; set; }

        public virtual Product Product { get; set; }
    }

}
