using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Library.Data.Enumeration;
using Library.Data.Pages.Registration;
using Library.Data.Service;
using Library.Infrastructure;
using Library.ViewModel;


namespace Library.View {
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow {
        private int _code;
        private readonly ViewModelRegisterUser _viewModelRegisterUser;
        private readonly LoginService _loginService;
        private readonly ObservableCollection<object> _pages = new();
        public RegistrationWindow(ViewModelTheme viewModelTheme, ViewModelRegisterUser viewModelRegisterUser, LoginService loginService) {
            InitializeComponent();
            DataContext = viewModelTheme;
            _viewModelRegisterUser = viewModelRegisterUser;
            _loginService = loginService;
            _pages.Add(new FirstRegistrationPage(viewModelRegisterUser));
            _pages.Add(new SecondRegistrationPage(viewModelRegisterUser));
            _pages.Add(new ThirdRegistrationPage());
            PagesFrame.Content = _pages[0];
            FirstButtonChangeStep.Command = new RelayCommand(ExecuteFirstButton, CanExecuteFirstButton);
            SecondButtonChangeStep.Command = new RelayCommand(ExecuteSecondButton, CanExecuteSecondButton);
        }

        #region Window Control
        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) { DialogResult = false; }
        private void ButtonCollapse_OnClick(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }
        private void UIRegistration_OnMouseDown(object sender, MouseButtonEventArgs e) { DragMove(); }

        #endregion

        private void ExecuteFirstButton(object obj) {
            if(PagesFrame.Content.GetType() == typeof(FirstRegistrationPage)) {
                FirstButtonChangeStep.Content = "Step Back";
                SecondButtonChangeStep.Content = "Next Step";
                SecondButtonChangeStep.Visibility = Visibility.Visible;
                PagesFrame.Content = _pages[1];
            }
            else if(PagesFrame.Content.GetType() == typeof(SecondRegistrationPage)) {
                FirstButtonChangeStep.Content = "Next Step";
                SecondButtonChangeStep.Visibility = Visibility.Collapsed;
                PagesFrame.Content = _pages[0];
            }
        }
        private bool CanExecuteFirstButton(object obj) {
            if(PagesFrame.Content == null)
                return false;
            var user = _viewModelRegisterUser.GetUser;
            return !string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Surname) &&
                   !string.IsNullOrWhiteSpace(user.Phone) && !string.IsNullOrWhiteSpace(user.Address);
        }
        private void ExecuteSecondButton(object obj) {
            if(PagesFrame.Content.GetType() == typeof(SecondRegistrationPage)) {
                var existsLoginEmail = _loginService.IsExists(_viewModelRegisterUser.UserLogin, _viewModelRegisterUser.UserEmail);
                if(existsLoginEmail) {
                    MessageBox.Show("Login or Mail is already in use.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if(_viewModelRegisterUser.Confirmation != _viewModelRegisterUser.UserPassword) {
                    MessageBox.Show("Passwords do not match.\r\nPlease check and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                SecondButtonChangeStep.Content = "Confirm";
                FirstButtonChangeStep.Visibility = Visibility.Collapsed;
                PagesFrame.Content = _pages[2];
                _code = new Random().Next(100000, 999999);
                try {
                    MailService.SendMessage($"{_viewModelRegisterUser.GetUser.Email}", "Code", $"Code:{_code}");
                }
                catch(Exception e) { MessageBox.Show(e.Message); }
            }
            else if(PagesFrame.Content.GetType() == new ThirdRegistrationPage().GetType()) {
                if(( (ThirdRegistrationPage)_pages[2] ).TxtCode.Text != _code.ToString()) {
                    var messageBoxResult = MessageBox.Show("The code is incorrect, send a new one?", "Error"
                                                         , MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if(messageBoxResult != MessageBoxResult.Yes)
                        return;
                    _code = new Random().Next(100000, 999999);
                    try {
                        MailService.SendMessage($"{_viewModelRegisterUser.GetUser.Email}", "Code", $"Code:{_code}");
                    }
                    catch(Exception e) { MessageBox.Show(e.Message); }
                }
                else {
                    var user = _viewModelRegisterUser.GetUser;
                    DateTime.TryParse("2079-06-06 23:59:00", out var date);
                    user.SubscriptionName = SubscriptionNames.Default;
                    user.SubscriptionValidUntil = date;
                    var result = _loginService.Register(user);
                    if(result) {
                        MessageBox.Show("Success");
                        DialogResult = true;
                    }
                    else
                        MessageBox.Show("We were unable to add the user.\r\nPlease try again", "Error"
                                      , MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool CanExecuteSecondButton(object obj) {
            if(PagesFrame.Content == null)
                return false;
            if(PagesFrame.Content.GetType() != typeof(SecondRegistrationPage))
                return true;
            var secondPage = (SecondRegistrationPage) PagesFrame.Content;
            var user       = _viewModelRegisterUser.GetUser;
            return !string.IsNullOrWhiteSpace(user?.Login) && !string.IsNullOrWhiteSpace(user.Email) &&
                   !string.IsNullOrWhiteSpace(secondPage.TxtUserPassword.Password) &&
                   !string.IsNullOrWhiteSpace(secondPage.TxtConfirmation.Password);
        }
    }
}
