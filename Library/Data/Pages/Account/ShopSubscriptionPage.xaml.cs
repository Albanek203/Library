using System.Windows;
using System.Windows.Controls;
using Library.Data.Enumeration;
using Library.Data.Models;
using Library.ViewModel;

namespace Library.Data.Pages.Account {
    public partial class ShopSubscriptionPage {
        private ViewModelUser _viewModelUser;
        public ShopSubscriptionPage(ViewModelUser viewModelUser) {
            InitializeComponent();
            _viewModelUser = viewModelUser;
        }
        private void BuyElement_OnClick(object sender, RoutedEventArgs e) {
            var button = (Button)sender;
            switch ((string)button.Content) {
                case "Silver" when _viewModelUser.Money < 200:
                    MessageWindow.Show(null, "Not enough money\r\nTop up and try again\r\nCost:200$"
                                     , TypeWindow.ErrorWindow, MessageButton.Ok);
                    break;
                case "Gold" when _viewModelUser.Money < 500:
                    MessageWindow.Show(null, "Not enough money\r\nTop up and try again\r\nCost:500$"
                                     , TypeWindow.ErrorWindow, MessageButton.Ok);
                    break;
                case "Diamond" when _viewModelUser.Money < 1000:
                    MessageWindow.Show(null, "Not enough money\r\nTop up and try again\r\nCost:1000$"
                                     , TypeWindow.ErrorWindow, MessageButton.Ok);
                    break;
                default:
                    MessageWindow.Show(null, $"You have subscribed to a \"{button.Content}\" subscription for a month"
                                     , TypeWindow.Information, MessageButton.Ok);
                    break;
            }
        }
    }
}