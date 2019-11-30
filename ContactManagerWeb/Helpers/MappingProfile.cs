using System.Collections.Generic;
using AutoMapper;
using ContactManagerWeb.Data.Paging;
using ContactManagerWeb.Models;
using ContactManagerWeb.ViewModels;

namespace ContactManagerWeb.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add mapping here...
            CreateDataMap<Contact, ContactViewModel>();
            CreateDataMap<Paginate<Contact>, Paginate<ContactViewModel>>();
        }

        // Generic method to create mapping both ways
        public void CreateDataMap<TOne, TTwo>()
        {
            CreateMap<TOne, TTwo>();
            CreateMap<TTwo, TOne>();
        }
    }
}