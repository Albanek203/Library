using System;
using System.Windows;
using Library.Data.Enumeration;

namespace Library.View.AdditionalView {
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow {
        public InformationWindow(string information, MessageButton messageButton) {
            InitializeComponent();
            switch (messageButton) {
                case MessageButton.Ok:
                    FistButton.Visibility = Visibility.Visible;
                    FistButton.Content    = messageButton.ToString();
                    break;
                case MessageButton.YesNo:
                    FistButton.Visibility   = Visibility.Visible;
                    FistButton.Content      = "Yes";
                    SecondButton.Visibility = Visibility.Visible;
                    SecondButton.Content    = "No";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageButton), messageButton, null);
            }
            TxtInformation.Text = information;
        }
        private void FirstButton_OnClick(object  sender, RoutedEventArgs e) { DialogResult = true; }
        private void SecondButton_OnClick(object sender, RoutedEventArgs e) { DialogResult = false; }
    }
}