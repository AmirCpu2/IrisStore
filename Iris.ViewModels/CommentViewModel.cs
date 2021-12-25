using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Iris.ViewModels
{
    public class CommentViewModel : IHaveCustomMappings
    {

        public CommentViewModel()
        {

        }
            

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int? UserId { get; set; }

        public DateTime? RegesterDate { get; set; }

        public int Status { get; set; } = (int)Enums.CommentState.Pending;

        public string StatusFa { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateFa => CreateDate != null ? PersianDateUtils.ToPersianDate(CreateDate) : string.Empty;

        public bool IsDeleted { get; set; } = false;

        public bool IsEdited { get; set; } = false;

        public string TextEdit { get; set; }

        public int? ParentId { get; set; }

        public DateTime? EditedDate { get; set; }

        public bool IsQuestionAnswer { get; set; } = false;

        public int? PostId { get; set; }

        public int? ProductId { get; set; }

        public Nullable<int> SuggestStatus { get; set; } = (int) Enums.SuggestStatus.Nop;


        public Nullable<Enums.SuggestStatus> SuggestStatusEnum => (Enums.SuggestStatus) SuggestStatus;

        public int? ConstructionQualityRate { get; set; } = (int) Enums.Rate.Normal;

        public int? WorthBuyingRate { get; set; } = (int) Enums.Rate.Normal;

        public int? InnovationRate { get; set; } = (int) Enums.Rate.Normal;

        public int? FeaturesRate { get; set; } = (int) Enums.Rate.Normal;

        public int? EaseOfUseRate { get; set; } = (int) Enums.Rate.Normal;

        public int? DesignRate { get; set; } = (int) Enums.Rate.Normal;

        public ProductPageViewModel ProductVM { get; set; }


        public void CreateMappings(IConfiguration configuration)
        {

            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(x => x.ProductId,op => op.MapFrom(q=> q.Product.Id ))
                //.ForMember(x => x.PostId,op => op.MapFrom(q=> q.Post.Id ))
                //.ForMember(x => x.Status,op => op.MapFrom(q=> q.status ))
                
                ;
            
            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(x => x.ProductId,op => op.MapFrom(q=> q.Product.Id ))
                //.ForMember(x => x.PostId,op => op.MapFrom(q=> q.Post.Id ))
                //.ForMember(x => x.Status, op => op.MapFrom(q => q.status))
                .ReverseMap()
                ;



        }

    }
}
