using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
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
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
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
                    command.Parameters.AddWithValue("@fromDate", fromDate);
                    command.Parameters.AddWithValue("@toDate", toDate);
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
    }
}
