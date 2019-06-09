namespace Web.Choice.Service.Interfaces
{
    public interface ILoginSer
    {
        void SetAdminSession(int userId);
        void SetTeacherSession(int userId);
        void SetStudentSession(int userId);
        bool IsValid(string username, string password);
    }
}
