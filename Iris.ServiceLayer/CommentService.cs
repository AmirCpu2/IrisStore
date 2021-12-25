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
using Utilities;

namespace Iris.ServiceLayer
{
    public class CommentService : ICommentService
    {
        private readonly IMappingEngine _mappingEngine;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDbSet<Comment> _comment;

        public CommentService(IUnitOfWork unitOfWork, IMappingEngine mappingEngine)
        {
            _unitOfWork = unitOfWork;
            _mappingEngine = mappingEngine;
            _comment = unitOfWork.Set<Comment>();
        }
        

        public async Task<CommentViewModel> AddOrUpdate(CommentViewModel comment)
        {
            if(comment.Id > 0)
            {
                var oldComment = await _comment.FirstOrDefaultAsync(q => q.Id == comment.Id);

                //Change Text
                //oldComment.TextEdit = comment.Text; این درستشه ولی باید ادمینش رو پیاده کنم اول تا بتونه تایید کنه
                oldComment.Text = comment.Text;

                oldComment.IsEdited = true;
                oldComment.EditedDate = DateTime.Now;

                //oldComment.Status = (int)Enums.CommentState.PendingForEdit; این درستشه ولی باید ادمینش رو پیاده کنم اول تا بتونه تایید کنه
                oldComment.Status = (int)Enums.CommentState.Confirmed;

                oldComment.SuggestStatus = comment.SuggestStatus;

                oldComment.DesignRate = comment.DesignRate;
                oldComment.ConstructionQualityRate = comment.ConstructionQualityRate;
                oldComment.EaseOfUseRate = comment.EaseOfUseRate;
                oldComment.FeaturesRate = comment.FeaturesRate;
                oldComment.InnovationRate = comment.InnovationRate;
                oldComment.WorthBuyingRate = comment.WorthBuyingRate;

                _comment.Attach(oldComment);

                _unitOfWork.Entry(oldComment).State = EntityState.Modified;

                _unitOfWork.SaveAllChanges();

                return comment;

            }
            else
            {

                comment.ProductId = comment.ProductVM.Id;
                comment.IsQuestionAnswer = false;
                comment.CreateDate = DateTime.Now;

                //comment.Status = (int)Enums.CommentState.Pending; این درستشه ولی باید ادمینش رو پیاده کنم اول تا بتونه تایید کنه
                comment.Status = (int)Enums.CommentState.Confirmed;

                var v =  Mapper.Map <Comment ,CommentViewModel> ( _comment.Add(Mapper.Map<CommentViewModel, Comment>(comment)) );
                _unitOfWork.SaveAllChanges();

                return v;

            }
        }

        public async Task<List<Comment>> GetAllCommentByProductIdAsync(int productId)
        {
            if (productId < 1)
                return null;


            return await _comment.Where(q => q.Product.Id == productId).ToListAsync();
        }

        public async Task<CommentViewModel> GetAllCommentByProductIdAndUser(int productId, int userId)
        {
            if (productId < 1 || userId < 1)
                return null;

            return Mapper.Map<Comment, CommentViewModel>( await _comment.FirstOrDefaultAsync(q => q.Product.Id == productId && q.UserId == userId) );

        }

        public async Task<CommentViewModel> GetOneById(int id)
        {
            if (id < 1)
                return null;

            return Mapper.Map<Comment, CommentViewModel>( await _comment.FirstOrDefaultAsync(q => q.Id == id) );
        }

        public List<Comment> GetAllCommentByProductIdByState(int productId, int Status)
        {
            if (productId < 1)
                return null;


            return _comment.Where(q => q.Product.Id == productId && q.Status.Equals(Status)).ToList();
        }
    }
}
