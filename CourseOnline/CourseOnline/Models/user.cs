//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CourseOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Courses = new HashSet<Course>();
            this.Registrations = new HashSet<Registration>();
            this.UserRoles = new HashSet<UserRole>();
        }
    
        public int user_id { get; set; }
        public string user_group { get; set; }
        public string user_fullname { get; set; }
        public string user_email { get; set; }
        public string use_mobile { get; set; }
        public bool user_gender { get; set; }
        public bool user_status { get; set; }
    
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
