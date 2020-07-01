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
    public class UserRepository
    {
        private string connectionString;
        public UserRepository()
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
                catch (Exception ex)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public User GetUser(string username)
        {
            User user = new User();
            string sqlCommand = @"select Username, Firstname, Lastname from users where username = @param1";
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
                            };
                        }

                        return user;
                    }
                }
                catch (Exception ex)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public User IsAuthenticatedUser(string username, string password)
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
                            CryptographyHelper ch = new CryptographyHelper();
                            if (reader["Password"].ToString() == ch.GetHashedString(password + reader["Salt"]))
                            {
                                user = new User()
                                {
                                    Username = reader["Username"].ToString(),
                                    Firstname = reader["Firstname"].ToString(),
                                    Lastname = reader["Lastname"].ToString(),
                                };
                                return user;
                            }
                        }
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
