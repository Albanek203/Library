using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Library.Data.Interfaces;
using Library.Data.Models;
using Library.Data.Service;
using Microsoft.Data.SqlClient;

namespace Library.Data.Repository {
    public class BookRepository : IRepository<Book>, IRepositoryIsExists<string>, IRepositoryGetId<string> {
        private readonly SqlConnection         _sqlConnection;
        private readonly AuthorService         _authorService;
        private readonly BookCategoriesService _bookCategoriesService;
        public BookRepository(SqlConnection         sqlConnection, AuthorService authorService
                            , BookCategoriesService bookCategoriesService) {
            _sqlConnection         = sqlConnection;
            _authorService         = authorService;
            _bookCategoriesService = bookCategoriesService;
        }
        //
        public void Add(Book data) {
            // ==================== Checking / Select Author ====================

            var authorId = _authorService.GetId(data.AuthorName, data.AuthorSurname);
            if (authorId == 0) {
                _authorService.Add(data.AuthorName, data.AuthorSurname);
                authorId = _authorService.GetId(data.AuthorName, data.AuthorSurname);
            }

            // ==================== Select Category ====================

            var bookCategoriesId = _bookCategoriesService.GetId(data.BookCategory);

            // ==================== Insert Book ====================
            var sqlString =
                "INSERT Books([Name],[Image],[AuthorId],[BookCategoryId],[YearEdition]) OUTPUT INSERTED.Id " +
                $"VALUES (@BookName,@Content,@AuthorId,@BookCategoriesId,@YearEdition)";

            var cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@BookName",         data.BookName);
            cmd.Parameters.AddWithValue("@AuthorId",         authorId);
            cmd.Parameters.AddWithValue("@BookCategoriesId", bookCategoriesId);
            cmd.Parameters.AddWithValue("@YearEdition",      data.YearEdition);
            var param = cmd.Parameters.Add("@Content", SqlDbType.Image);
            param.Value = ImageConvert.FromBitmapImageToBytes((BitmapImage)data.Image.Source);
            _sqlConnection.Open();
            var booksId = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();

            // ==================== INSERT Last Add Book ====================
            sqlString = $"SELECT COUNT(*) FROM LastAddedBook";
            cmd       = new SqlCommand(sqlString, _sqlConnection);
            _sqlConnection.Open();
            var res = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            if (res == 0) {
                sqlString = "INSERT LastAddedBook([BooksId],[ShortDescription]) VALUES(@BookId,@ShortDescription)";
                cmd       = new SqlCommand(sqlString, _sqlConnection);
                cmd.Parameters.AddWithValue("@BookId",           booksId);
                cmd.Parameters.AddWithValue("@ShortDescription", data.Description);
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
                _sqlConnection.Close();
            }
            else {
                // ==================== Update Last Add Book ====================
                sqlString = $"UPDATE LastAddedBook SET BooksId = @BookId, ShortDescription = @ShortDescription";
                cmd       = new SqlCommand(sqlString, _sqlConnection);
                cmd.Parameters.AddWithValue("@BookId",           booksId);
                cmd.Parameters.AddWithValue("@ShortDescription", data.Description);
                _sqlConnection.Open();
                cmd.ExecuteNonQuery();
                _sqlConnection.Close();
            }
        }
        public Book Find(Book data) {
            if (!string.IsNullOrEmpty(data.Description)) {
                var sqlString   = $"SELECT BooksId,ShortDescription FROM LastAddedBook";
                var cmd         = new SqlCommand(sqlString, _sqlConnection);
                var lastBookId  = 0;
                var description = "";
                _sqlConnection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    lastBookId  = reader.GetInt32(0);
                    description = reader.GetString(1);
                }
                _sqlConnection.Close();
                var book = Find(new Book { BookId = lastBookId });
                book.Description = description;
                return book;
            }
            var bookId  = data.BookId != 0 ? data.BookId : GetId(data.BookName);
            var newBook = new Book();
            var nSqlString =
                $"SELECT Books.Name,Persons.Name,Persons.Surname,BookCategories.Name,Books.YearEdition,Image FROM Books " +
                "INNER JOIN Authors ON Authors.Id = Books.AuthorId " +
                "INNER JOIN Persons ON Persons.Id = Authors.PersonId " +
                $"INNER JOIN BookCategories ON BookCategories.Id = Books.BookCategoryId AND Books.Id = {bookId}";
            var command = new SqlCommand(nSqlString, _sqlConnection);
            _sqlConnection.Open();
            var readered = command.ExecuteReader();
            while (readered.Read()) {
                var imgBytes  = (byte[])readered["Image"];
                var newBitmap = ImageConvert.FromBytesToBitmapImage(imgBytes);
                newBook.BookName      = readered.GetString(0);
                newBook.AuthorName    = readered.GetString(1);
                newBook.AuthorSurname = readered.GetString(2);
                newBook.BookCategory  = readered.GetString(3);
                newBook.YearEdition   = readered.GetInt32(4);
                newBook.Image         = new Image { Source = newBitmap };
            }
            _sqlConnection.Close();
            return newBook;
        }
        public IEnumerable<Book> FindAll(Book data) {
            var           listBooks = new ObservableCollection<Book>();
            string        sqlString;
            SqlCommand    cmd;
            SqlDataReader reader;
            if (data == null) {
                listBooks = new ObservableCollection<Book>();
                sqlString =
                    $"SELECT Books.Name,Persons.Name,Persons.Surname,BookCategories.Name,Books.YearEdition,Image FROM Books " +
                    "INNER JOIN Authors ON Authors.Id = Books.AuthorId " +
                    "INNER JOIN Persons ON Persons.Id = Authors.PersonId " +
                    "INNER JOIN BookCategories ON BookCategories.Id = Books.BookCategoryId";
                cmd = new SqlCommand(sqlString, _sqlConnection);
                _sqlConnection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var imgBytes  = (byte[])reader["Image"];
                    var newBitmap = ImageConvert.FromBytesToBitmapImage(imgBytes);
                    var book = new Book {
                        BookName      = reader.GetString(0)
                      , AuthorName    = reader.GetString(1)
                      , AuthorSurname = reader.GetString(2)
                      , BookCategory  = reader.GetString(3)
                      , YearEdition   = reader.GetInt32(4)
                      , Image         = new Image { Source = newBitmap }
                    };
                    listBooks.Add(book);
                }
                _sqlConnection.Close();
                return listBooks;
            }

            if (!string.IsNullOrEmpty(data.BookCategory) && string.IsNullOrEmpty(data.AuthorName) &&
                string.IsNullOrEmpty(data.AuthorSurname)) {
                sqlString =
                    $"SELECT Books.Name,Persons.Name,Persons.Surname,BookCategories.Name,Books.YearEdition,Image FROM Books " +
                    "INNER JOIN Authors ON Authors.Id = Books.AuthorId " +
                    "INNER JOIN Persons ON Persons.Id = Authors.PersonId " +
                    $"INNER JOIN BookCategories ON BookCategories.Id = Books.BookCategoryId  WHERE BookCategories.Name = @BookCategory";
                cmd = new SqlCommand(sqlString, _sqlConnection);
                cmd.Parameters.AddWithValue("@BookCategory", data.BookCategory);
                _sqlConnection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var imgBytes  = (byte[])reader["Image"];
                    var newBitmap = ImageConvert.FromBytesToBitmapImage(imgBytes);
                    var book = new Book {
                        BookName      = reader.GetString(0)
                      , AuthorName    = reader.GetString(1)
                      , AuthorSurname = reader.GetString(2)
                      , BookCategory  = reader.GetString(3)
                      , YearEdition   = reader.GetInt32(4)
                      , Image         = new Image { Source = newBitmap }
                    };
                    listBooks.Add(book);
                }
                _sqlConnection.Close();
                return listBooks;
            }
            if (!string.IsNullOrEmpty(data.AuthorName) && !string.IsNullOrEmpty(data.AuthorSurname) &&
                string.IsNullOrEmpty(data.BookCategory)) {
                listBooks = new ObservableCollection<Book>();
                sqlString =
                    $"SELECT Books.Name,Persons.Name,Persons.Surname,BookCategories.Name,Books.YearEdition,Image FROM Books " +
                    "INNER JOIN Authors ON Authors.Id = Books.AuthorId " +
                    "INNER JOIN Persons ON Persons.Id = Authors.PersonId " +
                    $"INNER JOIN BookCategories ON BookCategories.Id = Books.BookCategoryId  WHERE Persons.Name = @AuthorName and Persons.Surname = @AuthorSurname";
                cmd = new SqlCommand(sqlString, _sqlConnection);
                cmd.Parameters.AddWithValue("@AuthorName",    data.AuthorName);
                cmd.Parameters.AddWithValue("@AuthorSurname", data.AuthorSurname);
                _sqlConnection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var imgBytes  = (byte[])reader["Image"];
                    var newBitmap = ImageConvert.FromBytesToBitmapImage(imgBytes);
                    var book = new Book {
                        BookName      = reader.GetString(0)
                      , AuthorName    = reader.GetString(1)
                      , AuthorSurname = reader.GetString(2)
                      , BookCategory  = reader.GetString(3)
                      , YearEdition   = reader.GetInt32(4)
                      , Image         = new Image { Source = newBitmap }
                    };
                    listBooks.Add(book);
                }
                _sqlConnection.Close();
                return listBooks;
            }
            if (string.IsNullOrEmpty(data.BookCategory) || string.IsNullOrEmpty(data.AuthorName) ||
                string.IsNullOrEmpty(data.AuthorSurname))
                return null;
            {
                listBooks = new ObservableCollection<Book>();
                sqlString =
                    $"SELECT Books.Name,Persons.Name,Persons.Surname,BookCategories.Name,Books.YearEdition,Image FROM Books " +
                    "INNER JOIN Authors ON Authors.Id = Books.AuthorId " +
                    "INNER JOIN Persons ON Persons.Id = Authors.PersonId " +
                    "INNER JOIN BookCategories ON BookCategories.Id = Books.BookCategoryId " +
                    $"WHERE Persons.Name = @AuthorName and Persons.Surname = @AuthorSurname and BookCategories.Name = @BookCategory";
                cmd = new SqlCommand(sqlString, _sqlConnection);
                cmd.Parameters.AddWithValue("@AuthorName",    data.AuthorName);
                cmd.Parameters.AddWithValue("@AuthorSurname", data.AuthorSurname);
                cmd.Parameters.AddWithValue("@BookCategory",  data.BookCategory);
                _sqlConnection.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var imgBytes  = (byte[])reader["Image"];
                    var newBitmap = ImageConvert.FromBytesToBitmapImage(imgBytes);
                    var book = new Book {
                        BookName      = reader.GetString(0)
                      , AuthorName    = reader.GetString(1)
                      , AuthorSurname = reader.GetString(2)
                      , BookCategory  = reader.GetString(3)
                      , YearEdition   = reader.GetInt32(4)
                      , Image         = new Image { Source = newBitmap }
                    };
                    listBooks.Add(book);
                }
                _sqlConnection.Close();
                return listBooks;
            }
        }
        public bool IsExists(string data) {
            if (data == null) return false;
            var sqlString = $"SELECT COUNT(*) FROM Books WHERE Books.Name = @BookName";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@BookName", data);
            _sqlConnection.Open();
            var res = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            return res != 0;
        }
        public int GetId(string data) {
            if (!IsExists(data)) { return -1; }

            var sqlString = $"SELECT Id FROM Books WHERE Name = @BookName";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@BookName", data);
            _sqlConnection.Open();
            var res = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            return res;
        }
    }
}