using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;

namespace Iris.ServiceLayer
{
    public class AdminPanelService : IAdminPanelService
    {
        private readonly IDbSet<Product> _products;
        private readonly IDbSet<Factor> _factor;

        public AdminPanelService(IUnitOfWork unitOfWork)
        {
            _products = unitOfWork.Set<Product>();
            _factor = unitOfWork.Set<Factor>();
        }

        public async Task<AdminDashboardViewModel> GetDashboardStatistics()
        {
            return new AdminDashboardViewModel
            {
                AvailableProductsCount =
                    await _products.Where(p => p.ProductStatus == ProductStatus.Available).CountAsync(),
                UnAvailableProductsCount =
                    await _products.Where(p => p.ProductStatus == ProductStatus.NotAvailable).CountAsync(),
                TotalProductsCount = await _products.CountAsync(),
                NewOrdersCount = await _factor.Where(q => q.Status == FactorStatus.Paid).CountAsync()
            };
        }
    }
}
