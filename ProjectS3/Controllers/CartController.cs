using ProjectS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectS3.Controllers.MyEngines;
namespace ProjectS3.Controllers
{
    public class CartController : Controller
    {
        ProjectS3Entities db = new ProjectS3Entities();
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult thongtin()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult thongtin(ThongTinNguoiDungMuaHang model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("xacnhan", model);
            }
            return View(model);
        }

        [Authorize]
        public ActionResult xacnhan(ThongTinNguoiDungMuaHang model)
        {
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public string xacnhan(string diachi, string dienthoai, string thoigian,
            List<SanPhamTrongGioHang> chitiet)
        {
            if (chitiet != null)
            {
                if (chitiet.Count <= 0)
                {
                    return null;
                }
                MyDynamicValues dynamic = new MyDynamicValues();
                float tygia_WonVND = MyStaticFunction.MyFloatParse(dynamic.getValue("tygia_WonVND"));
                float hesonhan = MyStaticFunction.MyFloatParse(dynamic.getValue("hesonhan"));

                DonHang dh = db.DonHang.Create();

                dh.UserID = User.Identity.GetUserId();
                dh.SoDienThoai = dienthoai;
                dh.ThoiGianGiao = DateTime.Parse(thoigian);
                dh.TinhTrang = 1;
                dh.DiaChiGiao = diachi;
                dh.NgayTao = DateTime.Now;

                db.DonHang.Add(dh);

                for (int i = 0; i < chitiet.Count; i++)
                {
                    int tempid = chitiet[i].id;
                    SanPham sp = db.SanPham.SingleOrDefault(t => t.ID == tempid);
                    if (sp == null)
                    {
                        MyEngines.MySystemLog.WriteLog("ERROR: /Cart/xacnhan:  Add to cart error: ID San Phan Cant not found: " + chitiet[i].id);
                        continue;
                    }

                    ChiTietDonHang item = new ChiTietDonHang();
                    item.IDDonHang = dh.ID;
                    item.IDSanPham = chitiet[i].id;
                    item.SoLuong = chitiet[i].soluong;
                    item.IDBoSanPham = chitiet[i].idbosanpham;
                    item.DioGia = sp.DioGia * tygia_WonVND * hesonhan;

                    item.Size = chitiet[i].size;
                    item.Color = chitiet[i].color;

                    db.ChiTietDonHang.Add(item);
                }

                db.SaveChanges();
                
                // sendmessage to gmail
                GMailer gmail = new GMailer();
                gmail.Send("Đơn đặt hàng mới", "Đơn đặt hàng mới + " + dh.ID + ". Ngày hết hạn: " + dh.ThoiGianGiao);
                return dh.ID.ToString();
            }

            return null;
        }

        public ActionResult thankyou(string ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        [Authorize]
        public ActionResult history()
        {
            string userid = User.Identity.GetUserId();
            List<DonHang> donhang = db.DonHang.Where(t => t.UserID == userid).OrderByDescending(t => t.NgayTao).ToList();
            return View(donhang);
        }
    }
}