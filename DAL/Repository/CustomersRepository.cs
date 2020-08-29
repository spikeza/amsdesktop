using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AMSDesktop.DAL.Model;
using System.Data.OleDb;
using System.Data;

namespace AMSDesktop.DAL.Repository
{
    public class CustomersRepository
    {
        private string connectionString;

        public CustomersRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            string sqlCommand = @"select * from customers order by ContactName";
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
                            Customer c = new Customer()
                            {
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                CustomerNo = reader["CustomerNo"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                CardId = reader["CardId"].ToString(),
                                ContactName = reader["ContactName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Tel = reader["Tel"].ToString(),
                                ContactDate = reader["ContactDate"].ToString() != "" ? DateTime.Parse(reader["ContactDate"].ToString()) : (DateTime?)null
                            };
                            customers.Add(c);
                        }
                    }
                    return customers;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Customer GetCustomer(long customerId)
        {
            Customer customer;
            string sqlCommand = @"select * from customers where customerid = @param1";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@param1", customerId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            customer = new Customer()
                            {
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                CustomerNo = reader["CustomerNo"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                CardId = reader["CardId"].ToString(),
                                ContactName = reader["ContactName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Tel = reader["Tel"].ToString(),
                                ContactDate = reader["ContactDate"].ToString() != "" ? DateTime.Parse(reader["ContactDate"].ToString()) : (DateTime?)null
                            };

                            return customer;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Customer GetLatestCustomer()
        {
            Customer customer;
            string sqlCommand = @"select top 1 * from customers order by customerID desc";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            customer = new Customer()
                            {
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                CustomerNo = reader["CustomerNo"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                CardId = reader["CardId"].ToString(),
                                ContactName = reader["ContactName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Tel = reader["Tel"].ToString(),
                                ContactDate = reader["ContactDate"].ToString() != "" ? DateTime.Parse(reader["ContactDate"].ToString()) : (DateTime?)null
                            };

                            return customer;
                        }

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void AddCustomer(Customer customer)
        {
            string sqlCommand = "insert into customers ([CustomerNo], [CompanyName], [CardId], [ContactName], [Address], [Tel]) " +
                                "values(@CustomerNo, @CompanyName, @CardId, @ContactName, @Address, @Tel)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@CustomerNo", customer.CustomerNo);
                        command.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                        command.Parameters.AddWithValue("@CardId", customer.CardId);
                        command.Parameters.AddWithValue("@ContactName", customer.ContactName);
                        command.Parameters.AddWithValue("@Address", customer.Address);
                        command.Parameters.AddWithValue("@Tel", customer.Tel);

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

        public void UpdateCustomer(Customer customer)
        {
            string sqlCommand = "update customers set [CustomerNo] = @CustomerNo, [CompanyName] = @CompanyName, [CardId] = @CardId, " +
                                "[ContactName] = @ContactName, [Address] = @Address, [Tel] = @Tel " +
                                "where [CustomerId] = @CustomerId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@CustomerNo", customer.CustomerNo);
                        command.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                        command.Parameters.AddWithValue("@CardId", customer.CardId);
                        command.Parameters.AddWithValue("@ContactName", customer.ContactName);
                        command.Parameters.AddWithValue("@Address", customer.Address);
                        command.Parameters.AddWithValue("@Tel", customer.Tel);
                        command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);

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

        public void DeleteCustomer(Customer customer)
        {
            string sqlCommand = "delete from customers where [CustomerId] = @CustomerId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);

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

        public List<Customer> SearchCustomers(string searchValue)
        {
            List<Customer> customers = new List<Customer>();
            string sqlCommand = @"select * from customers where ContactName like @ContactName order by ContactName";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@ContactName", "%" + searchValue + "%");
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            Customer c = new Customer()
                            {
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                CustomerNo = reader["CustomerNo"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                CardId = reader["CardId"].ToString(),
                                ContactName = reader["ContactName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Tel = reader["Tel"].ToString(),
                                ContactDate = reader["ContactDate"].ToString() != "" ? DateTime.Parse(reader["ContactDate"].ToString()) : (DateTime?)null
                            };
                            customers.Add(c);
                        }

                        return customers;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

    }
}
