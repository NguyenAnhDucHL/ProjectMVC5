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
}