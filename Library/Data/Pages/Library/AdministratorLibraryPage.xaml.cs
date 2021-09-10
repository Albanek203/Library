using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Library.Data.Enumeration;
using Library.Data.Models;
using Library.Data.Service;
using Library.View.AdditionalView;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Data.Pages.Library {
    public partial class AdministratorLibraryPage {
        private readonly BookCategoriesService _bookCategoriesService;
        private readonly BookService           _bookService;
        private readonly UserService           _userService;
        private          List<AdminUser>       _allUsers = new();
        public AdministratorLibraryPage(UserService           userService, BookService bookService
                                      , BookCategoriesService bookCategoriesService) {
            InitializeComponent();
            _bookCategoriesService = bookCategoriesService;
            _bookService           = bookService;
            _userService           = userService;

            var listUsers = userService.FindAll();
            foreach (var user in listUsers) {
                if (user.AdvancedAccess) continue;
                _allUsers.Add(new AdminUser {
                    Id = user.UserId, Name = user.Name, Surname = user.Surname, Email = user.Email
                });
            }
            ListBoxUser.ItemsSource = _allUsers;
            var view = (CollectionView)CollectionViewSource.GetDefaultView(ListBoxUser.ItemsSource);
            view.Filter = UserFilter;
        }
        private bool UserFilter(object item) {
            if (string.IsNullOrEmpty(TxtUserLogin.Text)) return true;
            return ((AdminUser)item).Name.IndexOf(TxtUserLogin.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }
        private void ButtonAddBook_OnClick(object sender, RoutedEventArgs e) {
            var fillBookWindow = App.ServiceProvider.GetService<FillBookWindow>();
            if (fillBookWindow?.ShowDialog() != true) { return; }

            if (_bookService.IsExists(fillBookWindow.Book.BookName)) {
                MessageWindow.Show(null, "This book already exists", TypeWindow.ErrorWindow, MessageButton.Ok);
                return;
            }

            MessageBox.Show(fillBookWindow.Book.YearEdition.ToString());
            _bookService.Add(fillBookWindow.Book);
        }
        private void ButtonAddCategory_OnClick(object sender, RoutedEventArgs e) {
            var inputWindow = new InputWindow("Enter category name:", MessageButton.CancelApply);
            if (inputWindow.ShowDialog() != true) { return; }
            _bookCategoriesService.Add(inputWindow.Result);
        }
        private void TxtSearchUser_OnTextChanged(object sender, TextChangedEventArgs e) {
            CollectionViewSource.GetDefaultView(ListBoxUser.ItemsSource).Refresh();
        }
        private void AddAdvancedAccess_OnClick(object sender, RoutedEventArgs e) {
            var currentUser = (AdminUser)ListBoxUser.Items[ListBoxUser.SelectedIndex];
            _userService.AddAdvancedAccess(currentUser.Id);
            var listUsers = _userService.FindAll();
            _allUsers = new List<AdminUser>();
            foreach (var user in listUsers) {
                if (user.AdvancedAccess) continue;
                _allUsers.Add(new AdminUser {
                    Id = user.UserId, Name = user.Name, Surname = user.Surname, Email = user.Email
                });
            }
            ListBoxUser.ItemsSource = _allUsers;
            var view = (CollectionView)CollectionViewSource.GetDefaultView(ListBoxUser.ItemsSource);
            view.Filter = UserFilter;
        }
    }
}