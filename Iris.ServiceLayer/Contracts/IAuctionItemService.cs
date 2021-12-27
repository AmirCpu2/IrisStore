using Iris.DomainClasses;
using VM = Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer.Contracts
{
    public interface IAuctionItemService
    {
        VM.AuctionItemViewModel Add(AuctionItem coinFactor);
        void Delete(int id);
        void Edit(AuctionItem coinFactor);
        Task<IList<VM.AuctionItemViewModel>> GetAllByUserId(int UserId);
        Task<VM.AuctionItemViewModel> GetOneById(int id);

    }
}
