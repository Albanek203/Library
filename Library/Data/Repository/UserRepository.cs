using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Library.Data.Enumeration;
using Library.Data.Interfaces;
using Library.Data.Models;
using Microsoft.Data.SqlClient;

namespace Library.Data.Repository {
    public class UserRepository : IRepository<User>, IRepositoryIsExists<int> {
        private readonly SqlConnection _sqlConnection;
        public UserRepository(SqlConnection sql_sqlConnection) { _sqlConnection = sql_sqlConnection; }
        //
        public void Add(User data) {
            /*Ignore*/
        }
        public User Find(User data) {
            var userId   = data.UserId;
            var user     = new User { UserId = userId };
            var imgBytes = new object(); // Image

            // ==================== Checking the existence of the User ====================
            if (!IsExists(userId)) { return null; }

            // ==================== Select User ====================
            var str = "SELECT Persons.Name,Persons.Surname,LoginData.Email,LoginData.Login,LoginData.Password," +
                      "Subscriptions.SubscriptionsNameId,Subscriptions.ValidUntil,Users.Address,Users.Phone,AdvancedAccess,Image,Money FROM Users " +
                      $"inner join Persons on Persons.Id = Users.PersonId and Users.Id = {userId} " +
                      "inner join LoginData on LoginData.Id = Users.LoginDataId " +
                      "inner join Subscriptions on Subscriptions.Id = Users.SubscriptionsId";

            var cmd = new SqlCommand(str, _sqlConnection);
            _sqlConnection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                imgBytes                    = reader["Image"];
                user.Name                   = reader.GetString(0);
                user.Surname                = reader.GetString(1);
                user.Email                  = reader.GetString(2);
                user.Login                  = reader.GetString(3);
                user.Password               = Encipher.Decrypt(reader.GetString(4), 24);
                user.SubscriptionName       = (SubscriptionNames)reader.GetInt32(5);
                user.SubscriptionValidUntil = reader.GetDateTime(6);
                user.Address                = reader.GetString(7);
                user.Phone                  = reader.GetString(8);
                user.AdvancedAccess         = reader.GetBoolean(9);
                user.Money                  = reader.GetSqlMoney(11).ToInt32();
            }
            _sqlConnection.Close();

            // ==================== If the User without a photo - return ====================
            if (imgBytes is DBNull) return user;

            // ==================== Set User photo ====================
            var newBitmap = ImageConvert.FromBytesToBitmapImage((byte[])imgBytes);
            user.Image = new Image { Source = newBitmap };
            return user;
        }
        public IEnumerable<User> FindAll(User data) {
            var listUsers = new List<User>();

            var str = "SELECT Persons.Name,Persons.Surname,LoginData.Email,LoginData.Login,LoginData.Password," +
                      "Subscriptions.SubscriptionsNameId,Subscriptions.ValidUntil,Users.Address,Users.Phone,AdvancedAccess,Image,Users.Id FROM Users " +
                      $"inner join Persons on Persons.Id = Users.PersonId " +
                      "inner join LoginData on LoginData.Id = Users.LoginDataId " +
                      "inner join Subscriptions on Subscriptions.Id = Users.SubscriptionsId";
            var cmd = new SqlCommand(str, _sqlConnection);
            _sqlConnection.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                var imgBytes = reader["Image"]; // Image
                var user = new User {
                    Name                   = reader.GetString(0)
                  , Surname                = reader.GetString(1)
                  , Email                  = reader.GetString(2)
                  , Login                  = reader.GetString(3)
                  , Password               = Encipher.Decrypt(reader.GetString(4), 24)
                  , SubscriptionName       = (SubscriptionNames)reader.GetInt32(5)
                  , SubscriptionValidUntil = reader.GetDateTime(6)
                  , Address                = reader.GetString(7)
                  , Phone                  = reader.GetString(8)
                  , AdvancedAccess         = reader.GetBoolean(9)
                  , UserId                 = reader.GetInt32(11)
                };
                if (imgBytes is DBNull) {
                    listUsers.Add(user);
                    continue;
                }

                // ==================== Set User photo ====================
                var newBitmap = ImageConvert.FromBytesToBitmapImage((byte[])imgBytes);
                user.Image = new Image { Source = newBitmap };
                listUsers.Add(user);
            }
            _sqlConnection.Close();
            return listUsers;
        }
        public bool IsExists(int userId) {
            var sqlString = $"SELECT COUNT(*) FROM Users WHERE Id = @UserId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", userId);
            _sqlConnection.Open();
            var res = Convert.ToInt32(cmd.ExecuteScalar());
            _sqlConnection.Close();
            return res != 0;
        }
        public void AddAdvancedAccess(int userId) {
            var sqlString = $"UPDATE Users SET AdvancedAccess = '1' WHERE Users.Id = {userId}";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public void ChangePhoto(int userId, string url) {
            var imgBytes  = ImageConvert.FromBitmapImageToBytes(new BitmapImage(new Uri(url)));
            var sqlString = $"UPDATE Users SET [Image] = @Content WHERE Id = @UserId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", userId);
            var param = cmd.Parameters.Add("@Content", SqlDbType.Image);
            param.Value = imgBytes;
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public void ChangeName(int userId, string newName) {
            // ==================== Select Person Id ====================
            var sqlString = $"SELECT PersonId FROM Users WHERE Id = @UserId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", userId);
            _sqlConnection.Open();
            var personId = (int)cmd.ExecuteScalar();
            _sqlConnection.Close();

            // ==================== Update User Name ====================
            sqlString = $"UPDATE Persons SET [Name] = @NewName WHERE Id = @PersonId";
            cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@NewName",  newName);
            cmd.Parameters.AddWithValue("@PersonId", personId);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public void ChangeSurname(int userId, string newSurname) {
            // ==================== Select Person Id ====================
            var sqlString = $"SELECT PersonId FROM Users WHERE Id = @UserId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", userId);
            _sqlConnection.Open();
            var personId = (int)cmd.ExecuteScalar();
            _sqlConnection.Close();

            // ==================== Update User Surname ====================
            sqlString = $"UPDATE Persons SET [Surname] = @NewSurname WHERE Id = @PersonId";
            cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@NewSurname", newSurname);
            cmd.Parameters.AddWithValue("@PersonId",   personId);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public void ChangeLogin(int userId, string newLogin) {
            // ==================== Select Login Data Id ====================
            var sqlString = $"SELECT LoginDataId FROM Users WHERE Id = @UserId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", userId);
            _sqlConnection.Open();
            var loginDataId = (int)cmd.ExecuteScalar();
            _sqlConnection.Close();

            // ==================== Update User Login ====================
            sqlString = $"UPDATE LoginData SET [Login] = @NewLogin WHERE Id = @LoginDataId";
            cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@NewLogin",    newLogin);
            cmd.Parameters.AddWithValue("@LoginDataId", loginDataId);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
        public void ChangePassword(int userId, string newPassword) {
            // ==================== Select Login Data Id ====================
            var sqlString = $"SELECT LoginDataId FROM Users WHERE Id = @UserId";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@UserId", userId);
            _sqlConnection.Open();
            var loginDataId = (int)cmd.ExecuteScalar();
            _sqlConnection.Close();

            // ==================== Update User Password ====================
            sqlString = $"UPDATE LoginData SET [Password] = @Password WHERE Id = @LoginDataId";
            cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Password",    Encipher.Encrypt(newPassword, 24));
            cmd.Parameters.AddWithValue("@LoginDataId", loginDataId);
            _sqlConnection.Open();
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
        }
    }
}