using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.Controllers;

namespace Wild8.Models
{
    public class HomepageViewModel
    {
        // Već sam nešto napravio pa nisam htio bacit tih punih 5 minuta posla

        public List<CommentViewModel> GetResturantComments()
        {
            List<CommentViewModel> resturantCommentsView = new List<CommentViewModel>();
            List<Comment> resturantComments = CommentController.GetCommentControllerObject().LoadResturantComments();

            foreach (Comment comment in resturantComments) {
                resturantCommentsView.Add(new CommentViewModel(comment));
            }

            return resturantCommentsView;
        }
    }
}