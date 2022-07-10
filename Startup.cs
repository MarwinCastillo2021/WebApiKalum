using Microsoft.EntityFrameworkCore;
using WebApiKalum.Utilites;

namespace WebApiKalum
{
    public class Startup
    {
        public IConfiguration Configuration{get;}
        public Startup(IConfiguration _Configuration)
        {
            this.Configuration = _Configuration;
        }
        public void ConfigureServices (IServiceCollection _services)
        {
           _services.AddTransient<ActionFilter>();
            _services.AddControllers(options=>options.Filters.Add(typeof(ErrorFilterException)));
            _services.AddAutoMapper(typeof(Startup));
            _services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            _services.AddDbContext<KalumDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            _services.AddEndpointsApiExplorer();
            _services.AddSwaggerGen();
            // servicios solo en ambiente de desarrollo
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints=> {
                endpoints.MapControllers();
            });
        }
    }
}