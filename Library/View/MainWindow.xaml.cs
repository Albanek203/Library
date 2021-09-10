using System.Windows;
using System.Windows.Media;
using Library.Data.Pages.Library;
using Library.View.AdditionalView;
using Library.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Library.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow(ViewModelUser viewModelUser, GlobalLibraryPage globalLibraryPage) {
            InitializeComponent();
            DataContext = viewModelUser;
            if (viewModelUser.CurrentUser.AdvancedAccess) ButtonAdminTools.Visibility = Visibility.Visible;
            PagesFrame.Content = globalLibraryPage;
            if (viewModelUser.CurrentUser.Image is not null)
                UserIco.Fill = new ImageBrush { ImageSource = viewModelUser.CurrentUser.Image.Source };
        }
        private void ButtonClose_OnClick(object    sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void ButtonCollapse_OnClick(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void ButtonGlobalPage_OnClick(object sender, RoutedEventArgs e) {
            var globalLibraryPage = App.ServiceProvider.GetService<GlobalLibraryPage>();
            PagesFrame.Content = globalLibraryPage;
        }
        private void ButtonBooksPage_OnClick(object sender, RoutedEventArgs e) {
            var booksLibraryPage = App.ServiceProvider.GetService<BooksLibraryPage>();
            PagesFrame.Content = booksLibraryPage;
        }
        private void ButtonAccount_OnClick(object sender, RoutedEventArgs e) {
            var accountLibraryPage = App.ServiceProvider.GetService<AccountLibraryPage>();
            PagesFrame.Content = accountLibraryPage;
        }
        private void ButtonAdministrator_OnClick(object sender, RoutedEventArgs e) {
            var administratorLibraryPage = App.ServiceProvider.GetService<AdministratorLibraryPage>();
            PagesFrame.Content = administratorLibraryPage;
        }
        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e) {
            Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 100 };
            var settings = App.ServiceProvider.GetService<SettingsWindow>();
            if (settings != null) {
                settings.Owner = this;
                settings.ShowDialog();
            }
            Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 0 };
        }
    }
}