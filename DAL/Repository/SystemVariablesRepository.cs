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
    public class SystemVariablesRepository
    {
        private string connectionString;

        public SystemVariablesRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public SystemVariable GetSystemVariable(long apartmentId)
        {
            SystemVariable systemVariable;
            string sqlCommand = @"select * from systemVAR where ApartmentId = @param1";
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
                            systemVariable = new SystemVariable()
                            {
                                ApartmentId = long.Parse(reader["ApartmentId"].ToString()),
                                OwnerName = reader["OwnerName"].ToString(),
                                CardId = reader["CardId"].ToString(),
                                BuildingName = reader["BuildingName"].ToString(),
                                OwnerAddress = reader["OwnerAddress"].ToString(),
                                WUnit = Single.Parse(reader["WUnit"].ToString()),
                                EUnit = Single.Parse(reader["EUnit"].ToString()),
                                IncWUnit = bool.Parse(reader["Inc_WUnit"].ToString()),
                                IncEUnit = bool.Parse(reader["Inc_EUnit"].ToString()),
                                IncTUnit = bool.Parse(reader["Inc_TUnit"].ToString()),
                                IncImprove = bool.Parse(reader["Inc_Improve"].ToString()),
                                StartInv = reader["StartInv"].ToString(),
                                EndPay = reader["EndPay"].ToString(),
                                InterestRate = Decimal.Parse(reader["InterestRate"].ToString()),
                                IncInterest = bool.Parse(reader["Inc_Interest"].ToString()),
                                VatAmount = Single.Parse(reader["VatAmount"].ToString()),
                                TaxId = reader["TaxId"].ToString(),
                                Paid = bool.Parse(reader["Paid"].ToString()),
                                PaperSize = reader["PaperSize"].ToString(),
                                HeadInvoice = reader["HeadInvoice"].ToString(),
                                HeadReciept = reader["HeadReciept"].ToString(),
                                IncFrame = bool.Parse(reader["Inc_Frame"].ToString())
                            };

                            return systemVariable;
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

        public void UpdateSystemVariables(SystemVariable vars)
        {
            string sqlCommand = "update systemVAR set [BuildingName] = @BuildingName, [OwnerAddress] = @OwnerAddress, " +
                                "[TaxId] = @TaxId, [wUnit] = @WUnit, [EUnit] = @EUnit " +
                                "where [ApartmentId] = @ApartmentId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@BuildingName", vars.BuildingName);
                        command.Parameters.AddWithValue("@OwnerAddress", vars.OwnerAddress);
                        command.Parameters.AddWithValue("@TaxId", vars.TaxId);
                        command.Parameters.AddWithValue("@wUnit", vars.WUnit);
                        command.Parameters.AddWithValue("@EUnit", vars.EUnit);
                        command.Parameters.AddWithValue("@ApartmentId", vars.ApartmentId);

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
