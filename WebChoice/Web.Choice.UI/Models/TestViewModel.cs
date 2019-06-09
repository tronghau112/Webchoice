using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class TestViewModel
    {
        public string TestName { get; set; }
        public int TestCode { get; set; }
        public string Password { get; set; }
        public int SubjectId { get; set; }
        public int QuestionsTotal { get; set; }
        public int TimeToDo { get; set; }
        public string Note { get; set; }
        public int StatusId { get; set; }

        public Test Test { get; set; }
        public Subject Subject { get; set; }
        public Status Status { get; set; }
        public Score Score { get; set; }
        public StudentTestDetail StudentTestDetail { get; set; }
        public Student Student { get; set; }
    }
}