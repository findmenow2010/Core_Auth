using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthCoreApp.ViewModels
{
    public class UserRoleVm
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
