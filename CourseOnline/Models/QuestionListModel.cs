using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class QuestionListModel : Question
    {
        public String subject_name { get; set; }
        public String lesson_name { get; set; }
        public String domain_name { get; set; }
    }
}