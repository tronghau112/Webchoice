using System;
using System.Collections.Generic;
using System.Linq;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Common;
using Web.Choice.Service.Implementation;
using Web.Choice.Service.Interfaces;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Bussiness.Implementation
{
    public class AdminBus : IAdminBus
    {
        private IAdminSer _adminSer = null;
        private IAdminSer Admin => _adminSer ?? (_adminSer = new AdminSer());
        public bool AddAdmin(string name, string username, string password, string gender, string email, string birthday)
        {
            try
            {
                var checkUserName = GetAdmins().Find(x => x.UserName == username).IsEmpty();
                var checkEmail = GetAdmins().Find(x => x.Email == email).IsEmpty();
                if (!checkUserName)
                {
                    return false;
                }
                if (!checkEmail)
                {
                    return false;
                }
                if (username.IsEmpty() || password.IsEmpty() || name.IsEmpty() || gender.IsEmpty() || birthday.IsEmpty())
                {
                    return false;
                }

                return Admin.AddAdmin(name, username, password, gender, email, birthday);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public bool DeleteAdmin(int id)
        {
            return Admin.DeleteAdmin(id);
        }

        public bool EditAdmin(int adminId, string name, string username, string password, string gender, string email, string birthday)
        {
            return Admin.EditAdmin(adminId, name, username, password, gender, email, birthday);
        }

        public List<TeacherModel> GetTeachers()
        {
            return Admin.GetTeachers();
        }

        public List<Speciality> GetSpecialities()
        {
            return Admin.GetSpecialities();
        }

        public bool AddTeacher(string name, string username, string password, string gender, string email, string birthday, int specialityId)
        {
            var checkUserName = GetTeachers().Find(x => x.TEACHER.UserName == username).IsEmpty();
            var checkEmail = GetTeachers().Find(x => x.TEACHER.Email == email).IsEmpty();
            if (!checkUserName)
            {
                return false;
            }
            if (!checkEmail)
            {
                return false;
            }
            if (username.IsEmpty() || password.IsEmpty() || name.IsEmpty() || gender.IsEmpty() || birthday.IsEmpty())
            {
                return false;
            }

            return Admin.AddTeacher(name, username, password, gender, email, birthday, specialityId);
        }

        public bool DeleteTeacher(int id)
        {
            return Admin.DeleteTeacher(id);
        }

        public Teacher GetTeacher(int id)
        {
            return Admin.GetTeacher(id);
        }

        public bool EditTeacher(int teacherId, string name, string username, string password, string gender, string email, string birthday, int specialityId)
        {
            return Admin.EditTeacher(teacherId, name, username, password, gender, email, birthday, specialityId);
        }

        public List<Class> GetClasses()
        {
            return Admin.GetClasses();
        }

        public List<StudentModel> GetStudents()
        {
            return Admin.GetStudents();
        }

        public bool AddStudent(string name, string username, string password, string gender, string email, string birthday, int specialityId, int classId)
        {
            var checkUserName = GetStudents().Find(x => x.STUDENT.UserName == username).IsEmpty();
            var checkEmail = GetStudents().Find(x => x.STUDENT.Email == email).IsEmpty();
            if (!checkUserName)
            {
                return false;
            }
            if (!checkEmail)
            {
                return false;
            }
            if (username.IsEmpty() || password.IsEmpty() || name.IsEmpty() || gender.IsEmpty() || birthday.IsEmpty())
            {
                return false;
            }

            return Admin.AddStudent(name, username, password, gender, email, birthday, specialityId, classId);
        }

        public bool DeleteStudent(int id)
        {
            return Admin.DeleteStudent(id);
        }

        public Student GetStudent(int id)
        {
            return Admin.GetStudent(id);
        }

        public bool EditStudent(int studentId, string name, string username, string password, string gender, string email, string birthday, int specialityId, int classId)
        {
            return Admin.EditStudent(studentId, name, username, password, gender, email, birthday, specialityId, classId);
        }

        public List<Grade> GetGrades()
        {
            return Admin.GetGrades();
        }

        public List<ClassModel> GetClassesJoin()
        {
            return Admin.GetClassesJoin();
        }

        public bool AddGrade(string gradeName)
        {
            var checkGradeName = GetGrades().Find(x => x.GradeName == gradeName).IsEmpty();
            if (!checkGradeName)
            {
                return false;

            }
            return Admin.AddGrade(gradeName);
        }

        public bool AddClass(string className, int gradeId, int specialityId)
        {
            if (className.IsEmpty() || gradeId.IsEmpty() || specialityId.IsEmpty())
            {
                return false;
            }
            return Admin.AddClass(className, gradeId, specialityId);
        }

        public bool DeleteClass(int id)
        {
            return Admin.DeleteClass(id);
        }

        public Class GetClass(int id)
        {
            return Admin.GetClass(id);
        }

        public bool EditClass(int classId, string className, int specialityId, int gradeId)
        {
            return Admin.EditClass(classId, className, specialityId, gradeId);
        }

        public bool AddSpeciality(string specialityName)
        {
            var checkSpecialityName = GetSpecialities().Find(x => x.SpecialityName == specialityName).IsEmpty();
            if (!checkSpecialityName)
            {
                return false;
            }
            return Admin.AddSpeciality(specialityName);
        }

        public bool DeleteSpeciality(int id)
        {
            return Admin.DeleteSpeciality(id);
        }

        public Speciality GetSpeciality(int id)
        {

            return Admin.GetSpeciality(id);
        }

        public bool EditSpeciality(int specialityId, string specialityName)
        {
            return Admin.EditSpeciality(specialityId, specialityName);
        }

        public List<Subject> GetSubjects()
        {
            return Admin.GetSubjects();
        }

        public bool AddSubject(string subjectName)
        {
            var checkSubjectName = GetSubjects().Find(x => x.SubjectName == subjectName).IsEmpty();
            if (!checkSubjectName)
            {
                return false;
            }

            return Admin.AddSubject(subjectName);
        }

        public bool DeleteSubject(int id)
        {
            return Admin.DeleteSubject(id);
        }

        public Subject GetSubject(int id)
        {
            return Admin.GetSubject(id);
        }

        public bool EditSubject(int subjectId, string subjectName)
        {
            return Admin.EditSubject(subjectId, subjectName);
        }

        public List<QuestionModel> GetQuestions()
        {
            return Admin.GetQuestions();
        }

        public bool AddQuestion(int subjectId, int unit, string content, string contentImg, string answerA, string answerB,
            string answerC, string answerD, string correctAnswer)
        {
            var checkContent = GetQuestions().Find(x => x.QUESTION.Content == content).IsEmpty();
            var checkAnswerA = GetQuestions().Find(x =>( x.QUESTION.AnswerA == answerA || x.QUESTION.AnswerA == answerB || x.QUESTION.AnswerA == answerC || x.QUESTION.AnswerA == answerD)).IsEmpty();
            var checkAnswerB = GetQuestions().Find(x => (x.QUESTION.AnswerB == answerB || x.QUESTION.AnswerB == answerA || x.QUESTION.AnswerB == answerC || x.QUESTION.AnswerB == answerD)).IsEmpty();
            var checkAnswerC = GetQuestions().Find(x => (x.QUESTION.AnswerC == answerC || x.QUESTION.AnswerC == answerA || x.QUESTION.AnswerC == answerB || x.QUESTION.AnswerC == answerD)).IsEmpty();
            var checkAnswerD = GetQuestions().Find(x => (x.QUESTION.AnswerD == answerD || x.QUESTION.AnswerD == answerA || x.QUESTION.AnswerD == answerB || x.QUESTION.AnswerD == answerC)).IsEmpty();

            if (!checkContent)
            {
                if (!checkAnswerA&& !checkAnswerB&& !checkAnswerC&&!checkAnswerD)
                {
                    return false;
                }
            }
            return Admin.AddQuestion(subjectId, unit, content, contentImg, answerA, answerB, answerC, answerD,
                correctAnswer);
        }

        public bool DeleteQuestion(int id)
        {
            return Admin.DeleteQuestion(id);
        }

        public Question GetQuestion(int id)
        {
            return Admin.GetQuestion(id);
        }

        public bool EditQuestion(int questionId, int subjectId, int unit, string content, string contentImg, string answerA,
            string answerB, string answerC, string answerD, string correctAnswer)
        {
            return Admin.EditQuestion(questionId, subjectId, unit, content, contentImg, answerA, answerB, answerC,
                answerD, correctAnswer);
        }

        public List<UnitModel> GetUnits(int id)
        {
            return Admin.GetUnits(id);
        }

        public bool AddTest(string testName, string password, int testCode, int subjectId, int totalQuestion, int timeToDo,
            string note)
        {
            var checkTest = Tests().Find(x => x.TEST.TestCode == testCode).IsEmpty();
            if (!checkTest)
            {
                return false;
            }
            return Admin.AddTest(testName, password, testCode, subjectId, totalQuestion, timeToDo, note);
        }

        public Test GetTest(int testCode)
        {
            return Admin.GetTest(testCode);
        }

        public bool EditTest(int testCode, string testName, string password, int timeToDo, string note)
        {
            return Admin.EditTest(testCode, testName, password, timeToDo, note);
        }

        public bool ToggleStatus(int testId)
        {
            return Admin.ToggleStatus(testId);
        }

        public List<Question> GetQuestionsByUnit(int subjectId, int unit, int questOfUnit)
        {
            return Admin.GetQuestionsByUnit(subjectId, unit, questOfUnit);
        }

        public bool AddQuestionsToTest(int testCode, int questionId)
        {
            return Admin.AddQuestionsToTest(testCode, questionId);
        }

        public List<Question> GetQuestionsOfTest(int testCode)
        {
            return Admin.GetQuestionsOfTest(testCode);
        }

        public List<TestModel> Tests()
        {
            return Admin.Tests();
        }

        public Admin GetAdmin(int id)
        {
            return Admin.GetAdmin(id);
        }

        public List<Admin> GetAdmins()
        {
            return Admin.GetAdmins().ToList();
        }

        public Dictionary<string, int> GetDashBoard()
        {
            return Admin.GetDashBoard();
        }
        public void UpdateLastLogin()
        {
            Admin.UpdateLastLogin();
        }
        public void UpdateLastSeen(string name, string url)
        {
            Admin.UpdateLastSeen(name, url);
        }
    }
}
