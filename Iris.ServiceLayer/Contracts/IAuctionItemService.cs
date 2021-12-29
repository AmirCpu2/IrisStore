using Iris.DomainClasses;
using VM = Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Iris.ServiceLayer.Contracts
{
    public interface IAuctionItemService
    {
        VM.AuctionItemViewModel Add(AuctionItem coinFactor);
        void Delete(int id);
        void Edit(AuctionItem coinFactor);
        Task<IList<VM.AuctionItemViewModel>> GetAllByUserId(int UserId);
        Task<IList<VM.AuctionItemViewModel>> GetAllWinnByUserId(int UserId);
        Task<IList<VM.AuctionItemViewModel>> GetAllPaging(int take, int skip);
        IQueryable<AuctionItem> GetAll_asQuery(Expression<Func<AuctionItem, bool>> expression = null);
        Task<VM.AuctionItemViewModel> GetOneById(int id);
        Task<IList<VM.AuctionItemViewModel>> GetTopViewAuctionItem(int Take);
        Task<IList<VM.AuctionItemViewModel>> GetNewAuctionItems(int Take);

    }
}
