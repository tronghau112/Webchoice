using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services.Description;
using Web.Choice.Bussiness.Implementation;
using Web.Choice.Bussiness.Interfaces;
using Web.Choice.Common;
using Web.Choice.UI.Models;

namespace Web.Choice.UI.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        private IStudentBus _studentBus = null;

        private IStudentBus Student => _studentBus ?? (_studentBus = new StudentBus());

        private readonly User _user =new User();


        public ActionResult Index()
        {
            if (!_user.IsStudent())
            {
                return View("Error");
            }

            if (_user.IsTesting())
            {
                return RedirectToAction("DoingTest");
            }
            Student.UpdateLastLogin();
            Student.UpdateLastSeen("Trang Chủ", Url.Action("Index"));
            ViewBag.Score = Student.GetStudentTestCode();
            var getDashboard = Student.GetDashboard();
            var lstDashBoard = new List<TestViewModel>();
            foreach (var item in getDashboard)
            {
                var dashBoard = new TestViewModel
                {
                    Test = item.TEST,
                    Subject =item.SUBJECT,
                    Status = item.STATUS
                };
                lstDashBoard.Add(dashBoard);
            }
            return View(lstDashBoard);
        }

        public ActionResult TestPreview()
        {
            if (!_user.IsStudent())
            {
                return View("Error");
            }
            Student.UpdateLastLogin();
            Student.UpdateLastSeen("Trang Chủ", Url.Action("Index"));
            ViewBag.Score = Student.GetStudentTestCode();
            var getDashboard = Student.GetDashboardForMe();
            var lstDashBoard = new List<TestViewModel>();
            foreach (var item in getDashboard)
            {
                var dashBoard = new TestViewModel
                {
                    Test = item.TEST,
                    Subject = item.SUBJECT,
                    Status = item.STATUS,
                    Score = item.SCORE,
                    Student = item.STUDENT
                };
                lstDashBoard.Add(dashBoard);
            }
            return View(lstDashBoard);

        }
        public ActionResult Logout()
        {
            if (!_user.IsStudent())
                return View("Error");
            _user.Reset();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult CheckPassword(FormCollection form)
        {
            if (!_user.IsStudent())
            {
                return View("Error");
            }

            if (_user.IsTesting())
            {
                return RedirectToAction("DoingTest");
            }

            var testCode = Convert.ToInt32(form["TestCode"]);
            var password = form["Password"];
            var testPassword = Student.GetTest(testCode).Password;
            if (!password.Equals(testPassword))
            {
                TempData["StatusId"] = false;
                TempData["Status"] = "Mật khẩu không đúng!";
                return RedirectToAction("Index");
            }
            Student.CreateStudentQuestion(testCode);
            Student.UpdateStatus(testCode, Student.GetTest(testCode).TimeToDo + ":00");
            return RedirectToAction("DoingTest");
        }

        public ActionResult DoingTest()
        {
            if (!_user.IsStudent()|| !_user.IsTesting())
            {
                return View("Error");
            }

            if (_user.TIME != null)
            {
                string[] time = Regex.Split(_user.TIME, ":");
                ViewBag.min = time[0];
                ViewBag.sec = time[1];
            }
            var lstGetQuest = Student.GetListQuest(_user.TESTCODE);
            var lstQuest =new List<StudentQuestViewModel>();
            foreach (var item in lstGetQuest)
            {
                var quest = new StudentQuestViewModel
                {
                    Test = item.TEST,
                    Question = item.QUESTION,
                    StudentTestDetail = item.STUDENTTEST
                };
                lstQuest.Add(quest);
            }
            return View(lstQuest);
        }

        public ActionResult SubmitTest()
        {
            if (!_user.IsStudent() || !_user.IsTesting())
            {
                return View("Error");
            }
            var lstGetQuest = Student.GetListQuest(_user.TESTCODE);
            var lstQuest = new List<StudentQuestViewModel>();
            foreach (var item in lstGetQuest)
            {
                var quest = new StudentQuestViewModel
                {
                    Test = item.TEST,
                    Question = item.QUESTION,
                    StudentTestDetail = item.STUDENTTEST
                };
                lstQuest.Add(quest);
            }
            var totalQuest = lstQuest.First().Test.QuestionsTotal;
            var testCode = lstQuest.First().Test.TestCode;
            var pointQuestion = 10.0 / (double) totalQuest;
            var countCorrect = 0;
            foreach (var item in lstQuest)
            {
                if (item.StudentTestDetail.StudentAnswer != null && item.StudentTestDetail.StudentAnswer.Trim()
                        .Equals(item.Question.CorrectAnswer.Trim()))
                {
                    countCorrect++;
                }
            }
            var score = (pointQuestion * countCorrect).ToString();
            if (score.Length > 5)
            {
                score = score.Substring(0, 5);
            }
            var detail = countCorrect + "/" + totalQuest;
            Student.InsertScore(Convert.ToDouble(score),detail);
            Student.FinishTest();
            return RedirectToAction("PreviewTest/" + testCode);
        }
        public ActionResult PreviewTest(int id)
        {
            if (!_user.IsStudent())
            {
                return View("Error");
            }

            if (_user.IsTesting())
            {
                return RedirectToAction("DoingTest");
            }

            if (Student.GetStudentTestCode().IndexOf(id) == -1)
            {
                return View("Error");
            }
            var getScore = Student.GetScore(id);
            var scoreModel = new ScoreViewModel
            {
                TestCode = getScore.TestCode,
                ScoreId = getScore.ScoreId,
                StudentId = getScore.StudentId,
                Detail = getScore.Detail,
                ScoreNumber = getScore.ScoreNumber
            };
            ViewBag.Score = scoreModel;
            var scoreNum = scoreModel.ScoreNumber;
            ViewBag.Show = scoreNum > 8 ? "Giỏi" : scoreNum < 5 ? "Yếu" : scoreNum < 7? "Trung Bình Khá":"Khá";
            var lstGetQuest = Student.GetListQuest(id);
            var lstQuest = new List<StudentQuestViewModel>();
            foreach (var item in lstGetQuest)
            {
                var quest = new StudentQuestViewModel
                {
                    Test = item.TEST,
                    Question = item.QUESTION,
                    StudentTestDetail = item.STUDENTTEST
                };
                lstQuest.Add(quest);
            }

            return View(lstQuest);
        }

       
        [HttpPost]
        public void UpdateTiming(FormCollection form)
        {
            var min = form["min"];
            var sec = form["sec"];
            var time = min + ":" + sec;
            Student.UpdateTiming(time);
        }

        [HttpPost]
        public void UpdateStudentTest(FormCollection form)
        {
            var questId = Convert.ToInt32(form["id"]);
            var answer = form["answer"];
            answer = answer.Trim();
            var time = form["Min"] + ":" + form["Sec"];
            Student.UpdateStudentTest(questId, answer);
            Student.UpdateTiming(time);
        }
    }
}