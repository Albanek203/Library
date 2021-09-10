using System;
using System.Configuration;
using System.Windows;
using Library.Data.Models;
using Library.Data.Pages.Account;
using Library.Data.Pages.Library;
using Library.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Library.View;
using Library.View.AdditionalView;
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
            BllConfigureService.ConfigureService(serviceProvider
                                               , ConfigurationManager.ConnectionStrings["testConnection"].ConnectionString);
            serviceProvider.AddSingleton<ViewModelTheme>();
            serviceProvider.AddSingleton<User>();
            serviceProvider.AddSingleton<ViewModelUser>();

            serviceProvider.AddTransient<ViewModelRegisterUser>();
            serviceProvider.AddTransient<RegistrationWindow>();
            // Pages
            serviceProvider.AddTransient<GlobalLibraryPage>();
            serviceProvider.AddTransient<AdministratorLibraryPage>();
            serviceProvider.AddTransient<BooksLibraryPage>();
            serviceProvider.AddTransient<AccountLibraryPage>();
            serviceProvider.AddTransient<ReceivedBooksPage>();
            serviceProvider.AddTransient<SettingAccountPage>();
            serviceProvider.AddTransient<ShopSubscriptionPage>();
            // End
            serviceProvider.AddTransient<FillBookWindow>();
            serviceProvider.AddTransient<SettingsWindow>();
            serviceProvider.AddTransient<MainWindow>();
            serviceProvider.AddTransient<LoginWindow>();
        }
        private void App_OnStartup(object sender, StartupEventArgs e) {
            // Initialize themes
            var viewModelTheme = ServiceProvider.GetService<ViewModelTheme>();
            var loginWindow    = ServiceProvider.GetService<LoginWindow>();
            loginWindow?.Show();
        }
    }
}