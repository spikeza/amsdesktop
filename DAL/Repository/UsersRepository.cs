using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.OleDb;
using AMSDesktop.DAL.Model;
using AMSDesktop.Helpers;
using System.Data;

namespace AMSDesktop.DAL.Repository
{
    public class UsersRepository
    {
        private string connectionString;
        public UsersRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string sqlCommand = @"select * from users";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            User u = new User()
                            {
                                Username = reader["Username"].ToString(),
                                Firstname = reader["Firstname"].ToString(),
                                Lastname = reader["Lastname"].ToString(),
                                Password = reader["Password"].ToString(),
                                Salt = reader["Salt"].ToString()
                            };
                            users.Add(u);
                        }
                    }
                    return users;
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public User GetUser(string username)
        {
            User user;
            string sqlCommand = @"select * from users where username = @param1";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@param1", username);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            user = new User()
                            {
                                Username = reader["Username"].ToString(),
                                Firstname = reader["Firstname"].ToString(),
                                Lastname = reader["Lastname"].ToString(),
                                Password = reader["Password"].ToString(),
                                Salt = reader["Salt"].ToString()
                            };

                            return user;
                        }

                        return null;
                    }
                }
                catch (Exception)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void AddUser(User user)
        {
            string sqlCommand = "insert into users ([Username], [Firstname], [Lastname], [Password], [Salt]) " +
                                "values(@Username, @Firstname, @Lastname, @Password, @Salt)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Firstname", user.Firstname);
                        command.Parameters.AddWithValue("@Lastname", user.Lastname);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Salt", user.Salt);

                        con.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void UpdateUser(User user)
        {
            string sqlCommand = "update users set [Firstname] = @Firstname, [Lastname] = @Lastname " +
                                "where [Username] = @Username";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Firstname", user.Firstname);
                        command.Parameters.AddWithValue("@Lastname", user.Lastname);
                        command.Parameters.AddWithValue("@Username", user.Username);

                        con.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void DeleteUser(User user)
        {
            string sqlCommand = "delete from users where [Username] = @Username";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Username", user.Username);

                        con.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void ChangePassword(User user)
        {
            string sqlCommand = "update users set [Password] = @Password, [Salt] = @Salt " +
                                "where [Username] = @Username";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Salt", user.Salt);
                        command.Parameters.AddWithValue("@Username", user.Username);

                        con.Open();

                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
