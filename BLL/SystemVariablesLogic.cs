using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMSDesktop.DAL.Model;
using AMSDesktop.DAL.Repository;

namespace AMSDesktop.BLL
{
    public class SystemVariablesLogic
    {
        public SystemVariable GetSystemVariable(long apartmentId)
        {
            return new SystemVariablesRepository().GetSystemVariable(apartmentId);
        }
    }
}
