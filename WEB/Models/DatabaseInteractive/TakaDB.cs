using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using taka.Models.Enitities;
using taka.Utils;

namespace taka.Models.DatabaseInteractive
{
    public class BillItem
    {
        public int id { get; set; }
        public string teaName { get; set; }
        public int price { get; set; }
        public int amount { get; set; }
    }
    public class ListTea
    {
        public int pages { get; set; }
        public CATEGORY cate { get; set; }

        public List<TEA> TEAs { get; set; }

        public ListTea(int pages, List<TEA> TEAs)
        {
            this.pages = pages;
            this.TEAs = TEAs;
        }
    }
    public class TakaDB
    {
        TakaDBContext takaDB;

        public TakaDB()
        {
            takaDB = new TakaDBContext();
        }

        public ListTea GetListTea(int page = 1, string text = "", int cate = 0, int sort = 0, int pageSize = 12, int priceFrom = 0, int priceTo = 0)
        {
            var removeUnicode = HelperFunctions.RemoveUnicode(text);
            var listItem = takaDB.TEAs.Where(x => x.isHIDDEN != 1);
            listItem = listItem.Where(m => m.KEYSEARCH.Contains(removeUnicode));

            if (cate != 0)
                listItem = listItem.Where(m => m.ID_CATEGORY == cate);

            if (priceTo > 0)
                listItem = listItem.Where(m => (m.PRICE > priceFrom && m.PRICE < priceTo));
            else if (priceFrom > 0)
                listItem = listItem.Where(m => m.PRICE > priceFrom);

            switch (sort)
            {
                case 0:
                    listItem = listItem.OrderBy(m => m.ID);
                    break;
                case 1:
                    listItem = listItem.OrderByDescending(m => m.AMOUNT);
                    break;
                case 2:
                    listItem = listItem.OrderBy(m => m.AMOUNT);
                    break;
                case 3:
                    listItem = listItem.OrderBy(m => m.PRICE);
                    break;
                case 4:
                    listItem = listItem.OrderByDescending(m => m.PRICE);
                    break;
            }

            int maxPage = listItem.Count() / pageSize + 1;
            return new ListTea(maxPage, listItem.Skip((page - 1) * pageSize).Take(pageSize).ToList());
        }

        public List<ListTea> GetHomePage()
        {
            int pageSize = 10;
            var listItem = takaDB.TEAs.Where(x => x.isHIDDEN != 1);
            List<ListTea> list = new List<ListTea>();
            foreach (var cate in GetCategories())
            {
                ListTea listTea = new ListTea(0, listItem.Where(m => m.ID_CATEGORY == cate.ID).OrderBy(m => m.ID).Take(pageSize).ToList());
                listTea.cate = cate;
                list.Add(listTea);
            }
            return list;
        }


        public List<CATEGORY> GetCategories()
        {
            return takaDB.CATEGORies.ToList();
        }

        public List<USER> GetUsers()
        {
            return takaDB.USERs.Where(x => !x.ID.Equals(C.ID_ADMIN)).ToList();
        }

        public string findTextCategory(int id)
        {
            return takaDB.CATEGORies.Where(x => x.ID == id).First().NAME;
        }

        public TEA GetTeaDetail(int id)
        {
            return takaDB.TEAs.Where(x => x.ID == id && x.isHIDDEN != 1).First();
        }

        public USER Register(string phone, string password, string email = "", string gender = "", string fullname = "", string birthday = "")
        {
            if (takaDB.USERs.Where(x => x.PHONE == phone).Count() > 0)
                return null;
            USER user = new USER();
            user.PHONE = phone.Replace("+84", "0");
            user.PASSWORD = HelperFunctions.sha256(password);
            user.EMAIL = email;
            user.FULLNAME = fullname;
            user.GENDER = gender;
            user.BIRTHDAY = birthday.Length == 0 ? DateTime.Now.ToShortDateString() : birthday;
            takaDB.USERs.Add(user);
            takaDB.SaveChanges();
            return user;
        }

        public USER GetUserDetail(int id)
        {
            var user = takaDB.USERs.Where(x => x.ID == id).First();
            return user;
        }

        public USER UpdateUser(string phone, string email, string fullname, string gender, string birthday)
        {
            USER user = takaDB.USERs.Where(x => x.PHONE == phone).First();
            if (user == null)
                return null;
            user.EMAIL = email;
            user.FULLNAME = fullname;
            user.GENDER = gender;
            user.BIRTHDAY = birthday.Length == 0 ? DateTime.Now.ToShortDateString() : birthday;
            takaDB.SaveChanges();
            return user;
        }
        public void BanUser(int ID, int ban = 0)
        {
            USER user = takaDB.USERs.Where(x => x.ID == ID).First();
            if (user == null)
                return;
            user.isBAN = ban;
            takaDB.SaveChanges();
        }

        public void UpdateCategory(int id, string name)
        {
            CATEGORY cate = takaDB.CATEGORies.Where(x => x.ID == id).First();
            if (cate == null)
                return;
            cate.NAME = name;
            takaDB.SaveChanges();
        }

        public void AddCategory(string name)
        {
            if (takaDB.CATEGORies.Where(e => e.NAME.Equals(name)).Count() > 0)
                return;
            CATEGORY cate = new CATEGORY();
            cate.NAME = name;
            takaDB.CATEGORies.Add(cate);
            takaDB.SaveChanges();
        }

        public void RemoveCategory(int id)
        {
            try
            {
                CATEGORY cate = takaDB.CATEGORies.Where(x => x.ID == id).First();
                takaDB.CATEGORies.Remove(cate);
                takaDB.SaveChanges();
            }
            catch (Exception)
            {

            }
        }



        public USER Login(string phone, string password)
        {
            string hashpass = HelperFunctions.sha256(password);
            var user = takaDB.USERs.Where(x => x.PHONE == phone && x.PASSWORD == hashpass);
            if (user.Count() > 0)
                return user.First();
            return null;
        }
        public USER LoginWithGoogle(string gooogleId, string fullname, string email)
        {
            var user = takaDB.USERs.Where(x => x.GOOGLE_ID == gooogleId);
            if (user.Count() > 0)
                return user.First();
            USER newUser = new USER();
            newUser.GOOGLE_ID = gooogleId;
            newUser.FULLNAME = fullname;
            newUser.EMAIL = email;
            takaDB.USERs.Add(newUser);
            takaDB.SaveChanges();
            return newUser;
        }

        public CART AddCart(int idTea, int idUser, int amount = 1)
        {
            CART cart;
            var find_cart = takaDB.CARTs.Where(x => x.ID_TEA == idTea && x.ID_USER == idUser);
            if (find_cart.Count() > 0)
            {
                find_cart.First().AMOUNT += amount;
                cart = find_cart.First();
            }
            else
            {
                cart = new CART();
                cart.ID_TEA = idTea;
                cart.ID_USER = idUser;
                cart.AMOUNT = amount;
                takaDB.CARTs.Add(cart);
            }
            takaDB.SaveChanges();
            return cart;
        }
        public List<CART> GetListCarts(int idUser)
        {
            List<CART> listCarts = new List<CART>();
            listCarts = takaDB.CARTs.Where(x => x.ID_USER == idUser).ToList();
            return listCarts;
        }
        public void DeleteCartItem(int idUser, int idTea)
        {
            CART deleteItem = takaDB.CARTs.Where(x => x.ID_USER == idUser && x.ID_TEA == idTea).First();
            takaDB.CARTs.Remove(deleteItem);
            takaDB.SaveChanges();
        }
        public bool DeleteTea(int id, bool permanently = false)
        {
            var item = takaDB.TEAs.Where(x => x.ID == id).First();
            if (item != null)
                if (!permanently)
                {
                    item.isHIDDEN = 1;
                }
                else
                {
                    takaDB.CARTs.RemoveRange(takaDB.CARTs.Where(x => x.ID_TEA == id));
                    takaDB.IMAGEs.RemoveRange(takaDB.IMAGEs.Where(x => x.ID_TEA == id));
                    takaDB.RATEs.RemoveRange(takaDB.RATEs.Where(x => x.ID_TEA == id));
                    takaDB.ORDER_DETAIL.RemoveRange(takaDB.ORDER_DETAIL.Where(x => x.ID_TEA == id));
                    takaDB.TEAs.Remove(item);
                }
            takaDB.SaveChanges();
            return true;
        }


        public TEA AddTea(IEnumerable<HttpPostedFileBase> Images, string Title,
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
            TEA tea = new TEA();
            tea.TITLE = Title;
            tea.PRICE = Price;
            tea.ID_CATEGORY = idCategory;
            tea.AMOUNT = Amount;
            tea.DESCRIPTION = Description;
            tea.STORY = Story;
            tea.INGREDIENT = Ingredient;
            tea.FUNCTION = Function;
            tea.CAFFEIN = Caffein;
            tea.WEIGHT = Weight;
            tea.USE = Use;
            tea.RATECOUNT = 0;
            tea.RATEPOINT = 0;
            takaDB.TEAs.Add(tea);
            takaDB.SaveChanges();
            if (Images != null && Images.Count() > 0)
            {
                foreach (var image in Images)
                {
                    try
                    {
                        MemoryStream target = new MemoryStream();
                        image.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        var client = new RestClient("http://128.199.108.177:8001/upload_image");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "multipart/form-data");
                        request.AlwaysMultipartFormData = true;
                        request.AddFile("book_cover", data, "image.jpeg");
                        IRestResponse response = client.Execute(request);
                        string resJsonRaw = response.Content;
                        JObject json = JObject.Parse(resJsonRaw);
                        IMAGE imgObj = new IMAGE();
                        imgObj.ID_TEA = tea.ID;
                        imgObj.URL = json.GetValue("url").ToString();
                        takaDB.IMAGEs.Add(imgObj);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            takaDB.SaveChanges();
            return tea;
        }


        public TEA EditTea(int ID,
            IEnumerable<int> images_delete,
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
            if (images_delete != null)
                takaDB.IMAGEs.RemoveRange(takaDB.IMAGEs.Where(x => images_delete.Contains(x.ID)));
            TEA tea = takaDB.TEAs.Where(x => x.ID == ID).First();
            tea.TITLE = Title;
            tea.PRICE = Price;
            tea.ID_CATEGORY = idCategory;
            tea.AMOUNT = Amount;
            tea.DESCRIPTION = Description;
            tea.STORY = Story;
            tea.INGREDIENT = Ingredient;
            tea.FUNCTION = Function;
            tea.CAFFEIN = Caffein;
            tea.WEIGHT = Weight;
            tea.USE = Use;
            takaDB.SaveChanges();
            if (Images != null && Images.Count() > 0)
            {
                foreach (var image in Images)
                {
                    try
                    {
                        if (image != null)
                        {
                            MemoryStream target = new MemoryStream();
                            image.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();
                            var client = new RestClient("http://128.199.108.177:8001/upload_image");
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "multipart/form-data");
                            request.AlwaysMultipartFormData = true;
                            request.AddFile("book_cover", data, "image.jpeg");
                            IRestResponse response = client.Execute(request);
                            string resJsonRaw = response.Content;
                            JObject json = JObject.Parse(resJsonRaw);
                            IMAGE imgObj = new IMAGE();
                            imgObj.ID_TEA = ID;
                            imgObj.URL = json.GetValue("url").ToString();
                            takaDB.IMAGEs.Add(imgObj);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            takaDB.SaveChanges();
            return tea;
        }

        public void ChangeAmount(int idCart, int amount)
        {
            takaDB.CARTs.Where(x => x.ID == idCart).First().AMOUNT = amount;
            takaDB.SaveChanges();
        }

        public void ChangeImageOrder(int oldOrder, int newOrder, int id)
        {
            var img = takaDB.IMAGEs.Where(x => x.ID_TEA == id);
            int count = newOrder - oldOrder;

            if (count > 0)
            {
                for (int i = newOrder; i > oldOrder; i--)
                {
                    img.Where(x => x.ORDER == i).First().ORDER = img.Where(x => x.ORDER == i - 1).First().ORDER;
                }
                img.Where(x => x.ORDER == oldOrder).First().ORDER = newOrder;
            }
            else if (count < 0)
            {
                count = 0 - count;
                for (int i = newOrder; i < oldOrder; i++)
                {
                    img.Where(x => x.ORDER == i).First().ORDER = img.Where(x => x.ORDER == i + 1).First().ORDER;
                }
                img.Where(x => x.ORDER == oldOrder).First().ORDER = newOrder;
            }

            takaDB.SaveChanges();
        }

        public List<BillItem> GetBillItems(int[] ids)
        {

            List<BillItem> billItems = new List<BillItem>();


            foreach (var id in ids)
            {

                BillItem billItem = new BillItem();
                CART cart = takaDB.CARTs.Where(x => x.ID == id).ToList().First();
                billItem.id = cart.ID;
                billItem.price = (int)cart.TEA.PRICE;
                billItem.amount = (int)cart.AMOUNT;
                billItem.teaName = cart.TEA.TITLE;
                billItems.Add(billItem);
            }


            return billItems;
        }

        public List<ADDRESS> GetAddressByUser(int idUser)
        {
            List<ADDRESS> addresses = new List<ADDRESS>();
            addresses = takaDB.ADDRESSes.Where(x => x.ID_USER == idUser).ToList();
            return addresses;
        }

        public ADDRESS GetAddressByIdAddress(int? idAddress)
        {
            return takaDB.ADDRESSes.Where(x => x.ID == idAddress).First();
        }

        public void AddAddress(int idUser, string name, string phone, string address)
        {
            ADDRESS adr = new ADDRESS();
            adr.ID_USER = idUser;
            adr.NAME = name;
            adr.PHONE = phone;
            adr.ADDRESS1 = address;
            takaDB.ADDRESSes.Add(adr);
            takaDB.SaveChanges();
        }

        public void EditAddress(int idAddress, int idUser, string name, string phone, string address)
        {
            ADDRESS adr = takaDB.ADDRESSes.Where(x => x.ID == idAddress).First();
            if (adr == null)
                return;
            adr.NAME = name;
            adr.PHONE = phone;
            adr.ADDRESS1 = address;
            adr.ID_USER = idUser;
            takaDB.SaveChanges();
        }

        public void DeleteAddress(int idAddress)
        {
            try
            {
                ADDRESS address = takaDB.ADDRESSes.Where(x => x.ID == idAddress).First();
                takaDB.ADDRESSes.Remove(address);
                takaDB.SaveChanges();
            }
            catch (Exception)
            {

            }
        }

        public void ChangePassword(string newPass, int idUser)
        {
            USER user = takaDB.USERs.Where(x => x.ID == idUser).First();
            user.PASSWORD = HelperFunctions.sha256(newPass);
            takaDB.SaveChanges();
        }

        public void CheckOut(int[] id_cart, int id_address, int totalPrice, string shipper, int idUser, string fullName, string phone, string address, string message)
        {

            ORDER order = new ORDER();
            order.CREATE_DATE = DateTime.Now;
            order.ID_USER = idUser;
            if (id_address != -1)
            {
                string clientName = takaDB.ADDRESSes.Where(x => x.ID == id_address).First().NAME;

                order.ID_ADDRESS = id_address;
                order.CLIENT_NAME = clientName;
            }
            else
            {
                ADDRESS newAdress = new ADDRESS();
                newAdress.ID_USER = idUser;
                newAdress.NAME = fullName;
                newAdress.PHONE = phone;
                newAdress.ADDRESS1 = address;
                takaDB.ADDRESSes.Add(newAdress);
                takaDB.SaveChanges();
                order.ID_ADDRESS = takaDB.ADDRESSes.Where(x => x.ADDRESS1 == address).First().ID;
                order.CLIENT_NAME = fullName;
            }
            order.TOTAL_PRICE = totalPrice;
            order.ORDER_STATUS = 0;
            order.SHIPPER = shipper;
            order.NOTES = message;
            takaDB.ORDERs.Add(order);
            takaDB.SaveChanges();
            foreach (var idItem in id_cart)
            {
                ORDER_DETAIL orderDetail = new ORDER_DETAIL();
                CART cartItem = takaDB.CARTs.Where(x => x.ID == idItem).First();
                orderDetail.ID_ORDER = takaDB.ORDERs.OrderByDescending(x => x.ID).First().ID;
                orderDetail.ID_TEA = (int)cartItem.ID_TEA;
                orderDetail.AMOUNT = (int)cartItem.AMOUNT;
                takaDB.ORDER_DETAIL.Add(orderDetail);
                takaDB.CARTs.Remove(cartItem);
            }
            takaDB.SaveChanges();
        }

        public List<ORDER> GetProcessingOrders(int id = -1)
        {
            List<ORDER> orders = id.Equals(-1) ?
                takaDB.ORDERs.Where(x => x.ORDER_STATUS == 0).ToList()
                : takaDB.ORDERs.Where(x => x.ID_USER == id && x.ORDER_STATUS == 0).ToList();
            orders = orders.OrderByDescending(x => x.CREATE_DATE).ToList();
            return orders;
        }

        public List<ORDER> GetDoneOrders(int id = -1)
        {
            List<ORDER> orders = id.Equals(-1) ?
                takaDB.ORDERs.Where(x => x.ORDER_STATUS == 1).ToList()
                : takaDB.ORDERs.Where(x => x.ID_USER == id && x.ORDER_STATUS == 1).ToList();
            orders = orders.OrderByDescending(x => x.CREATE_DATE).ToList();
            return orders;
        }

        public void ConfirmOrder(int id)
        {
            ORDER order = takaDB.ORDERs.Where(x => x.ID.Equals(id)).First();
            if (order != null)
            {
                order.ORDER_STATUS = 1;
                order.CREATE_DATE = DateTime.Now;
                takaDB.SaveChanges();
            }
        }

        public void AddRating(int idTea, int star, int idUser)
        {

            var findRating = takaDB.RATEs.Where(x => x.ID_TEA == idTea && x.ID_USER == idUser).ToList();
            if (findRating.Count() > 0)
            {
                RATE rate = findRating.First();
                rate.RATE1 = star;
                takaDB.SaveChanges();
            }
            else
            {
                RATE rate = new RATE();
                rate.ID_TEA = idTea;
                rate.ID_USER = idUser;
                rate.RATE1 = star;
                takaDB.RATEs.Add(rate);
                takaDB.SaveChanges();
            }
        }
        public void AddComment(int idTea, string comment, int idUser)
        {
            var findComment = takaDB.RATEs.Where(x => x.ID_TEA == idTea && x.ID_USER == idUser).ToList();
            if (findComment.Count() > 0)
            {
                RATE rate = findComment.First();
                rate.COMMENT = comment;
                takaDB.SaveChanges();
            }
            else
            {
                RATE rate = new RATE();
                rate.ID_TEA = idTea;
                rate.ID_USER = idUser;
                rate.COMMENT = comment;
                takaDB.RATEs.Add(rate);
                takaDB.SaveChanges();
            }
        }

    }
}