using Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer.Contracts
{
    public interface IFavoriteService
    {

        Task<bool> GetFavoriteProductState(int userId, int productId);

        Task AddFavoriteProduct(int userId, int productId);

        Task RemoveFavoriteProduct(int userId, int productId);

        Task<List<FavoriteProductMiniWidget>> GetAllFavoriteProduct(int userId, int takeCount);

    }

}