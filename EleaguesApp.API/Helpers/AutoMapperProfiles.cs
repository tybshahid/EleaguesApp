using System.IO;
using System.Linq;
using AutoMapper;
using EleaguesApp.API.Dtos;
using EleaguesApp.API.Models;

namespace EleaguesApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<League, League>();
            CreateMap<League, LeagueForListDto>()
                .ForMember(dest => dest.RulesFilePath, opt =>
                {
                    opt.MapFrom(d => Directory.GetCurrentDirectory() + @"\Uploads\Rules\" + d.RulesFileName);
                });
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<Game, GameForListDto>()
                .ForMember(d => d.League, opt => opt
                    .MapFrom(s => s.League.Name))
                .ForMember(d => d.Winner, opt => opt
                    .MapFrom(s => s.Winner.KnownAs))
                .ForMember(d => d.Opponent, opt => opt
                    .MapFrom(s => s.Opponent.KnownAs))
                .ForMember(d => d.WinnerPhotoUrl, opt => opt
                    .MapFrom(s => s.Winner.PhotoUrl));
        }
    }
}