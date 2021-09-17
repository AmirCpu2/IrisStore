using AutoMapper;
using AutoMapperContracts;
using Iris.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iris.ViewModels
{
    public class UserInfoWidgetViewModel : IHaveCustomMappings

    {

        public UserInfoWidgetViewModel()
        {

        }


        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string PostalCode { get; set; }

        public string Address { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual string ThumbnailUrl { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserDataGridViewModel>();
        }
    }


}
