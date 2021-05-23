using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using taka.Models.DatabaseInteractive;
using taka.Models.Enitities;
using taka.Utils;

namespace taka.Controllers
{
    //[Authorize(Users = "admin")]
    [Authorize(Users = "1")]
    public class AdminController : Controller
    {

        TakaDB dB = new TakaDB();
        // GET: Admin
        public ActionResult Tea(int page = 1, string text = "", int cate = 0, int sort = 0, int pageSize = 16, int priceFrom = 0, int priceTo = 0)
        {
            ViewBag.ListCate = dB.GetCategories();
            ViewBag.Cate = cate;
            ViewBag.Sort = sort;
            if (priceFrom > priceTo)
                priceTo = 0;
            ViewBag.PriceFrom = priceFrom;
            ViewBag.PriceTo = priceTo;
            ViewBag.PageSize = 16;
            ViewBag.CurrentPage = page;
            switch (sort)
            {
                case 0:
                    ViewBag.TextSort = C.DROPDOWN_SORT.NEWEST;
                    break;
                case 1:
                    ViewBag.TextSort = C.DROPDOWN_SORT.OLDEST;
                    break;
                case 2:
                    ViewBag.TextSort = C.DROPDOWN_SORT.LOWEST_PRICE;
                    break;
                case 3:
                    ViewBag.TextSort = C.DROPDOWN_SORT.HIGHEST_PRICE;
                    break;
            }

            if (pageSize != 16 && pageSize % 16 == 0 && pageSize <= 64)
            {
                ViewBag.PageSize = pageSize;
            }
            ListTea listTea = dB.GetListTea(page, text, cate, sort, pageSize, priceFrom, priceTo);
            ViewBag.ListPage = HelperFunctions.getNumPage(page, listTea.pages);
            ViewBag.maxPage = listTea.pages;
            ViewBag.TextSearch = text;
            ViewBag.list = listTea.TEAs;
            return View();
        }


        public ActionResult Order()
        {
            return View();
        }

        public ActionResult User()
        {
            ViewBag.list = dB.GetUsers();
            return View();
        }
        public ActionResult Category()
        {
            ViewBag.list = dB.GetCategories();
            return View();
        }
        public ActionResult Edit(int id = -1)
        {
            ViewBag.listCategories = dB.GetCategories();
            try
            {
                if (id == -1)
                    throw new Exception("Not found");
                var item = dB.GetTeaDetail(id);
                return View(item);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public ActionResult EditTea(int ID,
             IEnumerable<HttpPostedFileBase> Images,
             IEnumerable<int> images_delete,
            string Title,
            int Price,
            int idCategory,
            int Amount,
            string Description,
            string Story,
            string Ingredient,
            string Function,
            string Caffein,
            string Weight,
            string Use)
        {
            dB.EditTea(ID, images_delete, Images, Title, Price, idCategory, Amount, Description, Story, Ingredient, Function, Caffein, Weight, Use);
            return RedirectToAction("Edit", "Admin", new { id = ID });
        }

        [HttpPost]
        public ActionResult Delete(int id = -1)
        {
            try
            {
                if (id == -1)
                    throw new Exception("Not found");
                dB.DeleteTea(id, false);
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Tea", "Admin");
        }


        [HttpPost]
        public ActionResult AddTea(
            IEnumerable<HttpPostedFileBase> Images,
            string Title,
            int Price,
            int idCategory,
            int Amount,
            string Description,
            string Story,
            string Ingredient,
            string Function,
            string Caffein,
            string Weight,
            string Use)
        {
            TEA tea = dB.AddTea(Images, Title, Price, idCategory, Amount, Description, Story, Ingredient, Function, Caffein, Weight, Use);
            return RedirectToAction("Detail", "Home", new { id = tea.ID });
        }

        public ActionResult Add()
        {
            ViewBag.listCategories = dB.GetCategories();
            
            return View();
        }

        public ActionResult BanUser(int id, int ban = 0)
        {
            dB.BanUser(id, ban);
            return RedirectToAction("User", "Admin");
        }

        public ActionResult UpdateUser(string phone, string email, string fullname, string gender, string birthday)
        {
            dB.UpdateUser(phone, email, fullname, gender, birthday);
            return RedirectToAction("User", "Admin");
        }

        public ActionResult UpdateCategory(int id, string name)
        {
            dB.UpdateCategory(id, name);
            return RedirectToAction("Category", "Admin");
        }

        public ActionResult AddCategory(string name)
        {
            dB.AddCategory(name);
            return RedirectToAction("Category", "Admin");
        }

        public ActionResult RemoveCategory(int id)
        {
            dB.RemoveCategory(id);
            return RedirectToAction("Category", "Admin");
        }

        


    }
}