using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<UserFavoritePost> _userFavoritePost;
        private readonly IDbSet<UserFavoriteProduct> _userFavoriteProduct;

        public FavoriteService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _userFavoritePost = unitOfWork.Set<UserFavoritePost>();
            _userFavoriteProduct = unitOfWork.Set<UserFavoriteProduct>();
        }

        public async Task<bool> GetFavoriteProductState(int userId,int productId)
        {
            if (userId < 1 || productId < 1)
                return false;

            var result = await _userFavoriteProduct.AsQueryable().Where(q => q.ProductId.Equals(productId) && q.UserId.Equals(userId)).AnyAsync();

            return result;
        }

        public async Task AddFavoriteProduct(int userId, int productId)
        {
            if (userId < 1 || productId < 1)
                return;

            if (await _userFavoriteProduct.AnyAsync(q => q.ProductId.Equals(productId) && q.UserId.Equals(userId)))
                return;

            _userFavoriteProduct.Add(new UserFavoriteProduct{
                DateTime = DateTime.Now,
                ProductId = productId,
                UserId = userId
            });
        }


        public async Task RemoveFavoriteProduct(int userId, int productId)
        {
            if (userId < 1 || productId < 1)
                return;

            var UserPF = await _userFavoriteProduct.FirstOrDefaultAsync(q => q.ProductId.Equals(productId) && q.UserId.Equals(userId));

            if (UserPF==null)
                return;

            _userFavoriteProduct.Remove(UserPF);
        }

        public async Task<List<FavoriteProductMiniWidget>> GetAllFavoriteProduct(int userId,int takeCount)
        {
            if (userId < 1)
                return null;

            var result = await _userFavoriteProduct.AsQueryable().Where(q => q.UserId.Equals(userId)).Take(takeCount).Select(q=> q.Product)
                .ToListAsync();

            return result.Select(q => Mapper.Map<FavoriteProductMiniWidget>(q)).ToList();
        }
    }
}
