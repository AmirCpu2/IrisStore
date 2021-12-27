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
    public class AuctionItemService : IAuctionItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<AuctionItem> _auctionItem;

        public AuctionItemService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _auctionItem = unitOfWork.Set<AuctionItem>();
            _mappingEngine = mappingEngine;
        }

        public AuctionItemViewModel Add(AuctionItem coinFactor)
        {
            if (coinFactor == null)
                return null;

            var entity = _auctionItem.Add(coinFactor);

            _unitOfWork.SaveAllChanges();

            return _mappingEngine.Map<AuctionItem, AuctionItemViewModel>(entity);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(AuctionItem auctionItem)
        {
            if (auctionItem?.Id != null)
                return;

            _auctionItem.Attach(auctionItem);

            _unitOfWork.SaveAllChanges();
        }

        public async Task<IList<AuctionItemViewModel>> GetAllByUserId(int UserId)
        {
            var auctionItemList = _auctionItem.Where(q => q.PostedByUserId == UserId);

            var coinFactorViewModelList = await auctionItemList.ToListAsync();

            return coinFactorViewModelList.Select(q => Mapper.Map<AuctionItem, AuctionItemViewModel>(q)).ToList();
        }

        public async Task<AuctionItemViewModel> GetOneById(int id)
        {
            var auctionItem = await _auctionItem.Where(q => q.Id == id)
                                        .FirstOrDefaultAsync();

            return Mapper.Map<AuctionItem, AuctionItemViewModel>(auctionItem);
        }
    }
}
