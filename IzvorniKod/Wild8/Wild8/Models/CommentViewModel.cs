using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class CommentViewModel
    {
        private string username { get; set; }

        private string message { get; set; }

        private int grade { get; set; }

        public CommentViewModel(Comment comment)
        {
            // TODO: popraviti sa podacima iz ISPRAVNE baze
            this.username = comment.User.userName;
            this.message = comment.comment1;
            this.grade = comment.Id;
        }
    }
}