using System.Windows;
using System.Windows.Input;
using Library.Data.Enumeration;
using Library.Data.Models;
using Library.Data.Service;
using Library.View.AdditionalView;
using Library.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Library.View {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow {
        private readonly ViewModelUser _viewModelUser;
        private readonly LoginService  _loginService;
        private readonly UserService   _userService;
        private readonly User          _user;
        public LoginWindow(ViewModelUser viewModelUser, LoginService loginService, UserService userService, User user) {
            InitializeComponent();
            _loginService  = loginService;
            _userService   = userService;
            _user          = user;
            _viewModelUser = viewModelUser;
        }

#region Window Control
        private void ButtonClose_OnClick(object    sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        private void ButtonCollapse_OnClick(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void UILogin_OnMouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
#endregion

        private void ButtonLogin_OnClick(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(TxtUserLogin.Text) || string.IsNullOrWhiteSpace(TxtUserPassword.Password)) {
                MessageWindow.Show(this, "You have not entered a value", TypeWindow.ErrorWindow, MessageButton.Ok);
                return;
            }

            var success = _loginService.Login(TxtUserLogin.Text, TxtUserPassword.Password);
            if (success > 0) {
                var user = _userService.Find(new User { UserId = success });
                _viewModelUser.SetUser(user);
                var mainWindow = App.ServiceProvider.GetService<MainWindow>();
                mainWindow?.Show();
                Close();
                return;
            }
            MessageWindow.Show(this, "Invalid Login or Password.\nPlease try again.", TypeWindow.ErrorWindow
                             , MessageButton.Ok);
        }
        private void CreateAccount_OnMouseDown(object sender, MouseButtonEventArgs e) {
            Hide();
            var registrationWindow = App.ServiceProvider.GetService<RegistrationWindow>();
            registrationWindow?.ShowDialog();
            Show();
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