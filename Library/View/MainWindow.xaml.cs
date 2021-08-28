using System.Windows;
using System.Windows.Media;
using Library.Data.Models;
using Library.Data.Pages.Library;

namespace Library.View {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow(User user) {
            InitializeComponent();
            DataContext = user;

            if (user.AdvancedAccess) ButtonAdminTools.Visibility = Visibility.Visible;
            PagesFrame.Content = new GlobalLibraryPage();
            if (user.Image is not null) UserIco.Fill = new ImageBrush { ImageSource = user.Image.Source };
            UserName.Text = user.Name;
        }
        private void ButtonClose_OnClick(object    sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void ButtonCollapse_OnClick(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void ButtonGlobalPage_OnClick(object sender, RoutedEventArgs e) {
            /*var globalLibraryPage = new GlobalLibraryPage();
            PagesFrame.Content = globalLibraryPage;*/
        }
        private void ButtonBooksPage_OnClick(object sender, RoutedEventArgs e) {
            /*PagesFrame.Content = new BooksLibraryPage(_user);*/
        }
        private void ButtonAccount_OnClick(object sender, RoutedEventArgs e) {
            /*var accountLibraryPage = new AccountLibraryPage(_user) {Tag = this};
            PagesFrame.Content = accountLibraryPage;*/
        }
        private void ButtonAdministrator_OnClick(object sender, RoutedEventArgs e) {
            /*PagesFrame.Content = new AdministratorLibraryPage();*/
        }
    }
}