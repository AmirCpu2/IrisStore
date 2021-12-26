using Iris.DomainClasses;
using VM = Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer.Contracts
{
    public interface ICoinFactorService
    {
        VM.CoinFactorViewModel Add(CoinFactor coinFactor);
        void Delete(Guid id);
        void Edit(CoinFactor coinFactor);
        Task<IList<VM.CoinFactorViewModel>> GetAllByUserId(int UserId);
        Task<VM.CoinFactorViewModel> GetOneById(Guid id);
    }
}
