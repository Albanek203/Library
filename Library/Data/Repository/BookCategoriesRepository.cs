using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Library.Data.Interfaces;
using Microsoft.Data.SqlClient;

namespace Library.Data.Repository {
    public class BookCategoriesRepository : IRepository<string>, IRepositoryGetId<string> {
        private readonly SqlConnection _sqlConnection;
        public BookCategoriesRepository(SqlConnection sqlConnection) { _sqlConnection = sqlConnection; }
        //
        public void Add(string data) {
            var sqlString = $"INSERT BookCategories([Name]) VALUES (@Data)";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Data", data);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public string Find(string data) { return string.Empty; }
        public IEnumerable<string> FindAll(string data) {
            var listCategories = new ObservableCollection<string>();
            var sqlString      = $"SELECT Name FROM BookCategories";
            var cmd            = new SqlCommand(sqlString, _sqlConnection);
            _sqlConnection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read()) { listCategories.Add(reader.GetString(0)); }
            _sqlConnection.Close();
            return listCategories;
        }
        public int GetId(string data) {
            var sqlString = $"SELECT Id FROM BookCategories WHERE Name = @Name";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Name", data);
            _sqlConnection.Open();
            var id = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            return id;
        }
    }
}