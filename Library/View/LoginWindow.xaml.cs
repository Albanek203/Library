using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Library.Data.Enumeration;
using Library.Data.Models;
using Library.Data.Service;
using Library.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace Library.View {
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow {
        private readonly LoginService   _loginService;
        private readonly ViewModelTheme _viewModelTheme;
        private readonly UserService    _userService;
        private readonly MainWindow     _mainWindow;
        private          User           _user;
        public LoginWindow(ViewModelTheme viewModelTheme, LoginService loginService, UserService userService, User user
                         , MainWindow     mainWindow) {
            InitializeComponent();
            DataContext     = viewModelTheme;
            _loginService   = loginService;
            _viewModelTheme = viewModelTheme;
            _userService    = userService;
            _user           = user;
            _mainWindow     = mainWindow;
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
                _user = _userService.Find(new User { UserId = success });
                _mainWindow.Show();
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
        private void ThemeSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var themeId = ThemeSelector.SelectedIndex;
            _viewModelTheme.ChangeTheme((Themes)themeId);
        }
    }
}