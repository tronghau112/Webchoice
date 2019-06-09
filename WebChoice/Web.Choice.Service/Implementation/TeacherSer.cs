using System;
using System.Collections.Generic;
using System.Linq;
using Web.Choice.Common;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;
using Web.Choice.Service.Interfaces;

namespace Web.Choice.Service.Implementation
{
    public class TeacherSer:ITeacherSer
    {
        private readonly User _user = new User();
        private readonly TestExamEntities _db = new TestExamEntities();
        public void UpdateLastLogin()
        {
            var update = (from x in _db.Teachers where x.TeacherId == _user.ID select x).Single();
            update.LastLogin = DateTime.Now;
            _db.SaveChanges();
        }

        public void UpdateLastSeen(string name, string url)
        {
            var update = (from x in _db.Teachers where x.TeacherId == _user.ID select x).Single();
            update.LastSeen = name;
            update.LastSeenUrl = url;
            _db.SaveChanges();
        }

        public List<TestModel> GetListTest()
        {
            return (from x in _db.Tests
                join s in _db.Subjects on x.SubjectId equals s.SubjectId
                join stt in _db.Statuses on x.StatusId equals stt.StatusId
                select new TestModel {TEST = x, SUBJECT = s, STATUS = stt}).ToList();
        }

        public List<ScoreModel> GetListScore(int testCode)
        {
            var score = new List<ScoreModel>();
            try
            {
                score = (from x in _db.Scores
                    join s in _db.Students on x.StudentId equals s.StudentId
                    where x.TestCode == testCode
                    select new ScoreModel { SCORE = x, STUDENT = s}).ToList();
            }
            catch (Exception e)
            {
                throw;
            }

            return score;
        }
    }
}
