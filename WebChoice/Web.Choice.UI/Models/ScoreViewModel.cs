using System;
using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class ScoreViewModel
    {
        public int ScoreId { get; set; }
        public int StudentId { get; set; }
        public int TestCode { get; set; }
        public double ScoreNumber { get; set; }
        public string Detail { get; set; }
        public string rank { get; set; }
        public Nullable<System.DateTime> TimeFinish { get; set; }

        public Score Score { get; set; }
        public Student Student { get; set; }
        public Test Test { get; set; }
    }
}