using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public partial class ProductQuestionAnswer
    {
        public int ProductId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual Product Product { get; set; }
    }
}
