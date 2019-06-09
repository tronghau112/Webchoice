using System.Collections.Generic;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Service.Implementation;
using Web.Choice.Service.Interfaces;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Bussiness.Implementation
{
    public class StudentBus : IStudentBus
    {
        private IStudentSer _studentSer = null;
        private IStudentSer Student => _studentSer ?? (_studentSer = new StudentSer());
        public void CreateStudentQuestion(int code)
        {
            Student.CreateStudentQuestion(code);
        }

        public void FinishTest()
        {
            Student.FinishTest();
        }

        public List<TestModel> GetDashboard()
        {
            return Student.GetDashboard();
        }

        public List<StudentQuestModel> GetListQuest(int testCode)
        {
            return Student.GetListQuest(testCode);
        }

        public Score GetScore(int testCode)
        {
            return Student.GetScore(testCode);
        }

        public List<int> GetStudentTestCode()
        {
            return Student.GetStudentTestCode();
        }

        public List<TestModel> GetDashboardForMe()
        {
            return Student.GetDashboardForMe();
        }

        public Test GetTest(int testCode)
        {
            return Student.GetTest(testCode);
        }

        public void InsertScore(double score, string detail)
        {
            Student.InsertScore(score, detail);
        }

        public void UpdateLastLogin()
        {
            Student.UpdateLastLogin();
        }

        public void UpdateLastSeen(string name, string url)
        {
            Student.UpdateLastSeen(name, url);
        }

        public void UpdateStatus(int testCode, string timeRemaining)
        {
            Student.UpdateStatus(testCode, timeRemaining);
        }

        public void UpdateStudentTest(int questionId, string answer)
        {
            Student.UpdateStudentTest(questionId, answer);
        }

        public void UpdateTiming(string time)
        {
            Student.UpdateTiming(time);
        }
    }
}
