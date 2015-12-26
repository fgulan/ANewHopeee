using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.Models;

namespace Wild8.Controllers
{
    public class MenuController : Controller
    {
        public List<MealViewModel> GetMeals(string category)
        {
            ;
        }

        public void GoToMeal(string mealName)
        {
            // TODO: pozvati kontrolerZaJelo.ucitajJelo(imeJela);
            ;
        }

        public void AddToBasket(string mealName, ICollection<MealAddOn> addOns, MealType type)
        {
            // TODO: pozvati kontrolerZaJelo.dodajUKosaricu() ??
            ;
        }
    }
}