using System;
using System.Linq;
using System.Web;
using Web.Choice.Service.Interfaces;

namespace Web.Choice.Service.Implementation
{
    public class LoginSer: ILoginSer
    {
        private readonly TestExamEntities _db = new TestExamEntities();
        public void SetAdminSession(int userId)
        {
            var user = _db.Admins.SingleOrDefault(x => x.AdminId == userId);
            HttpContext.Current.Session.Add(Common.UserSession.ISLOGIN, true);
            HttpContext.Current.Session.Add(Common.UserSession.ID, user.AdminId);
            HttpContext.Current.Session.Add(Common.UserSession.PERMISSION, user.PermissionId);
            HttpContext.Current.Session.Add(Common.UserSession.USERNAME, user.UserName);
            HttpContext.Current.Session.Add(Common.UserSession.EMAIL, user.Email);
            HttpContext.Current.Session.Add(Common.UserSession.AVATAR, user.Avatar);
            HttpContext.Current.Session.Add(Common.UserSession.NAME, user.Name);
        }

        public void SetTeacherSession(int userId)
        {
            var user = _db.Teachers.SingleOrDefault(x => x.TeacherId == userId);
            HttpContext.Current.Session.Add(Common.UserSession.ISLOGIN, true);
            HttpContext.Current.Session.Add(Common.UserSession.ID, user.TeacherId);
            HttpContext.Current.Session.Add(Common.UserSession.PERMISSION, user.PermissionId);
            HttpContext.Current.Session.Add(Common.UserSession.USERNAME, user.UserName);
            HttpContext.Current.Session.Add(Common.UserSession.EMAIL, user.Email);
            HttpContext.Current.Session.Add(Common.UserSession.AVATAR, user.Avatar);
            HttpContext.Current.Session.Add(Common.UserSession.NAME, user.Name);
        }
        public void SetStudentSession(int userId)
        {
            var user = _db.Students.SingleOrDefault(x => x.StudentId == userId);
            HttpContext.Current.Session.Add(Common.UserSession.ISLOGIN, true);
            HttpContext.Current.Session.Add(Common.UserSession.ID, user.StudentId);
            HttpContext.Current.Session.Add(Common.UserSession.PERMISSION, user.PermissionId);
            HttpContext.Current.Session.Add(Common.UserSession.USERNAME, user.UserName);
            HttpContext.Current.Session.Add(Common.UserSession.EMAIL, user.Email);
            HttpContext.Current.Session.Add(Common.UserSession.AVATAR, user.Avatar);
            HttpContext.Current.Session.Add(Common.UserSession.NAME, user.Name);
            HttpContext.Current.Session.Add(Common.UserSession.TESTCODE, user.IsTestting);
            HttpContext.Current.Session.Add(Common.UserSession.TIME, user.TimeRemaining);
        }

        public bool IsValid(string username, string password)
        {
            try
            {
                if (Convert.ToBoolean(_db.Admins.First(x => x.UserName == username && x.Password == password).AdminId))
                {
                    SetAdminSession(_db.Admins.First(x => x.UserName == username && x.Password == password).AdminId);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (Convert.ToBoolean(_db.Teachers.First(x => x.UserName == username && x.Password == password).TeacherId))
                {
                    SetTeacherSession(_db.Teachers.First(x => x.UserName == username && x.Password == password).TeacherId);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            try
            {
                if (Convert.ToBoolean(_db.Students.First(x => x.UserName == username && x.Password == password).StudentId))
                {
                    SetStudentSession(_db.Students.First(x => x.UserName == username && x.Password == password).StudentId);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
