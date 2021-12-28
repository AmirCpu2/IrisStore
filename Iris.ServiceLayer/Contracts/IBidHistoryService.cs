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
    public interface IBidHistoryService
    {
        BidHistory Add(BidHistory bidHistory);
        void Delete(int id);
        void Edit(AuctionItem bidHistory);
        Task<IList<BidHistory>> GetAllByUserId(int userId);
        Task<IList<BidHistory>> GetAllByProductId(int productId);
        Task<IList<BidHistory>> GetAllPaging(int take, int skip);
        IQueryable<BidHistory> GetAll_asQuery(Expression<Func<AuctionItem, bool>> expression = null);
        Task<BidHistory> GetOneById(int id);
    }
}
