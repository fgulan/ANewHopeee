using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.Models;

namespace Wild8.Controllers
{
    public class CommentController : Controller
    {
        private static CommentController _controller;

        private CommentController() {}

        public static CommentController GetCommentControllerObject()
        {
            if(_controller == null)
            {
                _controller = new CommentController();
            }

            return _controller;
        }

        public void SaveMealComment(int mealId, int grade, string message, string username)
        {
            lock (_controller)
            {
                // TODO: dohvati id od jela
                // Spremi komentar u bazu
            }
        }

        public void SaveResturantComment(int grade, string message, string username)
        {
            lock (_controller)
            {
                // TODO: Spremi komentar u bazu
            }
        }

        public List<Comment> LoadMealComments(int mealId)
        {
            List<Comment> comments;

            lock (_controller)
            {
                comments = GetCommentsFor(mealId);
            }

            return comments;
        }

        public List<Comment> LoadResturantComments()
        {
            List<Comment> comments;

            lock (_controller)
            {
                comments = GetCommentsFor(null);
            }

            return comments;
        }

        private List<Comment> GetCommentsFor(int? mealId)
        {
            List<Comment> comments = null;

            using (var db = new Wild8DBEntities1())
            {
                var query = (from c in db.Comments
                             where c.mealId == mealId
                             select c);
                comments = query.ToList();
            }

            return comments;
        }
    }
}