using System.Collections.Generic;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;
using Web.Choice.Service.Implementation;
using Web.Choice.Service.Interfaces;

namespace Web.Choice.Bussiness.Implementation
{
    public class TeacherBus:ITeacherBus
    {
        private ITeacherSer _teacherSer = null;
        private ITeacherSer Teacher => _teacherSer ?? (_teacherSer = new TeacherSer());
        public void UpdateLastLogin()
        {
            Teacher.UpdateLastLogin();
        }

        public void UpdateLastSeen(string name, string url)
        {
            Teacher.UpdateLastSeen(name, url);
        }

        public List<TestModel> GetListTest()
        {
            return Teacher.GetListTest();
        }

        public List<ScoreModel> GetListScore(int testCode)
        {
            return Teacher.GetListScore(testCode);
        }
    }
}
