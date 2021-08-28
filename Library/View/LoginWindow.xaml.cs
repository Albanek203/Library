using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Library.Data.Enumeration;
using Library.Data.Service;
using Library.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Library.View {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow {
        private readonly LoginService       _loginService;
        public LoginWindow(ViewModelTheme viewModelTheme, LoginService loginService) {
            InitializeComponent();
            DataContext = viewModelTheme;
            _loginService = loginService;
        }

        #region Window Control
        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void ButtonCollapse_OnClick(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void UILogin_OnMouseDown(object sender, MouseButtonEventArgs e) { if(e.LeftButton == MouseButtonState.Pressed) DragMove(); }

        #endregion

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e) {
            if(string.IsNullOrWhiteSpace(TxtUserLogin.Text) || string.IsNullOrWhiteSpace(TxtUserPassword.Password)) {
                MessageBox.Show("You have not entered a value");
                return;
            }

            var success = _loginService.Login(TxtUserLogin.Text, TxtUserPassword.Password);
            if(success > 0) {
                new MainWindow().Show();
                Close();
            }
            else {
                MessageBox.Show("Invalid Login or Password.\nPlease try again.");
            }
        }
        private void CreateAccount_OnMouseDown(object sender, MouseButtonEventArgs e) {
            Hide();
            var registrationWindow = App.ServiceProvider.GetService<RegistrationWindow>();
            registrationWindow?.ShowDialog();
            Show();
        }
        private void ThemeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var themeId        = ThemeSelector.SelectedIndex;
            var viewModelTheme = App.ServiceProvider.GetService<ViewModelTheme>();
            viewModelTheme?.ChangeTheme((Themes)themeId);
        }
    }
}
