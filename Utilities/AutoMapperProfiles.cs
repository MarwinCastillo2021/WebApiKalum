using AutoMapper;
using WebApiKalum.DTOs;
using WebApiKalum.Entities;
namespace WebApiKalum.Utilites
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CarreraTecnicaCreateDTO, CarreraTecnica>();  // Mapeo POST() : Conversion de <Class DTO a Tabla BD>
            CreateMap<CarreraTecnica,  CarreraTecnicaCreateDTO>(); // Mapeo GET() : Conversion de <Tabla BD a Class DTO>
            CreateMap<Jornada, JornadaCreateDTO>();              
            CreateMap<ExamenAdmision, ExamenAdmisionCreateDTO>();
            CreateMap<Aspirante, AspiranteListDTO>().ConstructUsing( e => new AspiranteListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<CarreraTecnica, ConsultaCarrerasListDTO>();
            CreateMap<Inscripcion, InscripcionConsultaCarrerasDTO>();
            CreateMap<Aspirante, AspiranteConsultaCarrerasDTO>().ConstructUsing(e => new AspiranteConsultaCarrerasDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Alumno, AlumnoListDTO>().ConstructUsing(e => new AlumnoListDTO{NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Inscripcion,InscripcionConsultaAlumnoDTO>();
            CreateMap<CuentaPorCobrar,CuentaPorCobrarConsultaAlumnoDTO>();
            CreateMap<Cargo,CargoListDTO>();
            CreateMap<CargoCreateDTO, Cargo>();
            CreateMap<ExamenAdmision,ExamenAdmisionListDTO>();
            CreateMap<ExamenAdmisionCreateDTO, ExamenAdmision>();
            CreateMap<Aspirante,AspiranteConsultaExamenAdmisionDTO>()
                    .ConstructUsing(e=>new AspiranteConsultaExamenAdmisionDTO
                    {NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<JornadaCreateDTO, Jornada>();
            CreateMap<Jornada, JornadaListDTO>();
            CreateMap<Aspirante, AspiranteConsultaJornadaDTO>().ConstructUsing(e=> new AspiranteConsultaJornadaDTO {NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Aspirante, InscripcionConsultaJornadaDTO>().ConstructUsing(e=> new InscripcionConsultaJornadaDTO {NombreCompleto = $"{e.Apellidos} {e.Nombres}"});
            CreateMap<Inscripcion,InscripcionConsultaJornadaDTO>();
            CreateMap<CuentaPorCobrar, CuentaPorCobrarListDTO>();
            CreateMap<InversionCarreraTecnica, InversionCarreraTecnicaListDTO>();



        }
    }
}