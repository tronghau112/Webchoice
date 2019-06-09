using System;
using System.Collections.Generic;
using System.Linq;
using Web.Choice.Common;
using Web.Choice.Service.Interfaces;
using Web.Choice.Entity;
using Web.Choice.Entity.ExtendModels;

namespace Web.Choice.Service.Implementation
{
    public class AdminSer : IAdminSer
    {
        private readonly User _user = new User();
        private readonly TestExamEntities _db = new TestExamEntities();
        public Dictionary<string, int> GetDashBoard()
        {
            var listCount = new Dictionary<string, int>();
            var countAdmin = _db.Admins.Count();
            listCount.Add("countAdmin", countAdmin);
            var countTeacher = _db.Teachers.Count();
            listCount.Add("countTeacher", countTeacher);
            int countStudent = _db.Students.Count();
            listCount.Add("countStudent", countStudent);
            int countGrade = _db.Grades.Count();
            listCount.Add("countGrade", countGrade);
            int countClass = _db.Classes.Count();
            listCount.Add("countClass", countClass);
            int countSpeciality = _db.Specialities.Count();
            listCount.Add("countSpeciality", countSpeciality);
            int countSubject = _db.Subjects.Count();
            listCount.Add("countSubject", countSubject);
            int countQuestion = _db.Questions.Count();
            listCount.Add("countQuestion", countQuestion);
            int countTest = _db.Tests.Count();
            listCount.Add("countTest", countTest);
            return listCount;
        }

        public List<Admin> GetAdmins()
        {
            return _db.Admins.ToList();
        }

        public Admin GetAdmin(int id)
        {
            var admin = new Admin();
            try
            {
                admin = _db.Admins.SingleOrDefault(x => x.AdminId == id);
            }
            catch (Exception)
            {
                throw;
            }
            return admin;
        }

        public bool AddAdmin(string name, string username, string password, string gender, string email, string birthday)
        {
            var addAdmin = new Admin
            {
                Name = name,
                UserName = username,
                Password = password,
                Gender = gender,
                Email = email,
                Birthday = Convert.ToDateTime(birthday),
                PermissionId = 1,
                Avatar = "avatar-default.jpg"
            };
         
            try
            {
                _db.Admins.Add(addAdmin);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Thêm admin thất bại");
            }

            return true;
        }

        public bool DeleteAdmin(int id)
        {
            var deleteAdmin = (from x in _db.Admins where x.AdminId == id select x).Single();
            try
            {
                _db.Admins.Remove(deleteAdmin);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Xoá admin thất bại");
            }

            return true;
        }

        public bool EditAdmin(int adminId, string name, string username, string password, string gender, string email, string birthday)
        {
            var updateAdmin = (from x in _db.Admins where x.AdminId == adminId select x).Single();
            try
            {
                updateAdmin.Name = name;
                updateAdmin.UserName = username;
                updateAdmin.Password = password;
                updateAdmin.Gender = gender;
                updateAdmin.Email = email;
                updateAdmin.Birthday = Convert.ToDateTime(birthday);
                if (password != null)
                {
                    updateAdmin.Password = password;
                }
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Sửa admin thất bại");
            }
            return true;
        }

        public void UpdateLastLogin()
        {
            var update = (from x in _db.Admins
                          where x.AdminId == _user.ID
                          select x).Single();
            update.LastLogin = DateTime.Now;
            _db.SaveChanges();
        }

        public void UpdateLastSeen(string name, string url)
        {
            var update = (from x in _db.Admins
                          where x.AdminId == _user.ID
                          select x).Single();
            update.LastSeen = name;
            update.LastSeenUrl = url;
            _db.SaveChanges();
        }

        public List<TeacherModel> GetTeachers()
        {
            return (from x in _db.Teachers
                    join s in _db.Specialities on x.SpecialityId equals s.SpecialityId
                    select new TeacherModel { TEACHER = x, SPECIALITY = s }).ToList();
        }

        public List<Speciality> GetSpecialities()
        {
            return _db.Specialities.ToList();
        }

        
        public bool AddTeacher(string name, string username, string password, string gender, string email, string birthday,int specialityId)
        {
            var teacher = new Teacher
            {
                Name = name,
                Password = password,
                UserName = username,
                Gender = gender,
                Email = email,
                PermissionId = 2,
                SpecialityId = specialityId,
                Avatar = "avatar-default.jpg",
                Birthday = Convert.ToDateTime(birthday)
            };
            try
            {
                _db.Teachers.Add(teacher);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Thêm teacher thất bại");
            }
            return true;
        }

        public bool DeleteTeacher(int id)
        {
            var delete = (from x in _db.Teachers where x.TeacherId == id select x).Single();
            try
            {
                _db.Teachers.Remove(delete);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Xoá teacher thất bại");
            }
            return true;
        }

        public Teacher GetTeacher(int id)
        {
            var teacher = new Teacher();
            try
            {
                teacher = _db.Teachers.SingleOrDefault(x => x.TeacherId == id);
            }
            catch (Exception)
            {
                throw;
            }
            return teacher;
        }

        public bool EditTeacher(int teacherId, string name, string username, string password, string gender, string email,string birthday, int specialityId)
        {
            var update = (from x in _db.Teachers where x.TeacherId == teacherId select x).Single();
            try
            {
                update.Name = name;
                update.UserName = username;
                update.Gender = gender;
                update.Email = email;
                update.Birthday = Convert.ToDateTime(birthday);
                update.SpecialityId = specialityId;
                if (password != null)
                {
                    update.Password = password;
                }
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Sửa teacher thất bại");
            }
            return true;
        }

        public List<Class> GetClasses()
        {
            return _db.Classes.ToList();
        }

        public List<StudentModel> GetStudents()
        {
            return (from x in _db.Students
                join s in _db.Specialities on x.SpecialityId equals s.SpecialityId
                join c in _db.Classes on x.ClassId equals c.ClassId
                select new StudentModel {STUDENT = x, SPECIALITY = s, CLASS = c}).ToList();
        }

        public bool AddStudent(string name, string username, string password, string gender, string email, string birthday, int specialityId, int classId)
        {
            var student = new Student
            {
                Name = name,
                UserName = username,
                Password = password,
                Gender = gender,
                Email = email,
                PermissionId = 3,
                SpecialityId = specialityId,
                ClassId = classId,
                Avatar = "avatar-default.jpg",
                Birthday = Convert.ToDateTime(birthday)
            };
            try
            {
                _db.Students.Add(student);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Thêm student thất bại");
            }

            return true;
        }

        public bool DeleteStudent(int id)
        {
            try
            {
                var delete = (from x in _db.Students where x.StudentId == id select x).Single();
                _db.Students.Remove(delete);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Xoá student thất bại");
            }

            return true;
        }

        public Student GetStudent(int id)
        {
            var student = new Student();
            try
            {
                student = _db.Students.SingleOrDefault(x => x.StudentId == id);
            }
            catch (Exception )
            {
                throw;
            }

            return student;
        }

        public bool EditStudent(int studentId, string name, string username, string password, string gender, string email, string birthday, int specialityId, int classId)
        {
            try
            {
                var update = (from x in _db.Students where x.StudentId == studentId select x).Single();
                update.Name = name;
                update.UserName = username;
                update.Email = email;
                update.Gender = gender;
                update.SpecialityId = specialityId;
                update.ClassId = classId;
                update.Birthday = Convert.ToDateTime(birthday);
                if (password != null)
                    update.Password = password;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Sửa student thất bại");
            }

            return true;
        }

        public List<Grade> GetGrades()
        {
            return _db.Grades.ToList();
        }

        public List<ClassModel> GetClassesJoin()
        {
            return (from x in _db.Classes
                join s in _db.Specialities on x.SpecialityId equals s.SpecialityId
                join c in _db.Grades on x.GradeId equals c.GradeId
                select new ClassModel { CLASS = x, GRADE = c, SPECIALITY = s}).ToList();
        }

        public bool AddGrade(string gradeName)
        {
            var grade = new Grade
            {
                GradeName = gradeName
            };
            try
            {
                _db.Grades.Add(grade);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public bool AddClass(string className, int gradeId, int specialityId)
        {
            var aClass = new Class
            {
                ClassName = className,
                GradeId = gradeId,
                SpecialityId = specialityId
            };
            try
            {
                _db.Classes.Add(aClass);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Thêm lớp thất bại");
            }
            return true;
        }

        public bool DeleteClass(int id)
        {
            var delete = (from x in _db.Classes where x.ClassId == id select x).Single();
            try
            {
                _db.Classes.Remove(delete);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Xoá class thất bại");
            }

            return true;
        }

        public Class GetClass(int id)
        {
            var aClass = new Class();
            try
            {
                aClass = _db.Classes.SingleOrDefault(x => x.ClassId == id);
            }
            catch (Exception e)
            {
                throw;
            }

            return aClass;
        }

        public bool EditClass(int classId, string className, int specialityId, int gradeId)
        {
            try
            {
                var update = (from x in _db.Classes where x.ClassId == classId select x).Single();
                update.ClassName = className;
                update.SpecialityId = specialityId;
                update.GradeId = gradeId;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Sửa class thất bại");
            }

            return true;
        }

        public bool AddSpeciality(string specialityName)
        {
            var speciality = new Speciality
            {
                SpecialityName = specialityName
            };
            try
            {
                _db.Specialities.Add(speciality);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Thêm thất bại");
            }
            return true;
        }

        public bool DeleteSpeciality(int id)
        {
            try
            {
                var delete = (from x in _db.Specialities where x.SpecialityId == id select x).Single();
                _db.Specialities.Remove(delete);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public Speciality GetSpeciality(int id)
        {
            var speciality = new Speciality();
            try
            {
                speciality = _db.Specialities.SingleOrDefault(x => x.SpecialityId == id);
            }
            catch (Exception e)
            {
                throw;
            }

            return speciality;
        }

        public bool EditSpeciality(int specialityId, string specialityName)
        {
            try
            {
                var update = (from x in _db.Specialities where x.SpecialityId == specialityId select x).Single();
                update.SpecialityName = specialityName;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public List<Subject> GetSubjects()
        {
            return _db.Subjects.ToList();
        }

        public bool AddSubject(string subjectName)
        {
            var subject = new Subject
            {
                SubjectName = subjectName
            };
            try
            {
                _db.Subjects.Add(subject);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public bool DeleteSubject(int id)
        {
            try
            {
                var delete = (from x in _db.Subjects where x.SubjectId == id select x).Single();
                _db.Subjects.Remove(delete);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public Subject GetSubject(int id)
        {
            var subject = new Subject();
            try
            {
                subject = _db.Subjects.SingleOrDefault(x => x.SubjectId == id);
            }
            catch (Exception e)
            {
                throw;
            }

            return subject;
        }

        public bool EditSubject(int subjectId, string subjectName)
        {
            try
            {
                var update  = (from x in _db.Subjects where x.SubjectId==subjectId select x).Single();
                update.SubjectName = subjectName;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public List<QuestionModel> GetQuestions()
        {
            return (from x in _db.Questions
                join s in _db.Subjects on x.SubjectId equals s.SubjectId
                select new QuestionModel { QUESTION = x, SUBJECT = s}).ToList();
        }

        public bool AddQuestion(int subjectId, int unit, string content, string contentImg, string answerA, string answerB,
            string answerC, string answerD, string correctAnswer)
        {
            var question = new Question
            {
                SubjectId = subjectId,
                Unit = unit,
                Content = content,
                ImgContent = contentImg,
                AnswerA = answerA,
                AnswerB = answerB,
                AnswerC = answerC,
                AnswerD = answerD,
                CorrectAnswer = correctAnswer
            };
            try
            {
                _db.Questions.Add(question);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public bool DeleteQuestion(int id)
        {
            try
            {
                var delete = (from x in _db.Questions where x.QuestionId == id select x).Single();
                _db.Questions.Remove(delete);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }

            return true;
        }

        public Question GetQuestion(int id)
        {
            var question = new Question();
            try
            {
                question = _db.Questions.SingleOrDefault(x => x.QuestionId == id);
            }
            catch (Exception e)
            {
                throw;
            }

            return question;
        }

        public bool EditQuestion(int questionId, int subjectId, int unit, string content, string contentImg, string answerA,
            string answerB, string answerC, string answerD, string correctAnswer)
        {
            try
            {
                var update = (from x in _db.Questions where x.QuestionId == questionId select x).Single();
                update.SubjectId = subjectId;
                update.Unit = unit;
                update.Content = content;
                update.ImgContent = contentImg;
                update.AnswerA = answerA;
                update.AnswerB = answerB;
                update.AnswerC = answerC;
                update.AnswerD = answerD;
                update.CorrectAnswer = correctAnswer;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
            return true;
        }

        public List<TestModel> Tests()
        {
            return (from x in _db.Tests
                join s in _db.Subjects on x.SubjectId equals s.SubjectId
                join stt in _db.Statuses on x.StatusId equals stt.StatusId
                select new TestModel {SUBJECT = s, STATUS = stt, TEST = x}).ToList();
        }

        public List<UnitModel> GetUnits(int id)
        {
            return (_db.Questions
                .Where(p => p.SubjectId == id)
                .GroupBy(p => p.Unit)
                .Select(g => new UnitModel {TOTAL = g.Count(), UNIT = g.Key})).ToList();
        }

        public bool AddTest(string testName, string password, int testCode, int subjectId, int totalQuestion, int timeToDo,
            string note)
        {
            var test = new Test
            {
                TestName = testName,
                Password = password,
                TestCode = testCode,
                SubjectId = subjectId,
                QuestionsTotal = totalQuestion,
                StatusId = 1,
                TimeToDo = timeToDo,
                Note = note
            };
            try
            {
                _db.Tests.Add(test);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public Test GetTest(int testCode)
        {
            var test = new Test();
            try
            {
                test = _db.Tests.SingleOrDefault(x => x.TestCode == testCode);
            }
            catch (Exception)
            {
                throw;
            }

            return test;
        }

        public bool EditTest(int testCode, string testName, string password, int timeToDo, string note)
        {
            try
            {
                var update = (from x in _db.Tests where x.TestCode == testCode select x).Single();
                update.TestName = testName;
                update.TimeToDo = timeToDo;
                update.Note = note;
                if (!password.IsEmpty())
                    update.Password = password;
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public bool ToggleStatus(int testId)
        {
            try
            {
                var update = (from x in _db.Tests where x.TestCode == testId select x).Single();
                if (update.StatusId == 1)
                {
                    update.StatusId = 2;
                }
                else
                {
                    update.StatusId = 1;
                }

                _db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public List<Question> GetQuestionsByUnit(int subjectId, int unit, int questOfUnit)
        {
            return (from x in _db.Questions
                where x.SubjectId == subjectId && x.Unit == unit
                select x).OrderBy(x => Guid.NewGuid()).Take(questOfUnit).ToList();
        }

        public bool AddQuestionsToTest(int testCode, int questionId)
        {
            var quesOftest = new QuestOfTest
            {
                TestCode = testCode,
                QuestionId = questionId
            };
            try
            {
                _db.QuestOfTests.Add(quesOftest);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public List<Question> GetQuestionsOfTest(int testCode)
        {
            var questId = (from x in _db.QuestOfTests where x.TestCode == testCode select x.QuestionId).ToList();
            var lstQuestion = new List<Question>();
            foreach (var item in questId)
            {
                var question = (from x in _db.Questions where x.QuestionId == item select x).Single();
                lstQuestion.Add(question);
            }
            return lstQuestion;
        }
    }
}
