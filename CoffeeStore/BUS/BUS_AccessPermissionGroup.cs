﻿using CoffeeStore.DAL;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.BUS
{
    class BUS_AccessPermissionGroup
    {
        DAL_AccessPermissionGroup dalAccPerGr = new DAL_AccessPermissionGroup();
        public bool CreateAccessPermissionGroup(DTO_AccessPermissionGroup newAccPerGr)
        {
            return dalAccPerGr.CreateAccessPermissionGroup(newAccPerGr);
        }    

        public bool DeleteAccessPermissionGroup(DTO_AccessPermissionGroup accPerGr)
        {
            return dalAccPerGr.DeleteAccessPermissionGroup(accPerGr);
        }    

        public bool DeleteByEmpTypeID(string empTypeID)
        {
            return dalAccPerGr.DeleteByEmpTypeID(empTypeID);
        }    
    }
}
