using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Library.Data.Interfaces;
using Library.Data.Models;
using Microsoft.Data.SqlClient;

namespace Library.Data.Repository {
    public class AuthorRepository : IRepository<Author>,IRepositoryGetId<Author> {
        private readonly SqlConnection _sqlConnection;
        public AuthorRepository(SqlConnection sqlConnection) { _sqlConnection = sqlConnection; }
        //
        public void Add(Author data) {
            // ==================== Insert new Person ====================
            var sqlString = "INSERT Persons([Name], [Surname]) OUTPUT INSERTED.Id VALUES (@Name,@Surname)";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Name",    data.Name);
            cmd.Parameters.AddWithValue("@Surname", data.Surname);
            _sqlConnection.Open();
            var id = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();

            // ==================== Insert new Author ====================
            sqlString = "INSERT Authors([PersonId]) VALUES (@PersonId)";
            cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@PersonId", id);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public Author Find(Author data) {
            var newAuthor = new Author();
            var sqlString = $"SELECT Persons.Name,Persons.Surname FROM Authors inner join Persons ON " +
                            $"Persons.Id = Authors.PersonId and Persons.Name = @Name and Persons.Surname = @Surname";
            var cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Name",    data.Name);
            cmd.Parameters.AddWithValue("@Surname", data.Surname);
            _sqlConnection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                newAuthor.Name    = reader.GetString(0);
                newAuthor.Surname = reader.GetString(1);
            }
            _sqlConnection.Close();
            return newAuthor;
        }
        public IEnumerable<Author> FindAll(Author data) {
            var listAuthor = new ObservableCollection<Author>();
            var sqlString = $"SELECT Persons.Name,Persons.Surname FROM Authors " +
                            "INNER JOIN Persons ON Persons.Id = Authors.PersonId";
            var command = new SqlCommand(sqlString, _sqlConnection);
            _sqlConnection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read()) {
                var author = new Author {Name = reader.GetString(0), Surname = reader.GetString(1)};
                listAuthor.Add(author);
            }
            _sqlConnection.Close();
            return listAuthor;
        }
        public int GetId(Author data) {
            var sqlString = $"SELECT Authors.Id FROM Authors inner join Persons ON " +
                            $"Persons.Id = Authors.PersonId and Persons.Name = @Name and Persons.Surname = @Surname";
            var cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Name",    data.Name);
            cmd.Parameters.AddWithValue("@Surname", data.Surname);
            _sqlConnection.Open();
            var id = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            return id;
        }
    }
}