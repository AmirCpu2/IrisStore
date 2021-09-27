using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses
{
    public partial class PropertyType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyType()
        {
            Properties = new HashSet<Property>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(150)]
        public string NameFA { get; set; }

        [Required]
        public bool IsArchive { get; set; }

        [Required]
        public bool IsEdited { get; set; }

        public DateTime? EditDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property> Properties { get; set; }
    }
}
