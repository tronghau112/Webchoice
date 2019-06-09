using System;
using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public System.DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public int PermissionId { get; set; }
        public int ClassId { get; set; }
        public int SpecialityId { get; set; }
        public Nullable<int> IsTestting { get; set; }
        public Nullable<System.DateTime> TimeStart { get; set; }
        public string TimeRemaining { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string LastSeen { get; set; }
        public string LastSeenUrl { get; set; }
        public Nullable<System.DateTime> TimeStamps { get; set; }

        public Student Student { get; set; }
        public Speciality Speciality { get; set; }
        public Class Class { get; set; }
    }
}