using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.StaticInfo;

namespace Wild8.Controllers
{
    public class CommentsController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Where(c => c.Meal == null).OrderByDescending(item => item.CommentDate);
            return View(comments.ToList());
        }


        [HttpPost]
        public ActionResult AddNewComment(string Username, string Message, int Rating)
        {
            if (ModelState.IsValid)
            {
                //Update restourant grade
                RestaurauntInfo info = RestaurauntInfo.Instance;
                var commentNumber = db.Comments.Count();
                info.RestourantGrade = (decimal)((commentNumber*info.RestourantGrade + Rating)/(commentNumber + 1));

                //Save comment
                Comment newComment = new Comment
                {
                    CommentDate = DateTime.Now,
                    Message = Message,
                    Username = Username,
                    Grade = Rating,
                };
                db.Comments.Add(newComment);
                db.SaveChanges();

                return PartialView("~/Views/Meals/Comment.cshtml", newComment);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
