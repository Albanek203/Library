using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Library.Data.Models;
using Library.Data.Service;

namespace Library.Data.Pages.Library {
    public partial class BooksLibraryPage {
        private readonly BookService                _bookService;
        private readonly ObservableCollection<Book> _currentListBook;
        public BooksLibraryPage(BookService   bookService, BookCategoriesService bookCategoriesService
                              , AuthorService authorService) {
            InitializeComponent();
            _bookService = bookService;

            var lstBooks       = bookService?.FindAll(null);
            var lstCategories  = bookCategoriesService?.FindAll();
            var lstAuthors     = authorService?.FindAll();
            var lstNameAuthors = new ObservableCollection<string>();
            if (lstAuthors != null)
                foreach (var author in lstAuthors)
                    lstNameAuthors.Add(author.Name + "_" + author.Surname);

            _currentListBook = (ObservableCollection<Book>)lstBooks;
            CmbGroupByCategories.Items.Add("None");
            CmbGroupByAuthor.Items.Add("None");

            if (lstCategories != null) {
                foreach (var categories in lstCategories) { CmbGroupByCategories.Items.Add(categories); }
            }
            foreach (var author in lstNameAuthors) { CmbGroupByAuthor.Items.Add(author); }
            ShowBooks(lstBooks);
        }
        private void ShowBooks(IEnumerable<Book> lstBooks) {
            BooksPanel.Children.Clear();
            if (lstBooks == null) return;
            foreach (var children in lstBooks.Select(book => new Button {
                Style      = FindResource("ButtonBookStyle") as Style
              , Margin     = new Thickness(5)
              , Height     = 430
              , Width      = 300
              , Background = new ImageBrush { ImageSource = book.Image.Source }
              , Tag        = book
            })) {
                children.MouseEnter += BookImg_OnMouseEnter;
                children.MouseLeave += BookImg_OnMouseLeave;
                BooksPanel.Children.Add(children);
            }
        }
        private void BookImg_OnMouseEnter(object sender, MouseEventArgs e) {
            var button = (Button)sender;
            var book   = (Book)((Button)sender).Tag;
            var innerButton = new Button {
                Width             = 200
              , Height            = 50
              , Content           = "Buy This Book"
              , VerticalAlignment = VerticalAlignment.Bottom
              , Tag               = book
              , Style             = FindResource("ButtonStyle") as Style
            };
            innerButton.Click += BuyBook_OnClick;
            button.Content =
                new StackPanel {
                Children = {
                    new StackPanel {
                        Children = {
                            new TextBlock {
                                TextWrapping = TextWrapping.Wrap
                              , Text         = book.BookName
                              , Margin       = new Thickness(0, 0, 0, 10)
                              , Style        = FindResource("TitleText") as Style
                            }
                          , new TextBlock {
                                FontStyle     = FontStyles.Italic
                              , TextWrapping  = TextWrapping.Wrap
                              , TextAlignment = TextAlignment.Center
                              , Text          = "( " + book.AuthorName + " " + book.AuthorSurname + " )"
                              , Margin        = new Thickness(0, 0, 0, 10)
                              , Style         = FindResource("TextStyle") as Style
                            }
                          , new TextBlock {
                                FontStyle     = FontStyles.Italic
                              , TextWrapping  = TextWrapping.Wrap
                              , TextAlignment = TextAlignment.Center
                              , Text          = book.BookCategory
                              , Margin        = new Thickness(0, 0, 0, 10)
                              , Style         = FindResource("TextStyle") as Style
                            }
                          , new TextBlock {
                                FontStyle     = FontStyles.Italic
                              , TextWrapping  = TextWrapping.Wrap
                              , TextAlignment = TextAlignment.Center
                              , Text          = book.YearEdition.ToString()
                              , Margin        = new Thickness(0, 0, 0, 30)
                              , Style         = FindResource("TextStyle") as Style
                            }
                        }
                    }
                  , innerButton
                }
            };
        }
        // 
        //
        //  Must 
        //
        //
        private void BuyBook_OnClick(object sender, RoutedEventArgs e) { }
        private void BookImg_OnMouseLeave(object sender, MouseEventArgs e) {
            var button = (Button)sender;
            button.Content = "";
        }
        private void CmbGroupBy_OnSelectionChanged(object sender, SelectionChangedEventArgs e) { FilterBook(); }
        private void TxtSearchBook_OnTextChanged(object sender, TextChangedEventArgs e) {
            if (string.IsNullOrWhiteSpace(TxtSearchBook.Text) || CmbGroupByAuthor.SelectedIndex != 0 ||
                CmbGroupByCategories.SelectedIndex != 0) {
                FilterBook();
                return;
            }

            var tmpListBook = new ObservableCollection<Book>();
            foreach (var book in _currentListBook) {
                if (book.BookName.ToLower().Contains(TxtSearchBook.Text.ToLower())) tmpListBook.Add(book);
            }
            ShowBooks(tmpListBook);
        }
        private void FilterBook() {
            if (CmbGroupByAuthor.SelectedIndex > 0 && CmbGroupByCategories.SelectedIndex == 0) {
                var author = CmbGroupByAuthor.Items[CmbGroupByAuthor.SelectedIndex].ToString();
                ShowBooks(_bookService.FindAll(new Book {
                    AuthorName = author?.Split('_')[0], AuthorSurname = author?.Split('_')[1]
                }));
            }
            else if (CmbGroupByAuthor.SelectedIndex == 0 && CmbGroupByCategories.SelectedIndex > 0) {
                var category = CmbGroupByCategories.Items[CmbGroupByCategories.SelectedIndex].ToString();
                ShowBooks(_bookService?.FindAll(new Book { BookCategory = category }));
            }
            else if (CmbGroupByAuthor.SelectedIndex == 0 && CmbGroupByCategories.SelectedIndex == 0) {
                ShowBooks(_bookService?.FindAll(null));
            }
            else if (CmbGroupByAuthor.SelectedIndex > 0 && CmbGroupByCategories.SelectedIndex > 0) {
                var category = CmbGroupByCategories.Items[CmbGroupByCategories.SelectedIndex].ToString();
                var author   = CmbGroupByAuthor.Items[CmbGroupByAuthor.SelectedIndex].ToString();
                ShowBooks(_bookService?.FindAll(new Book {
                    BookCategory = category, AuthorName = author?.Split('_')[0], AuthorSurname = author?.Split('_')[1]
                }));
            }
            if (string.IsNullOrWhiteSpace(TxtSearchBook.Text)) return;
            var tmpListBook = new ObservableCollection<Book>();
            for (var i = 0; i < BooksPanel.Children.Count; i++) {
                var book = (Book)((Button)BooksPanel.Children[i]).Tag;

                if (book.BookName.ToLower().Contains(TxtSearchBook.Text.ToLower())) tmpListBook.Add(book);
            }
            ShowBooks(tmpListBook);
        }
    }
}