using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Library.Data.Enumeration;
using Library.Data.Models;
using Library.Data.Service;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Library.View.AdditionalView {
    public partial class FillBookWindow : Window {
        public readonly Book Book = new Book();
        public FillBookWindow(BookCategoriesService bookCategoriesService) {
            InitializeComponent(); 

            var lstCategories = bookCategoriesService.FindAll();
            if (lstCategories == null) return;
            foreach (var categories in lstCategories) { CmbCategories.Items.Add(categories); }
        }
        private void UIFillBook_OnMouseDown(object sender, MouseButtonEventArgs e) { DragMove(); }
        private void BookImg_OnClick(object sender, RoutedEventArgs e) {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true) return;
            Book.Image         = new Image { Source           = new BitmapImage(new Uri(openFileDialog.FileName)) };
            BookImg.Background = new ImageBrush { ImageSource = Book.Image.Source };
        }
        private void BookImg_OnMouseEnter(object sender, MouseEventArgs e) {
            BookImg.Content = new PackIcon { Kind = PackIconKind.Upload, Width = 50, Height = 50 };
        }
        private void BookImg_OnMouseLeave(object sender, MouseEventArgs e) { BookImg.Content = ""; }
        private void UIElement_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtBookName_OnTextChanged(object sender, TextChangedEventArgs e) {
            Book.BookName = TxtBookName.Text;
        }
        private void TxtAuthorName_OnTextChanged(object sender, TextChangedEventArgs e) {
            Book.AuthorName = TxtAuthorName.Text;
        }
        private void TxtAuthorSurname_OnTextChanged(object sender, TextChangedEventArgs e) {
            Book.AuthorSurname = TxtAuthorSurname.Text;
        }
        private void CmbCategories_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            var category = CmbCategories.Items[CmbCategories.SelectedIndex].ToString();
            Book.BookCategory = category;
        }
        private void TxtYearEdition_OnTextChanged(object sender, TextChangedEventArgs e) {
            int.TryParse(TxtYearEdition.Text, out var year);
            Book.YearEdition = year;
        }
        private void TxtShortDescription_OnTextChanged(object sender, TextChangedEventArgs e) {
            if (TxtShortDescription.Text.Contains("'"))
                TxtShortDescription.Text = TxtShortDescription.Text.Replace("'", "");
            Book.Description = TxtShortDescription.Text;
        }
        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) { DialogResult = false; }
        private void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(TxtBookName.Text) || string.IsNullOrEmpty(TxtAuthorName.Text) ||
                string.IsNullOrEmpty(TxtAuthorSurname.Text) || string.IsNullOrEmpty(TxtYearEdition.Text) ||
                string.IsNullOrEmpty(TxtShortDescription.Text) || CmbCategories.SelectedIndex == -1 ||
                Book.Image == null) {
                MessageWindow.Show(null, "Not all cells are full", TypeWindow.ErrorWindow, MessageButton.Ok);
                return;
            }
            DialogResult = true;
        }
        private void BookImg_OnDrop(object sender, DragEventArgs e) {
            if (!(e.Data is DataObject data) || !data.ContainsFileDropList()) return;
            var files = data.GetFileDropList();
            Book.Image         = new Image { Source           = new BitmapImage(new Uri(files[0])) };
            BookImg.Background = new ImageBrush { ImageSource = Book.Image.Source };
        }
    }
}