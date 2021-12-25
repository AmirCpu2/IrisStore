using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Iris.ViewModels
{
    public class ShowCommentsProductViewModel
    {

        public CommentViewModel Comment { get; set; }

        public string UserFullName { get; set; }
        
        public string ProductNameFa { get; set; }

        public string SellerFa { get; set; }

        public bool IsBuyer { get; set; } = false;

        public string ColorNameFa { get; set; }

        public string ColorCode { get; set; }
        

    }
}
