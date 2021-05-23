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
    [Authorize]
    public class UserController : Controller
    {
        TakaDB db = new TakaDB();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Purchased()
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            ViewBag.Addresses = db.GetAddresses(user.ID);
            ViewBag.ProcessingOrders = db.GetProcessingOrders(user.ID);
            ViewBag.DoneOrders = db.GetDoneOrders(user.ID);
            return View(user);
        }
        public ActionResult AddToCart(int idTea, int amount)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            db.AddCart(idTea, user.ID, amount);
            TempData[C.TEMPDATA.Message] = "Thêm vào giỏ hàng thành công";
            return RedirectToAction("Detail", "Home", new { id = idTea });
        }
        public ActionResult BuyNow(int idTea, int amount)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            return RedirectToAction("Payment", "User", new { idCarts = db.AddCart(idTea, user.ID, amount).ID });
        }
        public ActionResult Payment(int[] idCarts)
        {
            var listItems = db.GetBillItems(idCarts);
            USER user = (USER)Session[C.SESSION.UserInfo];
            ViewBag.addresses = db.GetAddresses(user.ID);
            return View(listItems);
        }
        [HttpPost]
        public ActionResult CheckOut(int[] id_cart, int id_address, int totalPrice, string shipper, string fullName, string phone, string address, string message)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            db.CheckOut(id_cart, id_address, totalPrice, shipper, user.ID, fullName, phone, address, message);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ShoppingCart()
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            List<CART> listCarts = db.GetListCarts(user.ID);
            return View(listCarts);
        }
        [HttpPost]
        public JsonResult ChangeAmount(int idCart, int amount)
        {
            try
            {
                db.ChangeAmount(idCart, amount);
                return Json(new { status = 1 });
            }
            catch (Exception)
            {
                return Json(new { status = 0 });
            }
        }
        public ActionResult DeleteCartItem(int idTea)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            db.DeleteCartItem(user.ID, idTea);
            return RedirectToAction("ShoppingCart", "User", new { idUser = user.ID });
        }

        public ActionResult Infor()
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            return View(user);
        }

        public ActionResult EditUser()
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(string email, string fullname, string gender, string birthday)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            Session[C.SESSION.UserInfo] = db.UpdateUser(user.PHONE, email, fullname, gender, birthday);
            return RedirectToAction("Infor", "User", new { id = user.ID });
        }

        public ActionResult AddressDetails()
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            List<ADDRESS> listadr = db.GetAddresses(user.ID);
            return View(listadr);
        }

        public ActionResult AddAddress()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAddress(string name, string phone, string address)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            db.AddAddress(user.ID, name, phone, address);
            return RedirectToAction("AddressDetails", "User");
        }

        public ActionResult EditAddress(int idAddress)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            ADDRESS adr = db.GetAddress(idAddress);
            return View(adr);
        }
        [HttpPost]
        public ActionResult EditAddress(int idAddress, string name, string phone, string address)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            db.EditAddress(idAddress, user.ID, name, phone, address);
            return RedirectToAction("AddressDetails", "User");
        }
        [HttpPost]
        public ActionResult DeleteAddress(int idAddress)
        {
            USER user = (USER)Session[C.SESSION.UserInfo];
            db.DeleteAddress(idAddress);
            return RedirectToAction("AddressDetails", "User");
        }
    }
}