using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.DomainClasses.Configurations
{
    public class ProductQuestionAnswerConfig : EntityTypeConfiguration<ProductQuestionAnswer>
    {
        public ProductQuestionAnswerConfig()
        {
            HasOptional(e => e.Comment)
                .WithRequired(e => e.ProductQuestionAnswer);

        }
    }
}
