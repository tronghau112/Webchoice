using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SpecialityId { get; set; }
        public int GradeId { get; set; }

        public Class Class { get; set; }
        public Grade Grade { get; set; }
        public Speciality Speciality { get; set; }
    }
}