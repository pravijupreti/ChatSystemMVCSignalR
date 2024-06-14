using ChatSystemMVC.ApplicationDBcontext;
using ChatSystemMVC.IServices;
using Microsoft.EntityFrameworkCore;

namespace ChatSystemMVC.Services
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext")));


            #region DI
            services.AddTransient<IChatServices, ChatServices>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSignalR();
            #endregion
            return services;
        }
    }
}
