using System.Windows;
using Library.Data.Enumeration;
using Library.Data.Service;
using Library.View.AdditionalView;
using Library.ViewModel;

namespace Library.Data.Pages.Account {
    public partial class SettingAccountPage {
        private readonly UserService   _userService;
        private readonly ViewModelUser _viewModelUser;
        public SettingAccountPage(UserService userService, ViewModelUser viewModelUser) {
            InitializeComponent();
            _userService   = userService;
            _viewModelUser = viewModelUser;
        }
        private void ChangeName_OnClick(object sender, RoutedEventArgs e) {
            var inputWindow = new InputWindow("Enter new name", MessageButton.CancelApply);
            if (inputWindow.ShowDialog() != true) { return; }
            _userService.ChangeName(_viewModelUser.CurrentUser.UserId, inputWindow.Result);
            _viewModelUser.ChangeName(inputWindow.Result);
        }
        private void ChangeSurname_OnClick(object sender, RoutedEventArgs e) {
            var inputWindow = new InputWindow("Enter new surname", MessageButton.CancelApply);
            if (inputWindow.ShowDialog() != true) { return; }
            _userService.ChangeSurname(_viewModelUser.CurrentUser.UserId, inputWindow.Result);
            _viewModelUser.ChangeSurname(inputWindow.Result);
        }
        private void ChangeLogin_OnClick(object sender, RoutedEventArgs e) {
            var inputWindow = new InputWindow("Enter new login", MessageButton.CancelApply);
            if (inputWindow.ShowDialog() != true) { return; }
            _userService.ChangeLogin(_viewModelUser.CurrentUser.UserId, inputWindow.Result);
            _viewModelUser.ChangeLogin(inputWindow.Result);
        }
        private void ChangePassword_OnClick(object sender, RoutedEventArgs e) {
            var inputWindow = new InputWindow("Enter new login", MessageButton.CancelApply);
            if (inputWindow.ShowDialog() != true) { return; }
            _userService.ChangePassword(_viewModelUser.CurrentUser.UserId, inputWindow.Result);
            _viewModelUser.ChangePassword(inputWindow.Result);
        }
    }
}