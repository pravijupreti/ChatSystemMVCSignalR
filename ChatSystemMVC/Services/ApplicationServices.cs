using Microsoft.EntityFrameworkCore;

namespace ChatSystemMVC.Services
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("newsql")));


            #region DI
            services.AddTransient<IChatServices, ChatServices>();
            services.AddSignalR();
            #endregion
            return services;
        }
    }
}
