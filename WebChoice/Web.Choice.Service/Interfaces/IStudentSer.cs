using System.Collections.Generic;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Service.Interfaces
{
    public interface IStudentSer
    {
        void UpdateLastLogin();
        void UpdateLastSeen(string name, string url);
        List<TestModel> GetDashboard();
        Test GetTest(int testCode);
        void UpdateStatus(int testCode, string timeRemaining);
        void CreateStudentQuestion(int code);
        List<StudentQuestModel> GetListQuest(int testCode);
        void UpdateTiming(string time);
        void UpdateStudentTest(int questionId, string answer);
        void InsertScore(double score, string detail);
        void FinishTest();
        Score GetScore(int testCode);
        List<int> GetStudentTestCode();
        List<TestModel> GetDashboardForMe();
    }
}
