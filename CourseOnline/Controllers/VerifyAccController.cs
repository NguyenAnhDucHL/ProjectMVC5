using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseOnline.Models;
using System.Data.SqlClient;

namespace CourseOnline.Controllers
{
    public class VerifyAccController : Controller
    {
        public Boolean Permission(String email, String permission_link)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "Select P.permission_link " +
                            "from Permission P  " +
                            "join RolePermission RP on P.permission_id = RP.permission_id " +
                            "join Roles R on RP.role_id = R.role_id " +
                            "join UserRole UR on UR.role_id = R.role_id " +
                            "join [User] U on U.[user_id] = UR.[user_id] " +
                            "where U.user_email = @email";

                List<String> permissions = db.Database.SqlQuery<String>(sql, new SqlParameter("email", email)).ToList();
                foreach (String permission in permissions)
                {
                    if (permission.Equals(permission_link))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public String Menu(String email, String menu_link)
        {
            using (STUDYONLINEEntities db = new STUDYONLINEEntities())
            {
                string sql = "select ur.role_id " +
                            "from[User] u " +
                            "join UserRole ur on u.[user_id] = ur.[user_id] " +
                            "where u.user_email = @email";

                var role_id = db.Database.SqlQuery<int>(sql, new SqlParameter("email", email)).FirstOrDefault();
                if (role_id == 3)
                {
                    return "Student";
                }
                if (role_id == 1)
                {
                    return "Admin";
                }
                else
                {
                    sql = "Select M.menu_link " +
                             "from Menu M " +
                             "join RoleMenu RM on M.menu_id = RM.menu_id " +
                             "where rm.role_id = @role_id";

                    List<String> menus = db.Database.SqlQuery<String>(sql, new SqlParameter("role_id", role_id)).ToList();
                    foreach (String menu in menus)
                    {
                        if (menu.Equals(menu_link))
                        {
                            return "Accept";
                        }
                    }
                    return "Reject";
                }
            }
        }
    }
}