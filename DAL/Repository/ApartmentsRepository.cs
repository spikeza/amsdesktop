using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Model;

namespace AMSDesktop.DAL.Repository
{
    public class ApartmentsRepository
    {
        private string connectionString;

        public ApartmentsRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public List<Apartment> GetApartments()
        {
            List<Apartment> apartments = new List<Apartment>();
            string sqlCommand = @"select * from apartments order by ApartmentId";
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
                            Apartment a = new Apartment()
                            {
                                ApartmentId = long.Parse(reader["ApartmentId"].ToString()),
                                ApartmentName = reader["ApartmentName"].ToString(),
                                Address = reader["Address"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                TaxId = reader["TaxId"].ToString(),
                                Tel = reader["Tel"].ToString()
                            };
                            apartments.Add(a);
                        }
                    }
                    return apartments;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Apartment GetApartment(long apartmentId)
        {
            Apartment apartment;
            string sqlCommand = @"select * from apartments where apartmentid = @param1";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@param1", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            apartment = new Apartment()
                            {
                                ApartmentId = long.Parse(reader["ApartmentId"].ToString()),
                                ApartmentName = reader["ApartmentName"].ToString(),
                                Address = reader["Address"].ToString(),
                                CompanyName = reader["CompanyName"].ToString(),
                                TaxId = reader["TaxId"].ToString(),
                                Tel = reader["Tel"].ToString()
                            };

                            return apartment;
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

        public void AddApartment(Apartment apartment)
        {
            string sqlCommand = "insert into apartments ([ApartmentName], [Address], [CompanyName], [TaxId], [Tel]) " +
                             "values(@ApartmentName, @Address, @CompanyName, @TaxId, @Tel)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ApartmentName", apartment.ApartmentName);
                        command.Parameters.AddWithValue("@Address", apartment.Address);
                        command.Parameters.AddWithValue("@CompanyName", apartment.CompanyName);
                        command.Parameters.AddWithValue("@TaxId", apartment.TaxId);
                        command.Parameters.AddWithValue("@Tel", apartment.Tel);

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

        public void UpdateApartment(Apartment apartment)
        {
            string sqlCommand = "update apartments set [ApartmentName] = @ApartmentName, [Address] = @Address, [CompanyName] = @CompanyName, " +
                                "[TaxId] = @TaxId, [Tel] = @Tel " +
                                "where [ApartmentId] = @ApartmentId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ApartmentName", apartment.ApartmentName);
                        command.Parameters.AddWithValue("@Address", apartment.Address);
                        command.Parameters.AddWithValue("@CompanyName", apartment.CompanyName);
                        command.Parameters.AddWithValue("@TaxId", apartment.TaxId);
                        command.Parameters.AddWithValue("@Tel", apartment.Tel);
                        command.Parameters.AddWithValue("@ApartmentId", apartment.ApartmentId);

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

        public void DeleteApartment(Apartment apartment)
        {
            string sqlCommand = "delete from apartments where [ApartmentId] = @ApartmentId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ApartmentId", apartment.ApartmentId);

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
