using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public int SubjectId { get; set; }
        public int Unit { get; set; }
        public string ImgContent { get; set; }
        public string Content { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public string CorrectAnswer { get; set; }

        public Question Question { get; set; }
        public Subject Subject { get; set; }
    }
}