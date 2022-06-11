using Microsoft.EntityFrameworkCore;
using WebApiKalum.Entities;

namespace WebApiKalum
{
    public class KalumDbContext : DbContext
    {
        public DbSet<CarreraTecnica> CarreraTecnica { get; set; }
        public DbSet<Jornada> Jornada { get; set; }
        public DbSet<ExamenAdmision> ExamenAdmision { get; set; }
        public DbSet<Aspirante> Aspirante { get; set; } // por cada relacion a realizarse
        public DbSet<Inscripcion> Inscripcion { get; set; }
        public DbSet<InscripcionPago> InscripcionPago { get; set; }
        public DbSet<ResultadoExamenAdmision> ResultadoExamenAdmision { get; set; }
        public DbSet<InversionCarreraTecnica> InversionCarreraTecnica { get; set; }
        public DbSet<CuentaPorCobrar> CuentaPorCobrar { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Alumno> Alumno { get; set; }

        //#nullable disable warnings
        public KalumDbContext(DbContextOptions options) : base(options)
        {

        }
        //#nullable enable warnings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarreraTecnica>().ToTable("CarreraTecnica").HasKey(ct => new { ct.CarreraId });
            modelBuilder.Entity<Jornada>().ToTable("Jornada").HasKey(j => new { j.JornadaId });
            modelBuilder.Entity<ExamenAdmision>().ToTable("ExamenAdmision").HasKey(ex => new { ex.ExamenId });
            modelBuilder.Entity<Aspirante>().ToTable("Aspirante").HasKey(a => new { a.NoExpediente });
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion").HasKey(i => new { i.InscripcionId });
            modelBuilder.Entity<Alumno>().ToTable("Alumno").HasKey(al => new { al.Carne });
            modelBuilder.Entity<InscripcionPago>().ToTable("InscripcionPago").HasKey(ip => new { ip.BoletaPago, ip.Anio, ip.NoExpediente });
            modelBuilder.Entity<ResultadoExamenAdmision>().ToTable("ResultadoExamenAdmision").HasKey(r => new { r.NoExpediente, r.Anio });
            modelBuilder.Entity<InversionCarreraTecnica>().ToTable("InversionCarreraTecnica").HasKey(ict => new{ ict.InversionId});
            modelBuilder.Entity<CuentaPorCobrar>().ToTable("CuentaPorCobrar").HasKey(cxc => new{cxc.Cargo, cxc.Carne, cxc.Anio} );
            modelBuilder.Entity<Cargo>().ToTable("Cargo").HasKey(c => new{c.CargoId});

            modelBuilder.Entity<Aspirante>()  // estructura que relaciona las tablas de uno a muchos
                .HasOne<CarreraTecnica>(a => a.CarreraTecnica) // de uno 
                .WithMany(ct => ct.Aspirantes) // a muchos
                .HasForeignKey(a => a.CarreraId); // llave foranea en aspirantes

            modelBuilder.Entity<Aspirante>()
                .HasOne<Jornada>(a => a.Jornada)
                .WithMany(j => j.Aspirantes)
                .HasForeignKey(a => a.JornadaId);

            modelBuilder.Entity<Aspirante>()
                .HasOne<ExamenAdmision>(a => a.ExamenAdmision)
                .WithMany(ex => ex.Aspirantes)
                .HasForeignKey(a => a.ExamenId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<CarreraTecnica>(i => i.CarreraTecnica)
                .WithMany(ct => ct.Inscripciones)
                .HasForeignKey(i => i.CarreraId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Jornada>(i => i.Jornada)
                .WithMany(j => j.Inscripciones)
                .HasForeignKey(i => i.JornadaId);

            modelBuilder.Entity<Inscripcion>()
                .HasOne<Alumno>(i => i.Alumno)
                .WithMany(al => al.Inscripciones)
                .HasForeignKey(i => i.Carne);

            modelBuilder.Entity<InscripcionPago>()
                .HasOne<Aspirante>(ip => ip.aspirantes)
                .WithMany(a => a.InscripcionesPago)
                .HasForeignKey(ip => ip.NoExpediente);

            modelBuilder.Entity<ResultadoExamenAdmision>()
                .HasOne<Aspirante>(r => r.aspirantes)
                .WithMany(a => a.ResultadosExamenAdmision)
                .HasForeignKey(r => r.NoExpediente);

            modelBuilder.Entity<InversionCarreraTecnica>()
                .HasOne<CarreraTecnica>(ict => ict.CarreraTecnica)
                .WithMany(ct => ct.InversionesCarreraTecnica)
                .HasForeignKey(ict => ict.CarreraId);

            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Alumno>(cxc => cxc.Alumno)
                .WithMany(al => al.CuentasPorCobrar)
                .HasForeignKey(cxc => cxc.Carne);

            modelBuilder.Entity<CuentaPorCobrar>()
                .HasOne<Cargo>(cxc => cxc.Cargos)
                .WithMany(c => c.CuentasPorCobrar)
                .HasForeignKey(cxc => cxc.CargoId);

        }
    }
}

