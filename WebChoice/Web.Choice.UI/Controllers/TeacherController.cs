using System.Collections.Generic;
using System.Web.Mvc;
using Web.Choice.Bussiness.Implementation;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Common;
using Web.Choice.UI.Models;

namespace Web.Choice.UI.Controllers
{
    public class TeacherController : Controller
    {
        private ITeacherBus _teacherBus = null;

        private ITeacherBus Teacher => _teacherBus ?? (_teacherBus = new TeacherBus());

        private readonly User _user = new User();
        // GET: Teacher
        public ActionResult Index()
        {
            if (!_user.IsTeacher())
                return View("Error");
            Teacher.UpdateLastLogin();
            Teacher.UpdateLastSeen("Trang Chủ", Url.Action("Index"));
            var getListTest = Teacher.GetListTest();
            var lstTest = new List<TestViewModel>();
            foreach (var item in getListTest)
            {
                var test = new TestViewModel
                {
                    Test = item.TEST,
                    Subject = item.SUBJECT,
                    Status = item.STATUS
                };
                lstTest.Add(test);
            }
            return View(lstTest);
        }
        public ActionResult Preview(int id)
        {
            if (!_user.IsTeacher())
                return View("Error");
            var getListScore = Teacher.GetListScore(id);
            var lstScore = new List<ScoreViewModel>();
            foreach (var item in getListScore)
            {
                var scoreNum = item.SCORE.ScoreNumber;
                var score = new ScoreViewModel
                {
                    Test = item.TEST,
                    Student = item.STUDENT,
                    Score = item.SCORE,
                };
                score.rank = scoreNum > 8 ? "Giỏi" : scoreNum < 5 ? "Yếu" : scoreNum < 7 ? "Trung Bình Khá" : "Khá";
                lstScore.Add(score);
            }
            ViewBag.testCode = id;
            ViewBag.total = lstScore.Count;
            return View(lstScore);
        }
        public ActionResult Logout()
        {
            if (!_user.IsTeacher())
                return View("Error");
            _user.Reset();
            return RedirectToAction("Index", "Login");
        }
    }
}