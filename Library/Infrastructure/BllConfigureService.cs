using Library.Data.Repository;
using Library.Data.Service;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure {
    public static class BllConfigureService {
        public static void ConfigureService(IServiceCollection services, string connectionStr) {
            services.AddTransient(x => new SqlConnection(connectionStr));
            services.AddTransient<LoginRepository>();
            services.AddTransient<LoginService>();
            services.AddTransient<UserRepository>();
            services.AddTransient<UserService>();
        }
    }
}