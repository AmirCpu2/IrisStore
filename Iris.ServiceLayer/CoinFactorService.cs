﻿using AutoMapper;
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
    public class CoinFactorService : ICoinFactorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingEngine _mappingEngine;
        private readonly IDbSet<CoinFactor> _coinFactor;

        public CoinFactorService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _coinFactor = unitOfWork.Set<CoinFactor>();
            _mappingEngine = mappingEngine;
        }

        public CoinFactorViewModel Add(CoinFactor coinFactor)
        {
            if (coinFactor == null)
                return null;

            coinFactor.Id = Guid.NewGuid();

            var entity = _coinFactor.Add(coinFactor);

            _unitOfWork.SaveAllChanges();

            return _mappingEngine.Map<CoinFactor, CoinFactorViewModel>(entity);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Edit(CoinFactor coinFactor)
        {
            if (coinFactor?.Id == null)
                return ;


            var oldItem = _coinFactor.FirstOrDefault(q => q.Id == coinFactor.Id);

            //Edit coinFactor

            oldItem.StatusId = coinFactor.StatusId;

            oldItem.RefId = coinFactor.RefId;

            oldItem.BuyDate = coinFactor.BuyDate;

            _coinFactor.Attach(oldItem);

            _unitOfWork.Entry(oldItem).State = EntityState.Modified;

            _unitOfWork.SaveAllChanges();

        }

        public async Task<IList<CoinFactorViewModel>> GetAllByUserId(int UserId)
        {
            var coinList = _coinFactor.Where(q=> q.UserId == UserId);

            var coinFactorViewModelList = await coinList.ToListAsync();

            return coinFactorViewModelList.Select(q => Mapper.Map<CoinFactor, CoinFactorViewModel>(q)).ToList();
        }

        public async Task<CoinFactorViewModel> GetOneById(Guid id)
        {
            var coinFactor = await _coinFactor.Where(q => q.Id == id)
                                        .FirstOrDefaultAsync();

            return Mapper.Map<CoinFactor, CoinFactorViewModel>(coinFactor);
        }
    }
}
