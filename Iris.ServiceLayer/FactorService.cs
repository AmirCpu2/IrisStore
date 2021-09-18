using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer
{
    public class FactorService : IFactorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<Factor> _Factor;
        private readonly IDbSet<ApplicationUser> _userFavoriteProduct;

        public FactorService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _Factor = unitOfWork.Set<Factor>();
            _userFavoriteProduct = unitOfWork.Set<ApplicationUser>();
        }

        public virtual async Task<List<Iris.ViewModels.ListFactorViewModel>> GetListFactorsByUserId(int userId,int take)
        {

            var factorList = await _Factor.Where(q => q.UserId.Equals(userId)).OrderByDescending(q=>q.Id).Take(take).ToListAsync();

            return factorList.Select(Mapper.Map<Iris.ViewModels.ListFactorViewModel>).ToList();
        }

        public virtual async Task<Iris.ViewModels.ListFactorViewModel> GetFactorById(int id)
        {

            var factor = await _Factor.Where(q => q.Id.Equals(id)).Include(q=>q.Products).FirstOrDefaultAsync();

            return Mapper.Map<Iris.ViewModels.ListFactorViewModel>(factor);
        }
    }
}
