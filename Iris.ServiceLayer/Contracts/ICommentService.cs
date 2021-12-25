using Iris.DomainClasses;
using Iris.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ServiceLayer.Contracts
{
    public interface ICommentService
    {

        //addOrUpdate
        Task<CommentViewModel> AddOrUpdate(CommentViewModel comment);


        //Get one by id
        Task<CommentViewModel> GetOneById(int id);

        //GetAllCommentByProductIdAndUser
        Task<CommentViewModel> GetAllCommentByProductIdAndUser(int productId, int userId);

        //get ALL cm for ProductId
        Task<List<Comment>> GetAllCommentByProductIdAsync(int productId);

        List<Comment> GetAllCommentByProductIdByState(int productId, int Status);
    }
}
