using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public partial class ProductProperty
    {
        [Key]
        public int Id { get; set; }

        public int PropertyId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [MaxLength]
        public string Value { get; set; }

        public bool? ShowInIntro { get; set; }

        [Required]
        public int DisplayOrder { get; set; }


        public virtual Product Product { get; set; }

        public virtual Property Property { get; set; }
    }
}
