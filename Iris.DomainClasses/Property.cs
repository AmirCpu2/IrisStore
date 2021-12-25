using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public partial class Property 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Property()
        {
            ProductProperties = new HashSet<ProductProperty>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(250)]
        public string NameFA { get; set; }

        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }


        public virtual ICollection<ProductProperty> ProductProperties { get; set; }
    }
}
