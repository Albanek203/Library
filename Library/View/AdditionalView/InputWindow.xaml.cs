using System;
using System.Windows;
using Library.Data.Enumeration;

namespace Library.View.AdditionalView {
    public partial class InputWindow {
        public string Result;
        public InputWindow(string description, MessageButton messageButton) {
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
                case MessageButton.CancelApply:
                    FistButton.Visibility   = Visibility.Visible;
                    FistButton.Content      = "Apply";
                    SecondButton.Visibility = Visibility.Visible;
                    SecondButton.Content    = "Cancel";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageButton), messageButton, null);
            }
            TxtDescription.Text = description;
        }
        private void FirstButton_OnClick(object sender, RoutedEventArgs e) {
            Result       = TxtResult.Text;
            DialogResult = true;
        }
        private void SecondButton_OnClick(object sender, RoutedEventArgs e) { DialogResult = false; }
    }
}