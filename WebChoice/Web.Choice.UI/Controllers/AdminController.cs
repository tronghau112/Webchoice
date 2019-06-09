using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Web.Choice.Bussiness.Implementation;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Common;
using Web.Choice.UI.Models;

namespace Web.Choice.UI.Controllers
{
    public class AdminController : Controller
    {
        private IAdminBus _adminBus = null;

        private IAdminBus Admin => _adminBus ?? (_adminBus = new AdminBus());

        private User _user = new User();

        // GET: Admin
        public ActionResult Index()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastLogin();
            Admin.UpdateLastSeen("Trang Chủ", Url.Action("Index"));
            Dictionary<string, int> listCount = Admin.GetDashBoard();
            return View(listCount);
        }

        public ActionResult Logout()
        {
            if (!_user.IsAdmin())
                return View("Error");
            _user.Reset();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult AdminManager(AdminViewModel model)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Admin", Url.Action("AdminManager"));
            var lisAdmin = new List<AdminViewModel>();
            var admin = Admin.GetAdmins();
            foreach (var x in admin)
            {
                model = new AdminViewModel
                {
                    AdminId = x.AdminId,
                    UserName = x.UserName,
                    Password = x.Password,
                    Avatar = x.Avatar,
                    Birthday = x.Birthday,
                    Email = x.Email,
                    Gender = x.Gender,
                    LastLogin = x.LastLogin,
                    LastSeen = x.LastSeen,
                    LastSeenUrl = x.LastSeenUrl,
                    Name = x.Name,
                    Phone = x.Phone,
                    PermissionId = x.PermissionId,
                    TimeStamps = x.TimeStamps
                };
                lisAdmin.Add(model);
            }

            return View(lisAdmin);
        }

        [HttpPost]
        public ActionResult AddAdmin(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Admin", Url.Action("AddAdmin"));
            try
            {
                string name = form["Name"];

                string username = form["UserName"];

                string password = form["Password"];
                string email = form["Email"];
                string gender = form["Gender"];
                string birthday = form["Birthday"];
                var checkUserName = _adminBus.GetAdmins().Find(x => x.UserName == username).IsEmpty();
                var checkEmail = _adminBus.GetAdmins().Find(x => x.Email == email).IsEmpty();
                if (!checkUserName)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] = $"Tồn tại username {username} trong hệ thống";
                }
                if (!checkEmail)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] = $"Tồn tại email {email} trong hệ thống";
                }
                bool add = Admin.AddAdmin(name, username, password, gender, email, birthday);
                if (add)
                {
                    TempData["StatusId"] = true;
                    TempData["Status"] = $"Thêm {name} thành công";
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return RedirectToAction("AdminManager");
        }


        public ActionResult DeleteAdmin(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Admin", Url.Action("DeleteAdmin"));
            int adminId = Convert.ToInt32(id);
            bool del = Admin.DeleteAdmin(adminId);
            CheckDel(del);
            return RedirectToAction("AdminManager");
        }

        [HttpPost]
        public ActionResult DeleteAdmin(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Admin", Url.Action("DeleteAdmin"));
            string[] ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = true;
            TempData["Status"] = "Xóa Thất Bại ID: ";
            foreach (string id in ids)
            {
                int adminId = Convert.ToInt32(id);
                bool del = Admin.DeleteAdmin(adminId);
                if (!del)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += adminId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xoá thành công";
            }

            return RedirectToAction("AdminManager");
        }

        public ActionResult EditAdmin(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int adminId = Convert.ToInt32(id);

            try
            {
                var add = Admin.GetAdmin(adminId);
                var admin = new AdminViewModel
                {
                    AdminId = add.AdminId,
                    UserName = add.UserName,
                    Password = add.Password,
                    Avatar = add.Avatar,
                    Birthday = add.Birthday,
                    Email = add.Email,
                    Gender = add.Gender,
                    LastLogin = add.LastLogin,
                    LastSeen = add.LastSeen,
                    LastSeenUrl = add.LastSeenUrl,
                    Name = add.Name,
                    Phone = add.Phone,
                    PermissionId = add.PermissionId,
                    TimeStamps = add.TimeStamps
                };
                Admin.UpdateLastSeen("Sửa Admin " + add.Name, Url.Action("EditAdmin/" + id));

                return View(admin);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditAdmin(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int adminId = Convert.ToInt32(form["AdminId"]);
            string name = form["Name"];
            string username = form["Username"];
            string password = form["Password"];
            string email = form["Email"];
            string gender = form["Gender"];
            string birthday = form["Birthday"];
            bool edit = Admin.EditAdmin(adminId, name, username, password, gender, email, birthday);
            CheckEdit(edit);
            return RedirectToAction("EditAdmin/" + adminId);
        }

        public ActionResult TeacherManager(TeacherViewModel teacher)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Giáo Viên", Url.Action("TeacherManager"));
            ViewBag.ListSpecialities = GetListSpecialities();
            var lstTeacher = new List<TeacherViewModel>();
            var teachers = Admin.GetTeachers();
            foreach (var item in teachers)
            {
                teacher = new TeacherViewModel
                {
                    Teacher = item.TEACHER,
                    Speciality = item.SPECIALITY,
                    Class = item.CLASS
                };
                lstTeacher.Add(teacher);
            }

            return View(lstTeacher);
        }

        [HttpPost]
        public ActionResult AddTeacher(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Giảng Viên", Url.Action("AddTeacher"));
            string name = form["Name"];
            string username = form["Username"];
            string password = form["Password"];
            string email = form["Email"];
            string gender = form["Gender"];
            string birthday = form["Birthday"];
            int specialityId = Convert.ToInt32(form["specialityId"]);
            var checkUserName = _adminBus.GetTeachers().Find(x => x.TEACHER.UserName == username).IsEmpty();
            var checkEmail = _adminBus.GetTeachers().Find(x => x.TEACHER.Email == email).IsEmpty();
            if (!checkUserName)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Tồn tại username {username} trong hệ thống";
            }
            if (!checkEmail)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Tồn tại email {email} trong hệ thống";
            }
            bool add = Admin.AddTeacher(name, username, password, gender, email, birthday, specialityId);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm giáo viên {name} thành công";
            }
            return RedirectToAction("TeacherManager");
        }

        public ActionResult DeleteTeacher(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Giảng Viên", Url.Action("DeleteTeacher"));
            int teacherId = Convert.ToInt32(id);
            bool delete = Admin.DeleteTeacher(teacherId);
            CheckDel(delete);
            return RedirectToAction("TeacherManager");
        }

        [HttpPost]
        public ActionResult DeleteTeacher(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Giảng Viên", Url.Action("DeleteTeacher"));
            string[] ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = false;
            TempData["Status"] = "Xoá thất bại ID: ";
            foreach (var id in ids)
            {
                int teacherId = Convert.ToInt32(id);
                bool delete = Admin.DeleteAdmin(teacherId);
                if (!delete)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += teacherId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xoá thành công";
            }

            return RedirectToAction("TeacherManager");
        }

        public ActionResult EditTeacher(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int teacherId = Convert.ToInt32(id);
            try
            {
                var teacher = Admin.GetTeacher(teacherId);
                var teacherModel = new TeacherViewModel
                {
                    TeacherId = teacher.TeacherId,
                    UserName = teacher.UserName,
                    Password = teacher.Password,
                    Email = teacher.Email,
                    Name = teacher.Name,
                    Birthday = teacher.Birthday,
                    SpecialityId = teacher.SpecialityId,
                    Gender = teacher.Gender,
                    Avatar = teacher.Avatar
                };
                Admin.UpdateLastSeen("Sửa Giảng Viên " + teacher.Name, Url.Action("EditTeacher/" + id));
                ViewBag.ListSpecialities = GetListSpecialities();
                return View(teacherModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditTeacher(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int teacherId = Convert.ToInt32(form["teacherId"]);
            string name = form["Name"];
            string username = form["Username"];
            string password = form["Password"];
            string email = form["Email"];
            string gender = form["Gender"];
            string birthday = form["Birthday"];
            int specialityId = Convert.ToInt32(form["specialityId"]);
            bool edit = Admin.EditTeacher(teacherId, name, username, password, gender, email, birthday, specialityId);
            CheckEdit(edit);
            return RedirectToAction("EditTeacher/" + teacherId);
        }

        private List<SpecialityViewModel> GetListSpecialities()
        {
            var lstSpecialities = Admin.GetSpecialities();
            var lstSpeciality = new List<SpecialityViewModel>();
            foreach (var item in lstSpecialities)
            {
                var speciality = new SpecialityViewModel
                {
                    SpecialityId = item.SpecialityId,
                    SpecialityName = item.SpecialityName
                };
                lstSpeciality.Add(speciality);
            }

            return lstSpeciality;
        }

        private List<SubjectViewModel> GetListSubject()
        {
            var lstGetSubject = Admin.GetSubjects();
            var lstSubjects = new List<SubjectViewModel>();
            SubjectViewModel subject;
            foreach (var item in lstGetSubject)
            {
                subject = new SubjectViewModel
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName
                };
                lstSubjects.Add(subject);
            }

            return lstSubjects;
        }

        private List<GradeViewModel> GetListGrades()
        {
            var lstGrades = Admin.GetGrades();
            var lstGrade = new List<GradeViewModel>();
            foreach (var item in lstGrades)
            {
                var grade = new GradeViewModel
                {
                    GradeId = item.GradeId,
                    GradeName = item.GradeName
                };
                lstGrade.Add(grade);
            }

            return lstGrade;
        }

        private List<ClassViewModel> GetListClass()
        {
            var lstClasses = Admin.GetClasses();
            var lstClass = new List<ClassViewModel>();
            foreach (var item in lstClasses)
            {
                var aclass = new ClassViewModel
                {
                    ClassName = item.ClassName,
                    ClassId = item.ClassId
                };
                lstClass.Add(aclass);
            }

            return lstClass;
        }

        private void CheckEdit(bool edit)
        {
            if (edit)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = "Sửa Thành Công";
            }
            else
            {
                TempData["StatusId"] = false;
                TempData["Status"] = "Sửa Thất Bại";
            }
        }

        private void CheckDel(bool del)
        {
            if (del)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = "Xóa Thành Công";
            }
            else
            {
                TempData["StatusId"] = false;
                TempData["Status"] = "Xóa Thất Bại";
            }
        }

        public ActionResult StudentManager()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Sinh Viên", Url.Action("StudentManager"));
            ViewBag.ListSpecialities = GetListSpecialities();
            ViewBag.ListClass = GetListClass();

            StudentViewModel student;

            var lstStudent = new List<StudentViewModel>();
            var students = Admin.GetStudents();
            foreach (var item in students)
            {
                student = new StudentViewModel
                {
                    Student = item.STUDENT,
                    Class = item.CLASS,
                    Speciality = item.SPECIALITY
                };
                lstStudent.Add(student);
            }

            return View(lstStudent);
        }

        [HttpPost]
        public ActionResult AddStudent(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Sinh Viên", Url.Action("AddStudent"));
            string name = form["Name"];
            string username = form["Username"];
            string password = form["Password"];
            string email = form["Email"];
            string gender = form["Gender"];
            string birthday = form["birthday"];
            int specialityId = Convert.ToInt32(form["SpecialityId"]);
            int classId = Convert.ToInt32(form["ClassId"]);
            var checkUserName = _adminBus.GetStudents().Find(x => x.STUDENT.UserName == username).IsEmpty();
            var checkEmail = _adminBus.GetStudents().Find(x => x.STUDENT.Email == email).IsEmpty();
            if (!checkUserName)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Tồn tại username {username} trong hệ thống";
            }
            if (!checkEmail)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Tồn tại email {email} trong hệ thống";
            }
            bool add = Admin.AddStudent(name, username, password, gender, email, birthday, specialityId, classId);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm học sinh {name} thành công";
            }
           
            return RedirectToAction("StudentManager");
        }

        public ActionResult DeleteStudent(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Sinh Viên", Url.Action("DeleteStudent"));
            int studentId = Convert.ToInt32(id);
            bool del = Admin.DeleteStudent(studentId);
            CheckDel(del);
            return RedirectToAction("StudentManager");
        }

        [HttpPost]
        public ActionResult DeleteStudent(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Sinh Viên", Url.Action("DeleteStudent"));
            string[] ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = true;
            TempData["Status"] = "Xóa Thất Bại ID: ";
            foreach (string id in ids)
            {
                int studentId = Convert.ToInt32(id);
                bool del = Admin.DeleteStudent(studentId);
                if (!del)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += studentId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xóa Thành Công";
            }

            return RedirectToAction("StudentManager");
        }

        public ActionResult EditStudent(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int studentId = Convert.ToInt32(id);
            try
            {
                ViewBag.ListSpecialities = GetListSpecialities();
                ViewBag.ListClass = GetListClass();
                var student = Admin.GetStudent(studentId);
                Admin.UpdateLastSeen("Sửa Sinh Viên " + student.Name, Url.Action("EditStudent/" + id));
                var studentModel = new StudentViewModel
                {
                    StudentId = student.StudentId,
                    Name = student.Name,
                    UserName = student.UserName,
                    Gender = student.Gender,
                    Password = student.Password,
                    Email = student.Email,
                    Birthday = student.Birthday,
                    SpecialityId = student.SpecialityId,
                    ClassId = student.ClassId
                };
                return View(studentModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditStudent(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int studentId = Convert.ToInt32(form["studentId"]);
            string name = form["Name"];
            string username = form["Username"];
            string password = form["Password"];
            string email = form["Email"];
            string gender = form["Gender"];
            string birthday = form["Birthday"];
            int specialityId = Convert.ToInt32(form["SpecialityId"]);
            int classId = Convert.ToInt32(form["ClassId"]);
            bool edit = Admin.EditStudent(studentId, name, username, password, gender, email, birthday, specialityId,
                classId);
            CheckEdit(edit);
            return RedirectToAction("EditStudent/" + studentId);
        }

        public ActionResult ClassManager()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Khoá/Lớp", Url.Action("ClassManager"));
            ViewBag.ListSpecialities = GetListSpecialities();
            ViewBag.ListGrades = GetListGrades();
            var getClasses = Admin.GetClassesJoin();
            var lstClass = new List<ClassViewModel>();
            foreach (var item in getClasses)
            {
                var aClass = new ClassViewModel
                {
                    Class = item.CLASS,
                    Grade = item.GRADE,
                    Speciality = item.SPECIALITY
                };
                lstClass.Add(aClass);
            }

            return View(lstClass);
        }

        [HttpPost]
        public ActionResult AddGrade(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Khóa", Url.Action("AddGrade"));
            string gradeName = form["GradeName"];
            var checkUserName = _adminBus.GetGrades().Find(x => x.GradeName == gradeName).IsEmpty();
            if (!checkUserName)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Tồn tại khoá {gradeName} trong hệ thống";
            }
            bool add = Admin.AddGrade(gradeName);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm thành công khoá {gradeName}";
            }
            return RedirectToAction("ClassManager");
        }

        [HttpPost]
        public ActionResult AddClass(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Khóa", Url.Action("AddGrade"));
            string className = form["ClassName"];
            int specialityId = Convert.ToInt32(form["SpecialityId"]);
            int gradeId = Convert.ToInt32(form["GradeId"]);
            bool add = Admin.AddClass(className, gradeId, specialityId);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm thành công lớp  {className}";
            }
            else
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Thêm thất bại lớp {className}";
            }
           
            return RedirectToAction("ClassManager");
        }

        public ActionResult DeleteClass(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Lớp", Url.Action("DeleteClass"));
            int classId = Convert.ToInt32(id);
            bool del = Admin.DeleteClass(classId);
            CheckDel(del);
            return RedirectToAction("ClassManager");
        }

        [HttpPost]
        public ActionResult DeleteClass(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Lớp", Url.Action("DeleteClass"));
            string[] ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = true;
            TempData["Status"] = "Xóa Thất Bại ID: ";
            foreach (string id in ids)
            {
                int classId = Convert.ToInt32(id);
                bool del = Admin.DeleteClass(classId);
                if (!del)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += classId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xóa Thành Công";
            }

            return RedirectToAction("ClassManager");
        }

        public ActionResult EditClass(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int classId = Convert.ToInt32(id);
            try
            {
                var aClass = Admin.GetClass(classId);
                ViewBag.ListSpecialities = GetListSpecialities();
                ViewBag.ListGrades = GetListGrades();
                var classModel = new ClassViewModel
                {
                    ClassId = aClass.ClassId,
                    ClassName = aClass.ClassName,
                    SpecialityId = aClass.SpecialityId,
                    GradeId = aClass.GradeId
                };
                return View(classModel);
            }
            catch (Exception e)
            {
                throw new Exception("Sửa thất bại");
            }
        }

        [HttpPost]
        public ActionResult EditClass(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int classId = Convert.ToInt32(form["ClassId"]);
            string className = form["ClassName"];
            int specialityId = Convert.ToInt32(form["SpecialityId"]);
            int gradeId = Convert.ToInt32(form["GradeId"]);
            bool edit = Admin.EditClass(classId, className, specialityId, gradeId);
            CheckEdit(edit);
            return RedirectToAction("EditClass/" + classId);
        }

        public ActionResult SpecialityManager()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Ngành", Url.Action("SpecialityManager"));
            var getSpecialities = Admin.GetSpecialities();
            var lstSpeciality = new List<SpecialityViewModel>();
            SpecialityViewModel speciality;
            foreach (var item in getSpecialities)
            {
                speciality = new SpecialityViewModel
                {
                    SpecialityId = item.SpecialityId,
                    SpecialityName = item.SpecialityName
                };
                lstSpeciality.Add(speciality);
            }

            return View(lstSpeciality);
        }

        [HttpPost]
        public ActionResult AddSpeciality(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Ngành", Url.Action("AddSpeciality"));
            string specialityName = form["SpecialityName"];
            var checkSpecialityName = _adminBus.GetSpecialities().Find(x => x.SpecialityName == specialityName).IsEmpty();
            if (!checkSpecialityName)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Tồn tại khoa  {specialityName} trong hệ thống";
            }
            bool add = Admin.AddSpeciality(specialityName);
            if (add == true)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm thành công ngành {specialityName}";
            }
            return RedirectToAction("SpecialityManager");
        }

        public ActionResult DeleteSpeciality(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Ngành", Url.Action("DeleteSpeciality"));
            int specialityId = Convert.ToInt32(id);
            bool del = Admin.DeleteSpeciality(specialityId);
            CheckDel(del);
            return RedirectToAction("SpecialityManager");
        }

        [HttpPost]
        public ActionResult DeleteSpeciality(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Ngành", Url.Action("DeleteSpeciality"));
            string[] ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = true;
            TempData["Status"] = "Xóa Thất Bại ID: ";
            foreach (string id in ids)
            {
                int specialityId = Convert.ToInt32(id);
                bool del = Admin.DeleteSpeciality(specialityId);
                if (!del)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += specialityId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xóa Thành Công";
            }

            return RedirectToAction("SpecialityManager");
        }

        public ActionResult EditSpeciality(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int specialityId = Convert.ToInt32(id);
            try
            {
                var getSpeciality = Admin.GetSpeciality(specialityId);
                var specialityModel = new SpecialityViewModel
                {
                    SpecialityId = getSpeciality.SpecialityId,
                    SpecialityName = getSpeciality.SpecialityName
                };
                return View(specialityModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditSpeciality(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int specialityId = Convert.ToInt32(form["SpecialityId"]);
            string specialityName = form["SpecialityName"];
            bool edit = Admin.EditSpeciality(specialityId, specialityName);
            CheckEdit(edit);
            return RedirectToAction("EditSpeciality/" + specialityId);
        }

        public ActionResult SubjectManager()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Môn", Url.Action("SubjectManager"));
            var getListSubject = Admin.GetSubjects();
            SubjectViewModel subjectModel;
            var lstSubject = new List<SubjectViewModel>();
            foreach (var item in getListSubject)
            {
                subjectModel = new SubjectViewModel
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName
                };
                lstSubject.Add(subjectModel);
            }

            return View(lstSubject);
        }

        [HttpPost]
        public ActionResult AddSubject(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Môn", Url.Action("AddSubject"));
            string subjectName = form["SubjectName"];
            var checkSubjectName = _adminBus.GetSubjects().Find(x => x.SubjectName == subjectName).IsEmpty();
            if (!checkSubjectName)
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Môn học {subjectName} có trong hệ thống";
            }
            bool add = Admin.AddSubject(subjectName);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm môn {subjectName} thành công";
            }
            return RedirectToAction("SubjectManager");
        }

        public ActionResult DeleteSubject(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Môn", Url.Action("DeleteSubject"));
            int subjectId = Convert.ToInt32(id);
            bool del = Admin.DeleteSubject(subjectId);
            CheckDel(del);
            return RedirectToAction("SubjectManager");
        }

        [HttpPost]
        public ActionResult DeleteSubject(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Môn", Url.Action("DeleteSubject"));
            string[] ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = true;
            TempData["Status"] = "Xóa Thất Bại ID: ";
            foreach (string id in ids)
            {
                int subjectId = Convert.ToInt32(id);
                bool del = Admin.DeleteSubject(subjectId);
                if (!del)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += subjectId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xóa Thành Công";
            }

            return RedirectToAction("SubjectManager");
        }

        public ActionResult EditSubject(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int subjectId = Convert.ToInt32(id);
            try
            {
                var getSubject = Admin.GetSubject(subjectId);
                var subjectModel = new SubjectViewModel();
                subjectModel.SubjectId = getSubject.SubjectId;
                subjectModel.SubjectName = getSubject.SubjectName;
                Admin.UpdateLastSeen("Sửa Môn " + subjectModel.SubjectName, Url.Action("EditSubject/" + id));
                return View(subjectModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditSubject(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int subjectId = Convert.ToInt32(form["SubjectId"]);
            string subjectName = form["SubjectName"];
            bool edit = Admin.EditSubject(subjectId, subjectName);
            CheckEdit(edit);
            return RedirectToAction("EditSubject/" + subjectId);
        }

        public ActionResult QuestionManager()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Câu Hỏi", Url.Action("QuestionManager"));
            ViewBag.ListSubject = GetListSubject();
            var lstGetQuestion = Admin.GetQuestions();
            var lstQuestionModel = new List<QuestionViewModel>();
            foreach (var item in lstGetQuestion)
            {
                var questionModel = new QuestionViewModel
                {
                    Subject = item.SUBJECT,
                    Question = item.QUESTION
                };
                lstQuestionModel.Add(questionModel);
            }

            return View(lstQuestionModel);
        }

        public ActionResult DeleteQuestion(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Xóa Câu Hỏi", Url.Action("DeleteQuestion"));
            int questionId = Convert.ToInt32(id);
            bool del = Admin.DeleteQuestion(questionId);
            CheckDel(del);
            return RedirectToAction("QuestionManager");
        }

        [HttpPost]
        public ActionResult DeleteQuestion(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            var ids = Regex.Split(form["Checkbox"], ",");
            TempData["StatusId"] = true;
            TempData["Status"] = "Xóa Thất Bại ID: ";
            foreach (string id in ids)
            {
                var questionId = Convert.ToInt32(id);
                var del = Admin.DeleteQuestion(questionId);
                if (!del)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] += questionId.ToString() + ",";
                }
            }

            if ((bool) TempData["StatusId"])
            {
                TempData["Status"] = "Xóa Thành Công";
            }

            return RedirectToAction("QuestionManager");
        }

        public ActionResult AddQuestion(FormCollection form, HttpPostedFileBase File)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Câu Hỏi", Url.Action("AddQuestion"));
            var subjectId = Convert.ToInt32(form["SubjectId"]);
            var unit = Convert.ToInt32(form["Unit"]);
            var content = form["Content"];
            var answer = new string[]
            {
                form["AnswerA"],
                form["AnswerB"],
                form["AnswerC"],
                form["AnswerD"]
            };
            const string strD = "Cả 3 đáp án";
            const string strD1 = "Tất cả các đáp án trên";
            const string strD2 = "B và C đúng";
            const string strD3 = "A và B đều đúng";
            const string strC = "A và B đều đúng";
            if (!(form["AnswerC"] == strC || form["AnswerD"] == strD3 || form["AnswerD"] == strD2 || form["AnswerD"] == strD1 || form["AnswerD"] == strD ))
            {
                answer = ShuffleArray.Randomize(answer);
            }
            var answerA = answer[0];
            var answerB = answer[1];
            var answerC = answer[2];
            var answerD = answer[3];

            var correctAnswer = form["CorrectAnswer"];
            var imgContent = "noimage.png";
            try
            {
                var fileName = Path.GetFileName(File.FileName);
                //Upload image
                var path = Server.MapPath("~/Assets/img_questions/");
                //Đuôi hỗ trợ
                var allowedExtensions = new[] {".png", ".jpg"};
                //Lấy phần mở rộng của file
                var extensionName = Path.GetExtension(File.FileName).ToLower();
                //Kiểm tra đuôi file
                if (!allowedExtensions.Contains(extensionName))
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] = "Chỉ chọn file ảnh đuôi .PNG .png .JPG .jpg";
                    return RedirectToAction("QuestionManager");
                }
                else
                {
                    // Tạo tên file ngẫu nhiên
                    imgContent = DateTime.Now.Ticks.ToString() + extensionName;
                    // Upload file lên server
                    File.SaveAs(path + imgContent);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var checkContent = _adminBus.GetQuestions().Find(x => x.QUESTION.Content == content).IsEmpty();
            var checkAnswerA = _adminBus.GetQuestions().Find(x => (x.QUESTION.AnswerA == answerA || x.QUESTION.AnswerA == answerB || x.QUESTION.AnswerA == answerC || x.QUESTION.AnswerA == answerD)).IsEmpty();
            var checkAnswerB = _adminBus.GetQuestions().Find(x => (x.QUESTION.AnswerB == answerB || x.QUESTION.AnswerB == answerA || x.QUESTION.AnswerB == answerC || x.QUESTION.AnswerB == answerD)).IsEmpty();
            var checkAnswerC = _adminBus.GetQuestions().Find(x => (x.QUESTION.AnswerC == answerC || x.QUESTION.AnswerC == answerA || x.QUESTION.AnswerC == answerB || x.QUESTION.AnswerC == answerD)).IsEmpty();
            var checkAnswerD = _adminBus.GetQuestions().Find(x => (x.QUESTION.AnswerD == answerD || x.QUESTION.AnswerD == answerA || x.QUESTION.AnswerD == answerB || x.QUESTION.AnswerD == answerC)).IsEmpty();
            if (!checkContent)
            {
                if (!checkAnswerA && !checkAnswerB && !checkAnswerC && !checkAnswerD)
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] = $"Thêm thất bại câu hỏi";
                    return RedirectToAction("QuestionManager");
                }
            }
            var add = Admin.AddQuestion(subjectId, unit, content, imgContent, answerA, answerB, answerC, answerD,
                correctAnswer);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = $"Thêm thành công câu hỏi";
            }
            else
            {
                TempData["StatusId"] = false;
                TempData["Status"] = $"Thêm thất bại câu hỏi";
            }
            return RedirectToAction("QuestionManager");
        }

        public ActionResult EditQuestion(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            var questionId = Convert.ToInt32(id);
            try
            {
                var getQuestion = Admin.GetQuestion(questionId);
                Admin.UpdateLastSeen("Sửa Câu Hỏi " + getQuestion.QuestionId, Url.Action("EditQuestion/" + id));
                var question = new QuestionViewModel
                {
                    SubjectId = getQuestion.SubjectId,
                    AnswerA = getQuestion.AnswerA,
                    AnswerB = getQuestion.AnswerB,
                    AnswerC = getQuestion.AnswerC,
                    AnswerD = getQuestion.AnswerD,
                    CorrectAnswer = getQuestion.CorrectAnswer,
                    QuestionId = getQuestion.QuestionId,
                    Unit = getQuestion.Unit,
                    Content = getQuestion.Content,
                    ImgContent = getQuestion.ImgContent
                };
                ViewBag.ListSubject = GetListSubject();
                return View(question);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult EditQuestion(FormCollection form, HttpPostedFileBase File)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int subjectId = Convert.ToInt32(form["SubjectId"]);
            int questionId = Convert.ToInt32(form["QuestionId"]);
            int unit = Convert.ToInt32(form["Unit"]);
            string content = form["Content"];
            string[] answer = new string[]
            {
                form["AnswerA"],
                form["AnswerB"],
                form["AnswerC"],
                form["AnswerD"]
            };
            string answerA = answer[0];
            string answerB = answer[1];
            string answerC = answer[2];
            string answerD = answer[3];
            string correctAnswer = form["CorrectAnswer"];
            string imgContent = "noimage.png";
            try
            {
                string fileName = Path.GetFileName(File.FileName);
                //Upload image
                string path = Server.MapPath("~/Assets/img_questions/");
                //Đuôi hỗ trợ
                var allowedExtensions = new[] { ".png", ".jpg" };
                //Lấy phần mở rộng của file
                string extensionName = Path.GetExtension(File.FileName).ToLower();
                //Kiểm tra đuôi file
                if (!allowedExtensions.Contains(extensionName))
                {
                    TempData["StatusId"] = false;
                    TempData["Status"] = "Chỉ chọn file ảnh đuôi .PNG .png .JPG .jpg";
                    return RedirectToAction("QuestionManager");
                }
                else
                {
                    // Tạo tên file ngẫu nhiên
                    imgContent = DateTime.Now.Ticks.ToString() + extensionName;
                    // Upload file lên server
                    File.SaveAs(path + imgContent);
                }
            }
            catch (Exception)
            {
            }

            bool edit = Admin.EditQuestion(questionId, subjectId, unit, content, imgContent, answerA, answerB, answerC,
                answerD, correctAnswer);
            CheckEdit(edit);
            return RedirectToAction("EditQuestion/" + questionId);
        }

        public ActionResult TestManager()
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Quản Lý Bài Thi", Url.Action("TestManager"));
            ViewBag.ListSubject = GetListSubject();
            var getTests = Admin.Tests();
            var lstTestModel = new List<TestViewModel>();
            foreach (var item in getTests)
            {
                var testModel = new TestViewModel
                {
                    Subject = item.SUBJECT,
                    Status = item.STATUS,
                    Test = item.TEST
                };
                lstTestModel.Add(testModel);
            }

            return View(lstTestModel);
        }

        public JsonResult GetJsonUnits(int id)
        {
            return Json(Admin.GetUnits(id),JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTest(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            Admin.UpdateLastSeen("Thêm Đề Thi", Url.Action("AddTest"));
            string testName = form["TestName"];
            string password = form["Password"];
            var random = new Random();
            int testCode = random.Next(1, 9999);
            int subjectId = Convert.ToInt32(form["SubjectId"]);
            int totalQuestion = Convert.ToInt32(form["TotalQuestion"]);
            int timeToDo = Convert.ToInt32(form["TimeToDo"]);
            string note = "";
            if (form["Note"] != "")
                note = form["Note"];
            bool add = Admin.AddTest(testName, password, testCode, subjectId, totalQuestion, timeToDo, note);
            if (add)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = "Thêm Thành Công";
                //tạo bộ câu hỏi cho đề thi
                var getUnits = Admin.GetUnits(subjectId);

                var lstUnitModel = new List<UnitViewModel>();
                foreach (var item in getUnits)
                {
                    var unitModel = new UnitViewModel
                    {
                        Unit = item.UNIT,
                        Total = item.TOTAL
                    };
                    lstUnitModel.Add(unitModel);
                }
                foreach (var item in lstUnitModel)
                {
                    var questOfUnit = Convert.ToInt32(form["Unit-" + item.Unit]);
                    var getQuestionByUnit = Admin.GetQuestionsByUnit(subjectId, item.Unit, questOfUnit);
                    var lstQuestion = new List<QuestionViewModel>();
                    foreach (var m in getQuestionByUnit)
                    {
                        var question = new QuestionViewModel
                        {
                            QuestionId = m.QuestionId
                        };
                        lstQuestion.Add(question);
                    }

                    foreach (var id in lstQuestion)
                    {
                        Admin.AddQuestionsToTest(testCode, id.QuestionId);
                    }
                }
            }
            else
            {
                TempData["StatusId"] = false;
                TempData["Status"] = "Thêm Thất Bại";
            }
            return RedirectToAction("TestManager");
        }

        public ActionResult EditTest(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int testCode = Convert.ToInt32(id);
            try
            {
                var getTest = Admin.GetTest(testCode);
                Admin.UpdateLastSeen("Sửa Đề Thi " + getTest.TestCode, Url.Action("EditTest/" + id));
                var testModel = new TestViewModel
                {
                    SubjectId = getTest.SubjectId,
                    TestCode = getTest.TestCode,
                    TestName = getTest.TestName,
                    Password = getTest.Password,
                    TimeToDo =getTest.TimeToDo,
                    Note = getTest.Note
                };
                return View(testModel);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult EditTest(FormCollection form)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int testCode = Convert.ToInt32(form["TestCode"]);
            string testName = form["TestName"];
            string password = "";
            if (form["Password"] != "")
                password = form["Password"];
            int subjectId = Convert.ToInt32(form["SubjectId"]);
            int timeToDo = Convert.ToInt32(form["TimeToDo"]);
            string note = "";
            if (form["Note"] != "")
                note = form["Note"];
            bool edit = Admin.EditTest(testCode, testName, password, timeToDo, note);
            CheckEdit(edit);
            return RedirectToAction("EditTest/" + testCode);
        }
        public ActionResult ToggleStatus(int id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int testId = Convert.ToInt32(id);
            bool toggle = Admin.ToggleStatus(testId);
            if (toggle)
            {
                TempData["StatusId"] = true;
                TempData["Status"] = "Đã Thay Đổi Trạng Thái Đề Thi " + testId;
            }
            return RedirectToAction("TestManager/" + testId);
        }

        public ActionResult TestDetail(string id)
        {
            if (!_user.IsAdmin())
                return View("Error");
            int testCode = Convert.ToInt32(id);
            try
            {
                Admin.UpdateLastSeen("Chi Tiết Đề Thi " + testCode, Url.Action("TestDetail/" + testCode));
                ViewBag.testCode = testCode;
                var getQuestionOfTest = Admin.GetQuestionsOfTest(testCode);
                var lstQuestion = new List<QuestionViewModel>();
                foreach (var item in getQuestionOfTest)
                {
                    var question = new QuestionViewModel
                    {
                        Content = item.Content,
                        AnswerA = item.AnswerA,
                        AnswerB = item.AnswerB,
                        AnswerC = item.AnswerC,
                        AnswerD = item.AnswerD,
                        ImgContent = item.ImgContent,
                        CorrectAnswer = item.CorrectAnswer,
                        SubjectId = item.SubjectId,
                        QuestionId = item.QuestionId,
                        Unit = item.Unit
                    };
                    lstQuestion.Add(question);
                }
                return View(lstQuestion);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}