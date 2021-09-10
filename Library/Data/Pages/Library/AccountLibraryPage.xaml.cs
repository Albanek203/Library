using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Library.Data.Enumeration;
using Library.Data.Pages.Account;
using Library.ViewModel;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace Library.Data.Pages.Library {
    public partial class AccountLibraryPage {
        private readonly ViewModelUser _viewModelUser;
        public AccountLibraryPage(ViewModelUser viewModelUser) {
            InitializeComponent();
            _viewModelUser = viewModelUser;
            DataContext    = _viewModelUser;

            var receivedBooksPage = App.ServiceProvider.GetService<ReceivedBooksPage>();
            AccountFrame.Content = receivedBooksPage;
            var user = _viewModelUser.CurrentUser;
            if (user.AdvancedAccess)
                AdvancedCrown.Content = new PackIcon {
                    Kind = PackIconKind.Crown, Width = 100, Height = 100, Foreground = Brushes.Gold
                };
            if (user.Image != null) UserIco.Background = new ImageBrush { ImageSource = user.Image.Source };
            switch (user.SubscriptionName) {
                case SubscriptionNames.Default:
                    SubColor.Fill = new SolidColorBrush(Color.FromRgb(239, 235, 233));
                    break;
                case SubscriptionNames.Silver:
                    var dropEffect = new DropShadowEffect { BlurRadius = 180, Color = Color.FromRgb(239, 220, 213) };
                    SubColor.Fill   = new SolidColorBrush(Color.FromRgb(207, 207, 207));
                    SubColor.Effect = dropEffect;
                    break;
                case SubscriptionNames.Gold:
                    dropEffect      = new DropShadowEffect { BlurRadius = 180, Color = Color.FromRgb(255, 248, 141) };
                    SubColor.Fill   = Brushes.Gold;
                    SubColor.Effect = dropEffect;
                    break;
                case SubscriptionNames.Diamond:
                    dropEffect      = new DropShadowEffect { BlurRadius = 180, Color = Colors.BlueViolet };
                    SubColor.Fill   = new SolidColorBrush(Color.FromRgb(108, 89, 168));
                    SubColor.Effect = dropEffect;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void UserIco_OnClick(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            _viewModelUser.ChangeImage(openFileDialog.FileName);
        }
        private void UserIco_OnDrop(object sender, DragEventArgs e) {
            if (e.Data is not DataObject data || !data.ContainsFileDropList()) return;
            var files = data.GetFileDropList();
            _viewModelUser.ChangeImage(files[0]);
        }
        private void OpenUserSetting_OnClick(object sender, RoutedEventArgs e) {
            var settingAccountPage = App.ServiceProvider.GetService<SettingAccountPage>();
            AccountFrame.Content = settingAccountPage;
        }
        private void OpenShopSubscription_OnClick(object sender, RoutedEventArgs e) {
            var shopSubscriptionPage = App.ServiceProvider.GetService<ShopSubscriptionPage>();
            AccountFrame.Content = shopSubscriptionPage;
        }
        private void OpenReceivedList_OnClick(object sender, RoutedEventArgs e) {
            var receivedBooksPage = App.ServiceProvider.GetService<ReceivedBooksPage>();
            AccountFrame.Content = receivedBooksPage;
        }
    }
}