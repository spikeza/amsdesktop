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
                    command.Parameters.Add("@fromDate", OleDbType.Date).Value = fromDate;
                    command.Parameters.Add("@toDate", OleDbType.Date).Value = toDate;
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
                    command.Parameters.Add("@fromDate", OleDbType.Date).Value = fromDate;
                    command.Parameters.Add("@toDate", OleDbType.Date).Value = toDate;
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

        public Invoice GetInvoice(long invoiceId)
        {
            Invoice invoice;
            string sqlCommand = @"select * from invoices where invoiceId = @param1";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@param1", invoiceId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            invoice = new Invoice()
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

                            return invoice;
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

        public Invoice GetInvoiceForReceipt(long roomId, long month, int year)
        {
            Invoice invoice;
            string sqlCommand = "select top 1 * from invoices where roomId = @RoomId and monthNo = @MonthNo " +
                                "and Year(InvDate) = @Year " +
                                "order by InvDate desc";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@MonthNo", month);
                    command.Parameters.AddWithValue("@Year", year);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            invoice = new Invoice()
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

                            return invoice;
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

        public Invoice GetLatestInvoice(long roomId)
        {
            Invoice invoice;
            string sqlCommand = @"select top 1 * from invoices where roomId = @param1 order by InvDate desc";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@param1", roomId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            invoice = new Invoice()
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

                            return invoice;
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
                    sqlCommand += "where InvoiceNo like @SearchValue and InvDate >= @fromDate and InvDate <= @toDate and Invoices.apartmentId = @apartmentId ";
                }
                sqlCommand += "order by left(InvoiceNo,4) desc, right(InvoiceNo,4)";
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                    command.Parameters.Add("@fromDate", OleDbType.Date).Value = fromDate;
                    command.Parameters.Add("@toDate", OleDbType.Date).Value = toDate;
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

                            if (lastInvoiceNo.Substring(0, 4) == DateTime.Now.ToString("yyMM", thCulture))
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

        public void AddInvoice(Invoice invoice)
        {
            string sqlCommand = "insert into invoices ([ApartmentId], [InvoiceNo], [RoomId], [MonthNo], [InvDate], [WMeterStart], [EMeterStart], " +
                                "[WUsedUnit], [EUsedUnit], [TelCost], [WUnit], [EUnit], [ImproveText], [ImproveCost], [Comment], [Paid], " +
                                "[TotalText], [GrandTotal], [GrandTotalText]) " +
                                "values(@ApartmentId, @InvoiceNo, @RoomId, @MonthNo, @InvDate, @WMeterStart, @EMeterStart," +
                                "@WUsedUnit, @EUsedUnit, @TelCost, @WUnit, @EUnit, @ImproveText, @ImproveCost, @Comment, @Paid," +
                                "@TotalText, @GrandTotal, @GrandTotalText)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ApartmentId", invoice.ApartmentId);
                        command.Parameters.AddWithValue("@InvoiceNo", invoice.InvoiceNo);
                        command.Parameters.AddWithValue("@RoomId", invoice.Room.RoomId);
                        command.Parameters.AddWithValue("@MonthNo", invoice.MonthNo);
                        command.Parameters.Add("@InvDate", OleDbType.Date).Value = invoice.InvDate;
                        command.Parameters.AddWithValue("@WMeterStart", invoice.WMeterStart);
                        command.Parameters.AddWithValue("@EMeterStart", invoice.EMeterStart);
                        command.Parameters.AddWithValue("@WUsedUnit", invoice.WUsedUnit);
                        command.Parameters.AddWithValue("@EUsedUnit", invoice.EUsedUnit);
                        command.Parameters.AddWithValue("@TelCost", invoice.TelCost);
                        command.Parameters.AddWithValue("@WUnit", invoice.WUnit);
                        command.Parameters.AddWithValue("@EUnit", invoice.EUnit);
                        command.Parameters.AddWithValue("@ImproveText", invoice.ImproveText);
                        command.Parameters.AddWithValue("@ImproveCost", invoice.ImproveCost);
                        command.Parameters.AddWithValue("@Comment", invoice.Comment);
                        command.Parameters.AddWithValue("@Paid", invoice.Paid);
                        command.Parameters.AddWithValue("@TotalText", invoice.TotalText);
                        command.Parameters.AddWithValue("@GrandTotal", invoice.GrandTotal);
                        command.Parameters.AddWithValue("@GrandTotalText", invoice.GrandTotalText);

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

        public void UpdateInvoice(Invoice invoice)
        {
            string sqlCommand = "update invoices set [wUsedUnit] = @WUsedUnit, [eUsedUnit] = @EUsedUnit, [telCost] = @TelCost, " +
                                "[improveText] = @ImproveText, [improveCost] = @ImproveCost, [comment] = @Comment, [TotalText] = @TotalText, " +
                                "[GrandTotal] = @GrandTotal, [GrandTotalText] = @GrandTotalText " +
                                "where [InvoiceId] = @InvoiceId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@WUsedUnit", invoice.WUsedUnit);
                        command.Parameters.AddWithValue("@EUsedUnit", invoice.EUsedUnit);
                        command.Parameters.AddWithValue("@TelCost", invoice.TelCost);
                        command.Parameters.AddWithValue("@ImproveText", invoice.ImproveText);
                        command.Parameters.AddWithValue("@ImproveCost", invoice.ImproveCost);
                        command.Parameters.AddWithValue("@Comment", invoice.Comment);
                        command.Parameters.AddWithValue("@TotalText", invoice.TotalText);
                        command.Parameters.AddWithValue("@GrandTotal", invoice.GrandTotal);
                        command.Parameters.AddWithValue("@GrandTotalText", invoice.GrandTotalText);
                        command.Parameters.AddWithValue("@InvoiceId", invoice.InvoiceId);

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

        public void DeleteInvoice(Invoice invoice)
        {
            string sqlCommand = "delete from invoices where [InvoiceId] = @InvoiceId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@InvoiceId", invoice.InvoiceId);

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

        public bool IsThisMonthInvoiceExists(long roomId, long month, int year)
        {
            string sqlCommand = "select count(InvoiceId) as Num from invoices where roomId = @RoomId and monthNo = @MonthNo " +
                                "and Year(InvDate) = @Year";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@RoomId", roomId);
                    command.Parameters.AddWithValue("@MonthNo", month);
                    command.Parameters.AddWithValue("@Year", year);
                    con.Open();

                    if ((int)command.ExecuteScalar() > 0)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void SetInvoicePaidStatus(Invoice invoice)
        {
            string sqlCommand = "update invoices set [Paid] = @Paid " +
                                "where [InvoiceId] = @InvoiceId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Paid", invoice.Paid);
                        command.Parameters.AddWithValue("@InvoiceId", invoice.InvoiceId);

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
