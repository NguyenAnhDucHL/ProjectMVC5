using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class RolePermissionsModel : RolePermission
    {
        public String permission_name { get; set; }
    }
}