using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iris.ViewModels;

namespace Iris.ServiceLayer.Contracts
{
    public interface IProductPropertyService
    {
        List<ProductPropertyViewModel> GetProductPropertyList (int propertyTypeId, int productId);

        ProductPropertyViewModel GetProductProperty (int propertyTypeId, int productId);

        Task<bool> AddOrUpdateByString(string productProperty, int productId);
        
    }
}
