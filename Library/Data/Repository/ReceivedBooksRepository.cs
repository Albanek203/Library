using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Library.Data.Interfaces;
using Library.Data.Models;
using Library.Data.Service;
using Microsoft.Data.SqlClient;

namespace Library.Data.Repository {
    public class ReceivedBooksRepository : IRepository<ReceivedBook> {
        private readonly BookService   _bookService;
        private readonly UserService   _userService;
        private readonly SqlConnection _sqlConnection;
        public ReceivedBooksRepository(SqlConnection sqlConnection, BookService bookService, UserService userService) {
            _sqlConnection = sqlConnection;
            _bookService   = bookService;
            _userService   = userService;
        }
        //
        public void Add(ReceivedBook data) {
            var bookId = _bookService.GetId(data.Book.BookName);
            if (bookId == -1) { return; }

            var sqlString = $"SELECT COUNT(*) FROM ReceivedBooks WHERE UserId = @UserId and BookId = @BookId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", data.User.UserId);
            cmd.Parameters.AddWithValue("@BookId", bookId);
            _sqlConnection.Open();
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            if (count == 1) { return; }

            sqlString = "INSERT ReceivedBooks([BookId],[UserId],[IssueDate],[DeliveryDate]) VALUES " +
                        $"(@BookId,@UserId,@IssueDate,@DeliveryDate)";
            cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@BookId",       bookId);
            cmd.Parameters.AddWithValue("@UserId",       data.User.UserId);
            cmd.Parameters.AddWithValue("@IssueDate",    data.ReceivingDate);
            cmd.Parameters.AddWithValue("@DeliveryDate", data.DeliveryDate);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public ReceivedBook Find(ReceivedBook data) { return null; }
        public IEnumerable<ReceivedBook> FindAll(ReceivedBook data) {
            var returnIssuance = new ObservableCollection<ReceivedBook>();
            if (data.User.UserId != 0) {
                var sqlString =
                    $"SELECT BookId,ReceivingDate,DeliveryDate FROM ReceivedBooks WHERE UserId = {data.User.UserId}";
                var cmd = new SqlCommand(sqlString, _sqlConnection);
                _sqlConnection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var issuance = new ReceivedBook {
                        Book          = _bookService.Find(new Book { BookId = reader.GetInt32(0) })
                      , User          = _userService.Find(new User { UserId = data.User.UserId })
                      , ReceivingDate = reader.GetDateTime(1)
                      , DeliveryDate  = reader.GetDateTime(2)
                    };
                    returnIssuance.Add(issuance);
                }
                _sqlConnection.Close();
                return returnIssuance;
            }
            else {
                var sqlString = $"SELECT BookId,UserId,ReceivingDate,DeliveryDate FROM ReceivedBooks";
                var cmd       = new SqlCommand(sqlString, _sqlConnection);
                _sqlConnection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    var issuance = new ReceivedBook {
                        Book          = _bookService.Find(new Book { BookId = reader.GetInt32(0) })
                      , User          = _userService.Find(new User { UserId = reader.GetInt32(1) })
                      , ReceivingDate = reader.GetDateTime(2)
                      , DeliveryDate  = reader.GetDateTime(3)
                    };
                    returnIssuance.Add(issuance);
                }
                _sqlConnection.Close();
                return returnIssuance;
            }
        }
    }
}