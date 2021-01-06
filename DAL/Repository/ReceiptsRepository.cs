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
    public class ReceiptsRepository
    {
        private string connectionString;
        public ReceiptsRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public List<Receipt> GetReceipts(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            List<Receipt> receipts = new List<Receipt>();
            string sqlCommand = "select * from receipts " +
                "where RcpDate >= @fromDate and RcpDate <= @toDate and ApartmentId = @apartmentId " +
                "order by left(ReceiptNo,4) desc, right(ReceiptNo,4)";
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
                            Receipt r = new Receipt()
                            {
                                ReceiptId = long.Parse(reader["ReceiptId"].ToString()),
                                Invoice = new InvoicesRepository().GetInvoice(long.Parse(reader["InvoiceId"].ToString())),
                                ApartmentId = long.Parse(reader["ApartmentId"].ToString()),
                                ReceiptNo = reader["ReceiptNo"].ToString(),
                                InterestUnit = Decimal.Parse(reader["InterestUnit"].ToString()),
                                AmountDay = long.Parse(reader["AmountDay"].ToString()),
                                RcpDate = DateTime.Parse(reader["RcpDate"].ToString()),
                                Comment = reader["Comment"].ToString(),
                                TotalText = reader["TotalText"].ToString(),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString()),
                                GrandTotalText = reader["GrandTotalText"].ToString()
                            };
                            receipts.Add(r);
                        }
                    }
                    return receipts;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<ReceiptDataGridView> GetReceiptsForDataGrid(DateTime fromDate, DateTime toDate, long apartmentId)
        {
            List<ReceiptDataGridView> receipts = new List<ReceiptDataGridView>();
            string sqlCommand = "select [ReceiptId], [ReceiptNo], invoices.[InvoiceNo], rooms.[RoomNo], [MonthNo], [RcpDate], receipts.[GrandTotal] " +
                                "from ((receipts inner join invoices on receipts.[InvoiceId] = invoices.[InvoiceId]) " +
                                "inner join rooms on invoices.[RoomId] = rooms.[RoomId]) " +
                                "where RcpDate >= @fromDate and RcpDate <= @toDate and rooms.ApartmentId = @apartmentId " +
                                "order by left(ReceiptNo,4) desc, right(ReceiptNo,4)";
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
                            ReceiptDataGridView r = new ReceiptDataGridView()
                            {
                                ReceiptId = long.Parse(reader["ReceiptId"].ToString()),
                                ReceiptNo = reader["ReceiptNo"].ToString(),
                                InvoiceNo = reader["InvoiceNo"].ToString(),
                                RoomNo = reader["RoomNo"].ToString(),
                                MonthNo = long.Parse(reader["MonthNo"].ToString()),
                                RcpDate = DateTime.Parse(reader["RcpDate"].ToString()),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString())
                            };
                            receipts.Add(r);
                        }
                    }
                    return receipts;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Receipt GetReceipt(long receiptId)
        {
            Receipt receipt;
            string sqlCommand = @"select * from receipts where receiptId = @param1";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@param1", receiptId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            receipt = new Receipt()
                            {
                                ReceiptId = long.Parse(reader["ReceiptId"].ToString()),
                                Invoice = new InvoicesRepository().GetInvoice(long.Parse(reader["InvoiceId"].ToString())),
                                ApartmentId = long.Parse(reader["ApartmentId"].ToString()),
                                ReceiptNo = reader["ReceiptNo"].ToString(),
                                InterestUnit = Decimal.Parse(reader["InterestUnit"].ToString()),
                                AmountDay = long.Parse(reader["AmountDay"].ToString()),
                                RcpDate = DateTime.Parse(reader["RcpDate"].ToString()),
                                Comment = reader["Comment"].ToString(),
                                TotalText = reader["TotalText"].ToString(),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString()),
                                GrandTotalText = reader["GrandTotalText"].ToString()
                            };

                            return receipt;
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

        public List<ReceiptDataGridView> SearchReceiptsForDataGrid(string searchValue, string searchMode, DateTime fromDate, DateTime toDate, long apartmentId)
        {
            List<ReceiptDataGridView> receipts = new List<ReceiptDataGridView>();
            string sqlCommand = @"select [ReceiptId], [ReceiptNo], invoices.[InvoiceNo], rooms.[RoomNo], [MonthNo], [RcpDate], receipts.[GrandTotal] " +
                                "from ((receipts inner join invoices on receipts.[InvoiceId] = invoices.[InvoiceId]) " +
                                "inner join rooms on invoices.[RoomId] = rooms.[RoomId]) ";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                if (searchMode == "RoomNo")
                {
                    sqlCommand += "where RoomNo like @SearchValue and RcpDate >= @fromDate and RcpDate <= @toDate and receipts.apartmentId = @apartmentId ";
                }
                else
                {
                    sqlCommand += "where ReceiptNo like @SearchValue and RcpDate >= @fromDate and RcpDate <= @toDate and receipts.apartmentId = @apartmentId ";
                }
                sqlCommand += "order by left(ReceiptNo,4) desc, right(ReceiptNo,4)";
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
                            ReceiptDataGridView r = new ReceiptDataGridView()
                            {
                                ReceiptId = long.Parse(reader["ReceiptId"].ToString()),
                                ReceiptNo = reader["ReceiptNo"].ToString(),
                                InvoiceNo = reader["InvoiceNo"].ToString(),
                                RoomNo = reader["RoomNo"].ToString(),
                                MonthNo = long.Parse(reader["MonthNo"].ToString()),
                                RcpDate = DateTime.Parse(reader["RcpDate"].ToString()),
                                GrandTotal = Single.Parse(reader["GrandTotal"].ToString())
                            };
                            receipts.Add(r);
                        }
                    }
                    return receipts;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public string GetNewReceiptNumber(long apartmentId)
        {
            CultureInfo thCulture = new CultureInfo("th-TH");
            string prefix = DateTime.Now.ToString("yyMM-", thCulture);
            string lastReceiptNo = "", nextReceiptNo = "";
            string sqlCommand = @"select top 1 ReceiptNo from receipts where apartmentId = @apartmentId order by ReceiptId desc";
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
                            lastReceiptNo = reader["ReceiptNo"].ToString();

                            if (lastReceiptNo.Substring(0, 4) == DateTime.Now.ToString("yyMM", thCulture))
                            {
                                int next = int.Parse(lastReceiptNo.Substring(5).TrimStart('0')) + 1;
                                nextReceiptNo = DateTime.Now.ToString("yyMM-", thCulture) + next.ToString().PadLeft(4, '0');
                            }
                            else
                            {
                                nextReceiptNo = DateTime.Now.ToString("yyMM-", thCulture) + "0001";
                            }
                        }

                        return nextReceiptNo;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void AddReceipt(Receipt receipt)
        {
            string sqlCommand = "insert into receipts ([InvoiceID], [ApartmentID], [ReceiptNo], [InterestUnit], [AmountDay], [RcpDate], " +
                                "[Comment], [TotalText], [GrandTotal], [GrandTotalText]) " +
                                "values(@InvoiceID, @ApartmentID, @ReceiptNo, @InterestUnit, @AmountDay, @RcpDate," +
                                "@Comment, @TotalText, @GrandTotal, @GrandTotalText)";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@InvoiceID", receipt.Invoice.InvoiceId);
                        command.Parameters.AddWithValue("@ApartmentID", receipt.ApartmentId);
                        command.Parameters.AddWithValue("@ReceiptNo", receipt.ReceiptNo);
                        command.Parameters.AddWithValue("@InterestUnit", receipt.InterestUnit);
                        command.Parameters.AddWithValue("@AmountDay", receipt.AmountDay);
                        command.Parameters.Add("@RcpDate", OleDbType.Date).Value = receipt.RcpDate;
                        command.Parameters.AddWithValue("@Comment", receipt.Comment);
                        command.Parameters.AddWithValue("@TotalText", receipt.TotalText);
                        command.Parameters.AddWithValue("@GrandTotal", receipt.GrandTotal);
                        command.Parameters.AddWithValue("@GrandTotalText", receipt.GrandTotalText);

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

        public void UpdateReceipt(Receipt receipt)
        {
            string sqlCommand = "update receipts set [Comment] = @Comment " +
                                "where [ReceiptId] = @ReceiptId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Comment", receipt.Comment);
                        command.Parameters.AddWithValue("@ReceiptId", receipt.ReceiptId);

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

        public void DeleteReceipt(Receipt receipt)
        {
            string sqlCommand = "delete from receipts where [ReceiptId] = @ReceiptId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@ReceiptId", receipt.ReceiptId);

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

        public bool IsThisMonthReceiptExists(long roomId, long month, int year)
        {
            string sqlCommand = "select count(ReceiptId) as Num from receipts " +
                                "inner join invoices on receipts.InvoiceId = invoices.InvoiceId " +
                                "where invoices.roomId = @RoomId and invoices.monthNo = @MonthNo " +
                                "and Year(RcpDate) = @Year";

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

        public List<IncomeSummaryRecord> GetIncomeSummaryRecords(int month, int year, long apartmentId)
        {
            List<IncomeSummaryRecord> records = new List<IncomeSummaryRecord>();
            string sqlCommand = "select [ReceiptNo], invoices.[InvoiceNo], [RcpDate], invoices.[ImproveCost], receipts.[GrandTotal] " +
                                "from receipts inner join invoices on receipts.[InvoiceId] = invoices.[InvoiceId] " +
                                "where Year(RcpDate) = @Year and Month(RcpDate) = @Month and receipts.ApartmentId = @ApartmentId " +
                                "order by left(ReceiptNo,4) desc, right(ReceiptNo,4)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@ApartmentId", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            IncomeSummaryRecord r = new IncomeSummaryRecord()
                            {
                                ReceiptNo = reader["ReceiptNo"].ToString(),
                                InvoiceNo = reader["InvoiceNo"].ToString(),
                                RcpDate = DateTime.Parse(reader["RcpDate"].ToString()).ToString("d MMMM yyyy", new CultureInfo("th-TH")),
                                ImproveCost = Decimal.Parse(reader["ImproveCost"].ToString()),
                                GrandTotal = Decimal.Parse(reader["GrandTotal"].ToString())
                            };
                            records.Add(r);
                        }
                    }
                    return records;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
