﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class ExamListModel : Exam
    {
        public String subject_name { get; set; }

        public String pass_rate { get; set; }
        public String test_type { get; set; }
    }
}