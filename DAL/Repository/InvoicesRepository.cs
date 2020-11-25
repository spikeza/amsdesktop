using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Model;

namespace AMSDesktop.DAL.Repository
{
    public class InvoicesRepository
    {
        private string connectionString;
        public InvoicesRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public List<Invoice> GetInvoices(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            List<Invoice> invoices = new List<Invoice>();
            string sqlCommand = "select * from Invoices " +
                "where InvDate >= @fromDate and InvDate <= @toDate and ApartmentId = @apartmentId " +
                "order by left(InvoiceNo,4) desc, right(InvoiceNo,4)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
                    command.Parameters.AddWithValue("@apartmentId", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            Invoice i = new Invoice()
                            {
                                InvoiceId = long.Parse(reader["InvoiceId"].ToString()),
                                ApartmentId = long.Parse(reader["ApartmentId"].ToString()),
                                InvoiceNo = reader["InvoiceNo"].ToString(),
                                Room = new RoomsRepository().GetRoom(long.Parse(reader["RoomId"].ToString())),
                                MonthNo = long.Parse(reader["MonthNo"].ToString()),
                                InvDate = DateTime.Parse(reader["InvDate"].ToString()),
                                WMeterStart = long.Parse(reader["WMeterStart"].ToString()),
                                EMeterStart = long.Parse(reader["EMeterStart"].ToString()),
                                WUsedUnit = long.Parse(reader["WUsedUnit"].ToString()),
                                EUsedUnit = long.Parse(reader["EUsedUnit"].ToString()),
                                TelCost = Decimal.Parse(reader["TelCost"].ToString()),
                                WUnit = Decimal.Parse(reader["WUnit"].ToString()),
                                EUnit = Decimal.Parse(reader["EUnit"].ToString()),
                                ImproveText = reader["ImproveText"].ToString(),
                                ImproveCost = Decimal.Parse(reader["ImproveCost"].ToString()),
                                Comment = reader["Comment"].ToString(),
                                Paid = bool.Parse(reader["Paid"].ToString()),
                                TotalText = reader["TotalText"].ToString(),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString()),
                                GrandTotalText = reader["GrandTotalText"].ToString()
                            };
                            invoices.Add(i);
                        }
                    }
                    return invoices;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<InvoiceDataGridView> GetInvoicesForDataGrid(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            List<InvoiceDataGridView> invoices = new List<InvoiceDataGridView>();
            string sqlCommand = "select [InvoiceId], [InvoiceNo], Invoices.[RoomId], [RoomNo], [MonthNo], [InvDate], [GrandTotal] " +
                                "from Invoices inner join Rooms on Invoices.[RoomId] = Rooms.[RoomId] " +
                                "where InvDate >= @fromDate and InvDate <= @toDate and Invoices.ApartmentId = @apartmentId " +
                                "order by left(InvoiceNo,4) desc, right(InvoiceNo,4)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
                    command.Parameters.AddWithValue("@apartmentId", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            InvoiceDataGridView i = new InvoiceDataGridView()
                            {
                                InvoiceId = long.Parse(reader["InvoiceId"].ToString()),
                                InvoiceNo = reader["InvoiceNo"].ToString(),
                                RoomNo = reader["RoomNo"].ToString(),
                                MonthNo = long.Parse(reader["MonthNo"].ToString()),
                                InvDate = DateTime.Parse(reader["InvDate"].ToString()),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString())
                            };
                            invoices.Add(i);
                        }
                    }
                    return invoices;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<InvoiceDataGridView> SearchInvoicesForDataGrid(string searchValue, string searchMode, DateTime fromDate, DateTime toDate, long apartmentId)
        {
            List<InvoiceDataGridView> invoices = new List<InvoiceDataGridView>();
            string sqlCommand = @"select [InvoiceId], [InvoiceNo], Invoices.[RoomId], [RoomNo], [MonthNo], [InvDate], [GrandTotal] " +
                                "from Invoices inner join Rooms on Invoices.[RoomId] = Rooms.[RoomId] ";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                if (searchMode == "RoomNo")
                {
                    sqlCommand += "where RoomNo like @SearchValue and InvDate >= @fromDate and InvDate <= @toDate and Invoices.apartmentId = @apartmentId ";
                }
                else
                {
                    sqlCommand += "where InvoiceNo like @SearchValue and InvDate >= @fromDate and InvDate <= @toDate Invoices.apartmentId = @apartmentId ";
                }
                sqlCommand += "order by left(InvoiceNo,4) desc, right(InvoiceNo,4)";
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
                    command.Parameters.AddWithValue("@apartmentId", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            InvoiceDataGridView i = new InvoiceDataGridView()
                            {
                                InvoiceId = long.Parse(reader["InvoiceId"].ToString()),
                                InvoiceNo = reader["InvoiceNo"].ToString(),
                                RoomNo = reader["RoomNo"].ToString(),
                                MonthNo = long.Parse(reader["MonthNo"].ToString()),
                                InvDate = DateTime.Parse(reader["InvDate"].ToString()),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString())
                            };
                            invoices.Add(i);
                        }
                    }
                    return invoices;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string GetNewInvoiceNumber(long apartmentId)
        {
            CultureInfo thCulture = new CultureInfo("th-TH");
            string prefix = DateTime.Now.ToString("yyMM-", thCulture);
            string lastInvoiceNo = "", nextInvoiceNo = "";
            string sqlCommand = @"select top 1 InvoiceNo from invoices where apartmentId = @apartmentId order by InvoiceId desc";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@apartmentId", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            lastInvoiceNo = reader["InvoiceNo"].ToString();

                            if (lastInvoiceNo.Substring(0, 4) == DateTime.Now.ToString("yyMM-", thCulture))
                            {
                                int next = int.Parse(lastInvoiceNo.Substring(5).TrimStart('0')) + 1;
                                nextInvoiceNo = DateTime.Now.ToString("yyMM-", thCulture) + next.ToString().PadLeft(4, '0');
                            }
                            else
                            {
                                nextInvoiceNo = DateTime.Now.ToString("yyMM-", thCulture) + "0001";
                            }
                        }

                        return nextInvoiceNo;
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
