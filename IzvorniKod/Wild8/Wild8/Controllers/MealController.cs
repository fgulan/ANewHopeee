using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.ModelViews;

namespace Wild8.Controllers
{
    public class MealController : Controller
    {
        private Meal meal;

        private RestaurauntContext db = new RestaurauntContext();

        public MealController(Meal meal)
        {
            Meal sessionMeal = (Meal)Session["Meal"];

            if (sessionMeal == null)
                this.meal = meal;
            else
                this.meal = sessionMeal;
        }

        public ActionResult MealView()
        {
            List<Comment> comments = new List<Comment>();
            comments.AddRange(db.Comments.Where(comment => comment.Meal.MealID == meal.MealID));

            MealModelView modelView = new MealModelView(meal, comments);

            return View(modelView);
        }

        public ActionResult AddComment(Comment comment)
        {
            Session["Meal"] = meal;
            db.Comments.Add(comment);
            return RedirectToAction("Index");
        }

        public ActionResult AddToBasket()
        {
            // TODO: SACEKAJ MICEKA DA NAPISE KOSARICU
            return null;
        }
    }
}