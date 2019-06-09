using System.Collections.Generic;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Service.Interfaces
{
    public interface ITeacherSer
    {
        void UpdateLastLogin();
        void UpdateLastSeen(string name, string url);
        List<TestModel> GetListTest();
        List<ScoreModel> GetListScore(int testCode);
    }
}
