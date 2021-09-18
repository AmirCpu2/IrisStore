using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer.Contracts
{
    public interface IFactorService
    {
        Task<List<Iris.ViewModels.ListFactorViewModel>> GetListFactorsByUserId(int userId, int take);

        Task<Iris.ViewModels.ListFactorViewModel> GetFactorById(int id);


    }
}
