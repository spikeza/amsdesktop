using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Repository;
using AMSDesktop.DAL.Model;

namespace AMSDesktop.BLL
{
    public class RoomsLogic
    {
        public List<Room> GetRooms()
        {
            return new RoomsRepository().GetRooms();
        }

        public List<Room> GetRooms(long apartmentId)
        {
            return new RoomsRepository().GetRooms(apartmentId);
        }

        public List<RoomDropDownView> GetRoomsForDropDownList(long apartmentId)
        {
            return new RoomsRepository().GetRoomsForDropDownList(apartmentId);
        }

        public Room GetRoom(long roomId)
        {
            return new RoomsRepository().GetRoom(roomId);
        }
        public void AddRoom(Room room)
        {
            new RoomsRepository().AddRoom(room);
        }

        public void UpdateRoom(Room room)
        {
            new RoomsRepository().UpdateRoom(room);
        }

        public void DaleteRoom(Room room)
        {
            new RoomsRepository().DeleteRoom(room);
        }

        public List<Room> SearchRooms(string searchValue, long apartmentId)
        {
            return new RoomsRepository().SearchRooms(searchValue, apartmentId);
        }

        public void UpdateRoomMeterStart(Room room)
        {
            new RoomsRepository().UpdateRoomMeterStart(room);
        }
    }
}
