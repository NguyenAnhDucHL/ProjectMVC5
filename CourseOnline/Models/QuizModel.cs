using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseOnline.Models
{
    public class QuestionModel
    {
        public int questionID { get; set; }
        public string questiontext { get; set; }
        public string subjectname { get; set; }
        public ICollection<AnswerModel> answers { get; set; }

    }
    public class AnswerModel
    {
        public int answerID { get; set; }
        public string answertext { get; set; }
        public bool isCorrect { get; set; }
    }
    public class QuizResultModel
    {
        public int questionID { get; set; }
        public string questiontext { get; set; }
        public string answertext { get; set; }
        public bool isCorrect { get; set; }
        public string answercorrect { get; set; }
    }

    public class LessonQuizModel :Lesson
    {
        public int test_id { get; set; }
        public string test_name { get; set; }
        public int exam_duration { get; set; }
        public string due_date { get; set; }
    }
}