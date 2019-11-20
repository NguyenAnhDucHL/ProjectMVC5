using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class TestExamModel : ExamTest
    {
        public string exam_level { get; set; }
        public string exam_name { get; set; }
        public bool exam_is_practice { get; set; }
        public int exam_duration { get; set; }
        public string test_type { get; set; }
        public int pass_rate { get; set; }
        public string average { get; set; }
        public int tested { get; set; }
        public string question_name { get; set; }
        public ICollection<AnswerOption> answers { get; set; }
        public int question_id { get; set; }
    }
}