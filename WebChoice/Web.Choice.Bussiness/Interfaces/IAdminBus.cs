using System.Collections.Generic;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Bussiness.Interfaces
{
    public interface IAdminBus
    {
        void UpdateLastLogin();
        void UpdateLastSeen(string name, string url);
        Dictionary<string, int> GetDashBoard();
        List<Admin> GetAdmins();
        Admin GetAdmin(int id);

        bool AddAdmin(string name, string username, string password, string gender, string email, string birthday);
        bool DeleteAdmin(int id);

        bool EditAdmin(int adminId, string name, string username, string password, string gender, string email, string birthday);
        List<TeacherModel> GetTeachers();
        List<Speciality> GetSpecialities();
        bool AddTeacher(string name, string username, string password, string gender, string email, string birthday, int specialityId);
        bool DeleteTeacher(int id);
        Teacher GetTeacher(int id);

        bool EditTeacher(int teacherId, string name, string username, string password, string gender, string email, string birthday, int specialityId);
        List<Class> GetClasses();
        List<StudentModel> GetStudents();

        bool AddStudent(string name, string username, string password, string gender, string email, string birthday, int specialityId, int classId);
        bool DeleteStudent(int id);
        Student GetStudent(int id);

        bool EditStudent(int studentId, string name, string username, string password, string gender, string email, string birthday, int specialityId, int classId);
        List<Grade> GetGrades();
        List<ClassModel> GetClassesJoin();
        bool AddGrade(string gradeName);
        bool AddClass(string className, int gradeId, int specialityId);
        bool DeleteClass(int id);
        Class GetClass(int id);
        bool EditClass(int classId, string className, int specialityId, int gradeId);
        bool AddSpeciality(string specialityName);
        bool DeleteSpeciality(int id);
        Speciality GetSpeciality(int id);
        bool EditSpeciality(int specialityId, string specialityName);
        List<Subject> GetSubjects();
        bool AddSubject(string subjectName);
        bool DeleteSubject(int id);
        Subject GetSubject(int id);
        bool EditSubject(int subjectId, string subjectName);
        List<QuestionModel> GetQuestions();
        bool AddQuestion(int subjectId, int unit, string content, string contentImg, string answerA, string answerB, string answerC, string answerD, string correctAnswer);
        bool DeleteQuestion(int id);
        Question GetQuestion(int id);

        bool EditQuestion(int questionId, int subjectId, int unit, string content, string contentImg, string answerA, string answerB, string answerC, string answerD, string correctAnswer);
        List<UnitModel> GetUnits(int id);

        bool AddTest(string testName, string password, int testCode, int subjectId, int totalQuestion, int timeToDo,
            string note);

        Test GetTest(int testCode);

        bool EditTest(int testCode, string testName, string password, int timeToDo, string note);

        bool ToggleStatus(int testId);
        List<Question> GetQuestionsByUnit(int subjectId, int unit, int questOfUnit);
        bool AddQuestionsToTest(int testCode, int questionId);
        List<Question> GetQuestionsOfTest(int testCode);
        List<TestModel> Tests();
    }
}
