using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
            _auctionItem = _unitOfWork.Set<AuctionItem>();
            _mappingEngine = mappingEngine;
        }

        public AuctionItemViewModel Add(AuctionItem auctionItem)
        {
            if (auctionItem == null)
                return null;

            var entity = _auctionItem.Add(auctionItem);

            _unitOfWork.SaveAllChanges();

            return _mappingEngine.Map<AuctionItem, AuctionItemViewModel>(entity);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(AuctionItem auctionItem)
        {
            if (auctionItem?.Id == null)
                return;
            var oldItem = _auctionItem.FirstOrDefault(q=> q.Id == auctionItem.Id);

            //Edit AuctionItem


            oldItem.StartDate = auctionItem.StartDate;
            oldItem.StopDate = auctionItem.StopDate;
            oldItem.IsDelete = auctionItem.IsDelete;
            oldItem.IsEdit = auctionItem.IsEdit;

            if(auctionItem.ImageAddress != null)
            {
                oldItem.ImageName = auctionItem.ImageName;
                oldItem.ImageAddress = auctionItem.ImageAddress;

            }
            
            oldItem.Body = auctionItem.Body;
            oldItem.Title = auctionItem.Title;
            oldItem.EditDate = auctionItem.EditDate;
            oldItem.MaxPrice = auctionItem.MaxPrice;
            oldItem.MiniPrice = auctionItem.MiniPrice;
            oldItem.WinUserId = auctionItem.WinUserId;

           
            _auctionItem.Attach(oldItem);

            _unitOfWork.Entry(oldItem).State = EntityState.Modified;

            _unitOfWork.SaveAllChanges();
        }

        public async Task<IList<AuctionItemViewModel>> GetAllPaging(int take, int skip)
        {
           var auctionItemList = await _auctionItem.OrderBy(q=>q.Id).Skip(skip).Take(take).ToListAsync();

            return auctionItemList.Select(q => Mapper.Map<AuctionItem, AuctionItemViewModel>(q)).ToList();
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

        public IQueryable<AuctionItem> GetAll_asQuery(Expression<Func<AuctionItem, bool>> expression = null)
        {
            return (expression == null) ? _auctionItem : _auctionItem.Where(expression);
        }
    }
}
