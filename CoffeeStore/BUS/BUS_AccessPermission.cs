using CoffeeStore.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_AccessPermission
    {
        DAL_AccessPermission dalAccPer = new DAL_AccessPermission();
        public DataTable GetAccessInfo(int limit, int offset)
        {
            return dalAccPer.GetAccessInfo(limit, offset);
        }

        public DataTable GetAccessPermission()
        {
            return dalAccPer.GetAccessPermissions();
        }    
    }
}
