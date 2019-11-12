using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class TestListModel : ExamTest
    {
        public String exam_name { get; set; }

        public int tested { get; set; }

        public String average { get; set; }
    }
}