using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Choice.Common;
using Web.Choice.Service.Interfaces;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Service.Implementation
{
    public class StudentSer:IStudentSer
    {
        private readonly User _user = new User();
        private readonly TestExamEntities _db = new TestExamEntities();
        public void UpdateLastLogin()
        {
            try
            {
                var update = (from x in _db.Students where x.StudentId == _user.ID select x).Single();
                if (update == null)
                {
                    throw new Exception("not update");
                }
                update.LastLogin = DateTime.Now;
                _db.SaveChanges();
            }
            catch (Exception )
            {
                throw;
            }
        }

        public void UpdateLastSeen(string name, string url)
        {
            try
            {
                var update = (from x in _db.Students where x.StudentId == _user.ID select x).Single();
                if (update == null)
                {
                    throw new Exception("update is null");
                }
                update.LastSeen = name;
                update.LastSeenUrl = url;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<TestModel> GetDashboard()
        {
            return (from x in _db.Tests
                join s in _db.Subjects on x.SubjectId equals s.SubjectId
                join stt in _db.Statuses on x.StatusId equals stt.StatusId
                select new TestModel {SUBJECT = s, TEST = x, STATUS = stt}).ToList();
        }
        public List<TestModel> GetDashboardForMe()
        {

            return (from x in _db.Tests
                join s in _db.Subjects on x.SubjectId equals s.SubjectId
                join stt in _db.Statuses on x.StatusId equals stt.StatusId
                join sc in _db.Scores on x.TestCode equals sc.TestCode
                join st in _db.Students on sc.StudentId equals st.StudentId
                select new TestModel { SUBJECT = s, TEST = x, STATUS = stt ,SCORE = sc,STUDENT = st}).ToList();
        }
        public Test GetTest(int testCode)
        {
            var test = new Test();
            try
            {
                test = _db.Tests.SingleOrDefault(x => x.TestCode == testCode);
            }
            catch (Exception e)
            {
                throw;
            }
            return test;
        }

        public void UpdateStatus(int testCode, string timeRemaining)
        {
            try
            {
                var update = (from x in _db.Students where x.StudentId == _user.ID select x).Single();
                if (update == null)
                {
                    throw new Exception("Update Status is null");
                }
                update.IsTestting = testCode;
                update.TimeStart = DateTime.Now;
                update.TimeRemaining = timeRemaining;
                _db.SaveChanges();
                HttpContext.Current.Session[UserSession.TESTCODE] = testCode;
                _user.TESTCODE = testCode;
                HttpContext.Current.Session[UserSession.TIME] = timeRemaining;
                _user.TIME = timeRemaining;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void CreateStudentQuestion(int code)
        {
            try
            {
                var questions = (from x in _db.QuestOfTests
                    where x.TestCode == code
                    select x).OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var item in questions)
                {
                    var studentTestDetail = new StudentTestDetail
                    {
                        QuestionId = item.QuestionId,
                        TestCode = item.TestCode,
                        StudentId = _user.ID
                    };
                    var question = _db.Questions.SingleOrDefault(x => x.QuestionId == item.QuestionId);
                    if (question == null)
                    {
                        throw new Exception("Create Student Question is null");
                    }
                    string[] answer = { question.AnswerA, question.AnswerB, question.AnswerC, question.AnswerD };
                    answer = ShuffleArray.Randomize(answer);
                    studentTestDetail.AnswerA = answer[0];
                    studentTestDetail.AnswerB = answer[1];
                    studentTestDetail.AnswerC = answer[2];
                    studentTestDetail.AnswerD = answer[3];
                    _db.StudentTestDetails.Add(studentTestDetail);
                    _db.SaveChanges();
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<StudentQuestModel> GetListQuest(int testCode)
        {
            try
            {
                return (from x in _db.StudentTestDetails
                    join t in _db.Tests on x.TestCode equals t.TestCode
                    join q in _db.Questions on x.QuestionId equals q.QuestionId
                    where x.StudentId == _user.ID && x.TestCode == testCode
                    select new StudentQuestModel {QUESTION = q, STUDENTTEST = x, TEST = t}).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateTiming(string time)
        {
            var update = (from x in _db.Students where x.StudentId == _user.ID select x).Single();
            if (update == null)
            {
                throw new Exception("UpdateTiming is null");
            }

            update.TimeRemaining = time;
            HttpContext.Current.Session[UserSession.TIME] = time;
            _user.TIME = time;
            _db.SaveChanges();
        }

        public void UpdateStudentTest(int questionId, string answer)
        {
            var update = (from x in _db.StudentTestDetails
                where x.StudentId == _user.ID && x.TestCode == _user.TESTCODE && x.QuestionId == questionId
                select x).Single();
            if (update == null)
            {
                throw new Exception("not update");
            }
            update.StudentAnswer = answer;
            _db.SaveChanges();
        }

        public void InsertScore(double score, string detail)
        {
            var student = new Score
            {
                StudentId = _user.ID,
                TestCode = _user.TESTCODE,
                ScoreNumber = score,
                Detail = detail,
                TimeFinish = DateTime.Now,
            };
            if (student == null)
            {
                throw new Exception("Not insert score");
            }
            _db.Scores.Add(student);
            _db.SaveChanges();
        }

        public void FinishTest()
        {
            var update = (from x in _db.Students
                where x.StudentId == _user.ID
                select x).Single();
            if (update == null)
            {
                throw new Exception("update Finish Test is null");
            }

            update.IsTestting = null;
            update.TimeRemaining = null;
            update.TimeStart = null;
            _db.SaveChanges();
            HttpContext.Current.Session[UserSession.TESTCODE] = 0;
            _user.TESTCODE = 0;
            HttpContext.Current.Session[UserSession.TIME] = null;
            _user.TIME = null;
        }

        public Score GetScore(int testCode)
        {
            var score = new Score();
            try
            {
                score = _db.Scores.SingleOrDefault(x => x.TestCode == testCode && x.StudentId==_user.ID);
            }
            catch (Exception e)
            {
                throw;
            }

            return score;
        }
        public List<int> GetStudentTestCode()
        {
            var score = new List<int>();
            try
            {
                score = (from x in _db.Scores where x.StudentId == _user.ID select x.TestCode).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
            return score;
        }
    }
}
