using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Auth
{
    public class RolePrivilege
    {
        public int Id {get; set;}
        public int RoleId {get; set;}
        public int PrivilegeId {get; set;}

        public Role Role {get; set;}
        public Privilege Privilege {get; set;}
    }
}