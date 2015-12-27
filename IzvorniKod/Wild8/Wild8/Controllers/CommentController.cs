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

        public void SaveMealComment(string mealName, int grade, string message, string username)
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

        public List<Comment> LoadMealComments(string mealName)
        {
            List<Comment> comments;

            lock (_controller)
            {
                comments = null; // ovo će se maknuti
                // TODO: Dohvati komentare
            }

            return comments;
        }

        public List<Comment> LoadResturantComments()
        {
            List<Comment> comments;

            lock (_controller)
            {
                comments = null; // ovo će se maknuti
                // TODO: Dohvati komentare
            }

            return comments;
        }
    }
}