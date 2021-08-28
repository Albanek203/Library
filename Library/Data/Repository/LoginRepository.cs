using System;
using System.Data;
using Library.Data.Models;
using Microsoft.Data.SqlClient;

namespace Library.Data.Repository {
    public class LoginRepository {
        private readonly SqlConnection _sqlConnection;
        public LoginRepository(SqlConnection sqlConnection) { _sqlConnection = sqlConnection; }
        //
        public int Login(string login, string password) {
            if(login.Contains("@")) {
                var cmd = new SqlCommand("SelectUserByLoginDataEmail", _sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", login);
                cmd.Parameters.AddWithValue("@Password", Encipher.Encrypt(password, 24));
                _sqlConnection.Open();
                var res = cmd.ExecuteScalar();
                _sqlConnection.Close();
                return (int?)res ?? -1;
            }
            else {
                var cmd = new SqlCommand("SelectUserByLoginDataLogin", _sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", Encipher.Encrypt(password, 24));
                _sqlConnection.Open();
                var res = cmd.ExecuteScalar();
                _sqlConnection.Close();
                return (int?)res ?? -1;
            }
        }
        public bool Register(User user) {
            _sqlConnection.Open();
            var sqlString = "SELECT COUNT(*) FROM LoginData WHERE Login = @Login or Email = @Email";
            var cmd       = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Login", user.Login);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            var res       = Convert.ToInt32(cmd.ExecuteScalar());
            if(res != 0) { return false; }

            sqlString = "INSERT LoginData([Email],[Login],[Password]) OUTPUT INSERTED.Id " +
                        "VALUES (@Login,@Email,@Password)";
            cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Login", user.Login);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Password", Encipher.Encrypt(user.Password, 24));
            var loginDataId = Convert.ToInt32(cmd.ExecuteScalar());

            sqlString = "INSERT Persons([Name],[Surname]) OUTPUT INSERTED.Id VALUES (@Name,@Surname)";
            cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Surname", user.Surname);
            var personsId = Convert.ToInt32(cmd.ExecuteScalar());

            sqlString = "INSERT Subscriptions([SubscriptionsNameId], [ValidUntil]) OUTPUT INSERTED.Id " +
                        "VALUES (@SubscriptionsNameId, @ValidUntil)";
            cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@SubscriptionsNameId", (int)user.SubscriptionName);
            cmd.Parameters.AddWithValue("@ValidUntil", user.SubscriptionValidUntil);
            var subscriptionsId = Convert.ToInt32(cmd.ExecuteScalar());

            sqlString = "INSERT Users([PersonId],[LoginDataId],[SubscriptionsId],[Address],[Phone],[Money]) " +
                        "VALUES (@PersonId,@LoginDataId,@SubscriptionsId,@Address,@Phone,0)";
            cmd = new SqlCommand(sqlString, _sqlConnection);
            cmd.Parameters.AddWithValue("@PersonId", personsId);
            cmd.Parameters.AddWithValue("@LoginDataId", loginDataId);
            cmd.Parameters.AddWithValue("@SubscriptionsId", subscriptionsId);
            cmd.Parameters.AddWithValue("@Address", user.Address);
            cmd.Parameters.AddWithValue("@Phone", user.Phone);
            cmd.ExecuteNonQuery();
            _sqlConnection.Close();
            return true;
        }
    }
}