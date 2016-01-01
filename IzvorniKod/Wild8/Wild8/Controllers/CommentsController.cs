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

namespace Wild8.Controllers
{
    public class CommentsController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Where(c => c.Meal == null);
            return View(comments.ToList());
        }

        // POST: Comments/AddNewComment
        // TODO shity stars
        [HttpPost]
        public ActionResult AddNewComment(string Username, string Message, int? stars_existing)
        {
            
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment
                {
                    CommentDate = DateTime.Now,
                    Message = Message,
                    Username = Username,
                    Grade = 3
                };
                db.Comments.Add(newComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
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
