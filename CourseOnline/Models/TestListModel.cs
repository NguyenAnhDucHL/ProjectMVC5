using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class TestListModel : Exam
    {
        public String test_name { get; set; }

        public String exam_note { get; set; }

        public int tested { get; set; }

        public String average { get; set; }
    }
}