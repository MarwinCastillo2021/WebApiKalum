using AutoMapper;
using WebApiKalum.DTOs;
using WebApiKalum.Entities;
namespace WebApiKalum.Utilites
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();
        }
    }
}