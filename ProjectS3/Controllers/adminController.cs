
using ProjectS3.Controllers.MyEngines;
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
            MyDynamicEngine mydynamic = new MyDynamicEngine();
            string mytoemail = mydynamic.getValue("toemail");
            string myfromemail = mydynamic.getValue("fromemail");
            string mypassword = mydynamic.getValue("password");

            ViewBag.mytoemail = mytoemail;
            ViewBag.myfromemail = myfromemail;
            ViewBag.mypassword = mypassword;

            return View();
        }

        public async Task<ActionResult> testSendEmail()
        {
            MyEngines.GMailer gmail = new MyEngines.GMailer();
            await gmail.Send("Test email!", "Nếu bạn thấy email này, có nghĩa website đã được cài đặt thành công gửi email thông báo đến admin.");
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> testSendEmailToUser()
        {
            MyEngines.GMailer gmail = new MyEngines.GMailer();

            MyDynamicEngine mydynamic = new MyDynamicEngine();
            string mytoemail = mydynamic.getValue("toemail");

            UpdateOrderToUserModel userintfo1 = new UpdateOrderToUserModel();
            userintfo1.hoten = "Họ tên người dùng";
            userintfo1.email = mytoemail;
            userintfo1.iddonhang = "IDTest";
            userintfo1.tinhtrang = "Mới cập nhật";

            await gmail.SendWithCreateOrderToUserTemplate("Test Email", userintfo1);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> editemail(string fromemail, string password, string toemail)
        {
            MyDynamicEngine mydynamic = new MyDynamicEngine();
            await mydynamic.setValue("toemail", toemail);
            await mydynamic.setValue("fromemail", fromemail);
            await mydynamic.setValue("password", password);

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

        [HttpPost]
        public async Task<ActionResult> updatecomment(int id, string comment)
        {
            DonHang donhang = db.DonHang.SingleOrDefault(t => t.ID == id);
            if(donhang == null)
            {
                return HttpNotFound();
            }

            donhang.Comment = comment;
            db.Entry(donhang).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("chitietdonhang", new { id = id });
        }

        public async Task<ActionResult> doitrangthaidonhang(int id, int tinhtrang)
        {
            DonHang item = db.DonHang.SingleOrDefault(t => t.ID == id);

            if (item != null)
            {
                item.TinhTrang = (short)tinhtrang;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
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
        public async Task<ActionResult> createheaditem(HeadItem model)
        {
            HeadItem item = db.HeadItem.Create();
            if (ModelState.IsValid)
            {
                item.caption = model.caption;
                item.href = model.href;
                item.image = model.image;
                item.type = model.type;

                db.HeadItem.Add(item);
                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        public async Task<ActionResult> deleteheaditem(int id)
        {
            HeadItem item = db.HeadItem.SingleOrDefault(t => t.id == id);
            if (item != null)
            {
                db.HeadItem.Remove(item);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public ActionResult role()
        {
            return View(db.AspNetRoles.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> addUserToRole(string username, string roleid)
        {
            AspNetUsers user = db.AspNetUsers.SingleOrDefault(t => t.UserName == username);
            AspNetRoles role = db.AspNetRoles.SingleOrDefault(t => t.Id == roleid);
            role.AspNetUsers.Add(user);
            db.Entry(role).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("role");
        }

        public async Task<ActionResult> removeUserRole(string username, string roleid)
        {
            AspNetUsers user = db.AspNetUsers.SingleOrDefault(t => t.UserName == username);
            AspNetRoles role = db.AspNetRoles.SingleOrDefault(t => t.Id == roleid);
            role.AspNetUsers.Remove(user);
            db.Entry(role).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
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
            MyDynamicEngine dynamic = new MyDynamicEngine();
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
        public async Task<ActionResult> hesonhangia(string my_heso_1, string my_heso_1_2, string my_heso_2_3, string my_heso_3)
        {
            MyDynamicEngine dynamic = new MyDynamicEngine();
            if (MyEngines.MyStaticFunction.MyFloatParse(my_heso_1) <= 0)
            {
                return RedirectToAction("Index");
            }
            await dynamic.setValue("hesonhan_1", my_heso_1);

            if (MyEngines.MyStaticFunction.MyFloatParse(my_heso_1_2) <= 0)
            {
                return RedirectToAction("Index");
            }
            await dynamic.setValue("hesonhan_1_2", my_heso_1_2);

            if (MyEngines.MyStaticFunction.MyFloatParse(my_heso_2_3) <= 0)
            {
                return RedirectToAction("Index");
            }
            await dynamic.setValue("hesonhan_2_3", my_heso_2_3);

            if (MyEngines.MyStaticFunction.MyFloatParse(my_heso_3) <= 0)
            {
                return RedirectToAction("Index");
            }
            await dynamic.setValue("hesonhan_3", my_heso_3);

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