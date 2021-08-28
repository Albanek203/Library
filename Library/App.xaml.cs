using System;
using System.Configuration;
using System.Windows;
using Library.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Library.View;
using Library.ViewModel;

namespace Library {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        public static readonly IServiceProvider ServiceProvider;
        static App() {
            var servicesCollection = new ServiceCollection();
            ConfigServices(servicesCollection);
            ServiceProvider = servicesCollection.BuildServiceProvider();
        }
        private static void ConfigServices(IServiceCollection serviceProvider) {
            BllConfigureService.ConfigureService(serviceProvider, ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString);
            serviceProvider.AddSingleton<ViewModelTheme>();
            serviceProvider.AddTransient<LoginWindow>();
        }

        private void App_OnStartup(object sender, StartupEventArgs e) {
            var loginWindow = ServiceProvider.GetService<LoginWindow>();
            loginWindow?.Show();
        }
    }
}
