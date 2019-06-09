using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class StudentQuestViewModel
    {
        public Test Test { get; set; }
        public Question Question { get; set; }
        public StudentTestDetail StudentTestDetail { get; set; }
    }
}