using AutoMapper;
using Iris.DataLayer;
using Iris.DomainClasses;
using Iris.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer
{
    public class BidHistoryService : IBidHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<BidHistory> _bidHistory;

        public BidHistoryService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _bidHistory = _unitOfWork.Set<BidHistory>();
            _mappingEngine = mappingEngine;
        }

        public BidHistory Add(BidHistory bidHistory)
        {
            _bidHistory.Add(bidHistory);

            _unitOfWork.SaveAllChanges();

            return bidHistory;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(AuctionItem bidHistory)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BidHistory>> GetAllByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BidHistory>> GetAllByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<BidHistory>> GetAllPaging(int take, int skip)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BidHistory> GetAll_asQuery(Expression<Func<AuctionItem, bool>> expression = null)
        {
            return (expression == null) ? _bidHistory : _bidHistory.AsQueryable();
        }

        public Task<BidHistory> GetOneById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
