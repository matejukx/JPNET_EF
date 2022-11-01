namespace JPNET_EF;

using Microsoft.EntityFrameworkCore;
using Repository;

public class Startup
{
    public Startup(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }

    private IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(config => config.AddProfile(typeof(AutomapperProfile)));
        // services.AddDbContext<ApplicationDbContext>(
        //     opt =>
        //        opt.UseMySQL("server=localhost;database=JPNET_EF;user=root;password=root;"));
        services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("EF"));
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(120);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("*");
                });
        });
    }

    public void Configure(IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        
    }
}