using AutoMapper;
using Publications.Domain.Entities;
using Publications.MVC.ViewModels;

namespace Publications.MVC.Infrastructure.AutoMapper;

public class AuthorsProfile : Profile
{
    public AuthorsProfile()
    {
        CreateMap<Person, AuthorViewModel>()
           //.ForMember(p => p.Name, opt => opt.MapFrom(vm => vm.Name))
           .ReverseMap();
    }
}