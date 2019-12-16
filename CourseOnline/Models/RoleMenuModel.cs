using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class RoleMenuModel : Menu
    {
        public int role_menu_id { get; set; }
        public bool roll_menu_status { get; set; }
        public int total
        {
            get {
                if (roll_menu_status == true)
                    return role_menu_id * 10 + 1;
                else
                    return role_menu_id * 10;
            }
            set {
                if (roll_menu_status == true)
                    total = role_menu_id * 10 + 1;
                else
                    total = role_menu_id * 10;
            }
        }
    }
}