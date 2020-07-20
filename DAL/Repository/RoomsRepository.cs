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
    public class RoomsRepository
    {
        private string connectionString;

        public RoomsRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AMSDesktop.Properties.Settings.amsdbConnectionString"].ConnectionString;
        }

        public List<Room> GetRooms()
        {
            List<Room> rooms = new List<Room>();
            string sqlCommand = @"select * from rooms";
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
                            Room r = new Room()
                            {
                                RoomId = long.Parse(reader["RoomId"].ToString()),
                                RoomNo = reader["RoomNo"].ToString(),
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                ContactName = new CustomersRepository().GetCustomer(long.Parse(reader["CustomerId"].ToString())).ContactName,
                                WUnitStart = long.Parse(reader["WUnitStart"].ToString()),
                                EUnitStart = long.Parse(reader["EUnitStart"].ToString()),
                                MonthCost = Decimal.Parse(reader["MonthCost"].ToString()),
                                InsureCost = Decimal.Parse(reader["InsureCost"].ToString()),
                                StartDate = reader["StartDate"].ToString() != "" ? DateTime.Parse(reader["StartDate"].ToString()) : (DateTime?)null,
                                ApartmentId = long.Parse(reader["ApartMentId"].ToString()),
                                Floor = reader["Floor"].ToString(),
                                Picture = reader["Picture"].ToString(),
                                ContractMonth = long.Parse(reader["ContractMonth"].ToString()),
                                LandTaxedPerson = bool.Parse(reader["LandTaxedPerson"].ToString())
                            };
                            rooms.Add(r);
                        }
                    }
                    return rooms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<Room> GetRooms(long apartmentId)
        {
            List<Room> rooms = new List<Room>();
            string sqlCommand = @"select * from rooms where ApartmentId = @ApartmentId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@ApartmentId", apartmentId);
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            Room r = new Room()
                            {
                                RoomId = long.Parse(reader["RoomId"].ToString()),
                                RoomNo = reader["RoomNo"].ToString(),
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                ContactName = new CustomersRepository().GetCustomer(long.Parse(reader["CustomerId"].ToString())).ContactName,
                                WUnitStart = long.Parse(reader["WUnitStart"].ToString()),
                                EUnitStart = long.Parse(reader["EUnitStart"].ToString()),
                                MonthCost = Decimal.Parse(reader["MonthCost"].ToString()),
                                InsureCost = Decimal.Parse(reader["InsureCost"].ToString()),
                                StartDate = reader["StartDate"].ToString() != "" ? DateTime.Parse(reader["StartDate"].ToString()) : (DateTime?)null,
                                ApartmentId = long.Parse(reader["ApartMentId"].ToString()),
                                Floor = reader["Floor"].ToString(),
                                Picture = reader["Picture"].ToString(),
                                ContractMonth = long.Parse(reader["ContractMonth"].ToString()),
                                LandTaxedPerson = bool.Parse(reader["LandTaxedPerson"].ToString())
                            };
                            rooms.Add(r);
                        }
                    }
                    return rooms;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Room GetRoom(long roomId)
        {
            Room room;
            string sqlCommand = @"select * from rooms where roomid = @param1";
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
                            room = new Room()
                            {
                                RoomId = long.Parse(reader["RoomId"].ToString()),
                                RoomNo = reader["RoomNo"].ToString(),
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                ContactName = new CustomersRepository().GetCustomer(long.Parse(reader["CustomerId"].ToString())).ContactName,
                                WUnitStart = long.Parse(reader["WUnitStart"].ToString()),
                                EUnitStart = long.Parse(reader["EUnitStart"].ToString()),
                                MonthCost = Decimal.Parse(reader["MonthCost"].ToString()),
                                InsureCost = Decimal.Parse(reader["InsureCost"].ToString()),
                                StartDate = reader["StartDate"].ToString() != "" ? DateTime.Parse(reader["StartDate"].ToString()) : (DateTime?)null,
                                ApartmentId = long.Parse(reader["ApartMentId"].ToString()),
                                Floor = reader["Floor"].ToString(),
                                Picture = reader["Picture"].ToString(),
                                ContractMonth = long.Parse(reader["ContractMonth"].ToString()),
                                LandTaxedPerson = bool.Parse(reader["LandTaxedPerson"].ToString())
                            };

                            return room;
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

        public void AddRoom(Room room)
        {
            string sqlCommand = "insert into rooms ([RoomNo], [CustomerId], [WUnitStart], [EUnitStart], [MonthCost], [InsureCost], [StartDate], [ApartmentId], [Floor], [Picture], [ContractMonth], [LandTaxedPerson]) " +
                                "values(@RoomNo, @CustomerId, @WUnitStart, @EUnitStart, @MonthCost, @InsureCost, @StartDate, @ApartmentId, @Floor, @Picture, @ContractMonth, @LandTaxedPerson)";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@RoomNo", room.RoomNo);
                        command.Parameters.AddWithValue("@CustomerId", room.CustomerId);
                        command.Parameters.AddWithValue("@WUnitStart", room.WUnitStart);
                        command.Parameters.AddWithValue("@EUnitStart", room.EUnitStart);
                        command.Parameters.AddWithValue("@MonthCost", room.MonthCost);
                        command.Parameters.AddWithValue("@InsureCost", room.InsureCost);
                        command.Parameters.AddWithValue("@StartDate", room.StartDate);
                        command.Parameters.AddWithValue("@ApartmentId", room.ApartmentId);
                        command.Parameters.AddWithValue("@Floor", room.Floor);
                        command.Parameters.AddWithValue("@Picture", room.Picture);
                        command.Parameters.AddWithValue("@ContractMonth", room.ContractMonth);
                        command.Parameters.AddWithValue("@LandTaxedPerson", room.LandTaxedPerson);

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

        public void UpdateRoom(Room room)
        {
            string sqlCommand = "update rooms set [RoomNo] = @RoomNo, [CustomerId] = @CustomerId, [WUnitStart] = @WUnitStart, " +
                                "[EUnitStart] = @EUnitStart, [MonthCost] = @MonthCost, [InsureCost] = @InsureCost, [StartDate] = @StartDate, " +
                                "[ApartmentId] = @ApartmentId, [Floor] = @Floor, [Picture] = @Picture, [ContractMonth] = @ContractMonth, [LandTaxedPerson] = @LandTaxedPerson " +
                                "where [RoomId] = @RoomId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@RoomNo", room.RoomNo);
                        command.Parameters.AddWithValue("@CustomerId", room.CustomerId);
                        command.Parameters.AddWithValue("@WUnitStart", room.WUnitStart);
                        command.Parameters.AddWithValue("@EUnitStart", room.EUnitStart);
                        command.Parameters.AddWithValue("@MonthCost", room.MonthCost);
                        command.Parameters.AddWithValue("@InsureCost", room.InsureCost);
                        command.Parameters.AddWithValue("@StartDate", room.StartDate);
                        command.Parameters.AddWithValue("@ApartmentId", room.ApartmentId);
                        command.Parameters.AddWithValue("@Floor", room.Floor);
                        command.Parameters.AddWithValue("@Picture", room.Picture);
                        command.Parameters.AddWithValue("@ContractMonth", room.ContractMonth);
                        command.Parameters.AddWithValue("@LandTaxedPerson", room.LandTaxedPerson);
                        command.Parameters.AddWithValue("@RoomId", room.RoomId);

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

        public void DeleteRoom(Room room)
        {
            string sqlCommand = "delete from rooms where [RoomId] = @RoomId";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand command = new OleDbCommand(sqlCommand, con))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@RoomId", room.RoomId);

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

        public List<Room> SearchRooms(string searchValue)
        {
            List<Room> rooms = new List<Room>();
            string sqlCommand = @"select * from rooms where RoomId = @RoomId or ContractName like @ContactName";
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                OleDbCommand command = new OleDbCommand(sqlCommand, con);
                try
                {
                    command.Parameters.AddWithValue("@RoomId", searchValue);
                    command.Parameters.AddWithValue("@ContactName", "%" + searchValue + "%");
                    con.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        foreach (var item in reader)
                        {
                            Room r = new Room()
                            {
                                RoomId = long.Parse(reader["RoomId"].ToString()),
                                RoomNo = reader["RoomNo"].ToString(),
                                CustomerId = long.Parse(reader["CustomerId"].ToString()),
                                WUnitStart = long.Parse(reader["WUnitStart"].ToString()),
                                EUnitStart = long.Parse(reader["EUnitStart"].ToString()),
                                MonthCost = Decimal.Parse(reader["MonthCost"].ToString()),
                                InsureCost = Decimal.Parse(reader["InsureCost"].ToString()),
                                StartDate = reader["StartDate"].ToString() != "" ? DateTime.Parse(reader["StartDate"].ToString()) : (DateTime?)null,
                                ApartmentId = long.Parse(reader["ApartMentId"].ToString()),
                                Floor = reader["Floor"].ToString(),
                                Picture = reader["Picture"].ToString(),
                                ContractMonth = long.Parse(reader["ContractMonth"].ToString()),
                                LandTaxedPerson = bool.Parse(reader["LandTaxedPerson"].ToString())
                            };
                            rooms.Add(r);
                        }

                        return rooms;
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
