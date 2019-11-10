using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class SubjectListModel : Subject
    {
        public int lesson_count { get; set; }
    }
}