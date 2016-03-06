
using ProjectS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectS3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class adminController : Controller
    {
        ProjectS3Entities db = new ProjectS3Entities();
        //
        // GET: /admin/
        public ActionResult Index()
        {
            MyDynamicValues mydynamic = new MyDynamicValues();
            string mytoemail = mydynamic.getValue("toemail");
            string myfromemail = mydynamic.getValue("fromemail");
            string mypassword = mydynamic.getValue("password");

            ViewBag.mytoemail = mytoemail;
            ViewBag.myfromemail = myfromemail;
            ViewBag.mypassword = mypassword;

            return View();
        }

        public ActionResult testSendEmail()
        {
            MyEngines.GMailer gmail = new MyEngines.GMailer();
            gmail.Send("Test email!", "This is test-email");
            return RedirectToAction("Index");
        }

        public ActionResult editemail(string fromemail, string password, string toemail)
        {
            MyDynamicValues mydynamic = new MyDynamicValues();
            mydynamic.setValue("toemail", toemail);
            mydynamic.setValue("fromemail", fromemail);
            mydynamic.setValue("password", password);

            return RedirectToAction("Index");
        }

        //////////////////////////
        /////// Order //////////
        //////////////////////////
        public ActionResult chitietdonhang(int id)
        {
            DonHang item = db.DonHang.SingleOrDefault(t => t.ID == id);

            if (item != null)
            {
                return View(item);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult doitrangthaidonhang(int id, int tinhtrang)
        {
            DonHang item = db.DonHang.SingleOrDefault(t => t.ID == id);

            if (item != null)
            {
                item.TinhTrang = (short)tinhtrang;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult userinfor(string id)
        {
            AspNetUsers user = db.AspNetUsers.SingleOrDefault(t => t.Id == id);
            return View(user);
        }

        public ActionResult createheaditem()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult createheaditem(HeadItem model)
        {
            HeadItem item = db.HeadItem.Create();
            if (ModelState.IsValid)
            {
                item.caption = model.caption;
                item.href = model.href;
                item.image = model.image;
                item.type = model.type;

                db.HeadItem.Add(item);
                db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult deleteheaditem(int id)
        {
            HeadItem item = db.HeadItem.SingleOrDefault(t => t.id == id);
            if (item != null)
            {
                db.HeadItem.Remove(item);
                db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public ActionResult role()
        {
            return View(db.AspNetRoles.ToList());
        }

        [HttpPost]
        public ActionResult addUserToRole(string username, string roleid)
        {
            AspNetUsers user = db.AspNetUsers.SingleOrDefault(t => t.UserName == username);
            AspNetRoles role = db.AspNetRoles.SingleOrDefault(t => t.Id == roleid);
            role.AspNetUsers.Add(user);
            db.Entry(role).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("role");
        }

        public ActionResult removeUserRole(string username, string roleid)
        {
            AspNetUsers user = db.AspNetUsers.SingleOrDefault(t => t.UserName == username);
            AspNetRoles role = db.AspNetRoles.SingleOrDefault(t => t.Id == roleid);
            role.AspNetUsers.Remove(user);
            db.Entry(role).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("role");
        }


        ////////////////////////////
        /////// Cong cu ty gia /////
        ////////////////////////////

        /// <summary>
        /// Đổi 1 Won - ? VNĐ
        /// </summary>
        /// <param name="tygia"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> edittygia(string tygia)
        {
            MyDynamicValues dynamic = new MyDynamicValues();
            if (MyEngines.MyStaticFunction.MyFloatParse(tygia) <= 0)
            {
                return RedirectToAction("Index");
            }
            await dynamic.setValue("tygia_WonVND", tygia);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Đổi 1 Won - ? VNĐ
        /// </summary>
        /// <param name="tygia"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> hesonhangia(string tygia)
        {
            MyDynamicValues dynamic = new MyDynamicValues();
            if (MyEngines.MyStaticFunction.MyFloatParse(tygia) <= 0)
            {
                return RedirectToAction("Index");
            }
            await dynamic.setValue("hesonhan", tygia);
            return RedirectToAction("Index");
        }
    }

    public class AddGroupProductModel
    {
        public string Ten { get; set; }
        public string Mota { get; set; }
        public List<ItemInGroupProductModel> products { get; set; }
    }

    public class ItemInGroupProductModel
    {
        public int id { get; set; }
        public int number { get; set; }
    }

    public class EditGroupProductModel
    {
        public int id { get; set; }
        public string Ten { get; set; }
        public string Mota { get; set; }
        public List<ProductInfo> products { get; set; }
    }

    public class ProductInfo
    {
        public int id { get; set; }
        public string Ten { get; set; }
        public double DioGia { get; set; }

        public int number { get; set; }
    }
}