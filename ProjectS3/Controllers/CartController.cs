using ProjectS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectS3.Controllers.MyEngines;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        //[Authorize]
        public ActionResult thongtin()
        {
            ThongTinNguoiDungMuaHang model = new ThongTinNguoiDungMuaHang();
            if(User.Identity.IsAuthenticated)
            {
                string userid = User.Identity.GetUserId();
                AspNetUsers user = db.AspNetUsers.SingleOrDefault(t => t.Id == userid);

                model.DiaChiGiao = user.DiaChi;
                model.Email = user.Email;
                model.HoTen = user.HoTen;
                model.SoDienThoai = user.SoDienThoai;
                model.ThoiGianGiao = DateTime.Now;
            }
            return View(model);
        }

        //[Authorize]
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

        //[Authorize]
        public ActionResult xacnhan(ThongTinNguoiDungMuaHang model)
        {
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        public async Task<string> xacnhan(string diachi, string dienthoai, string thoigian, string hoten,
            List<SanPhamTrongGioHang> chitiet, string captval, string email)
        {
            //secret that was generated in key value pair
            const string secret = "6LdZwRoTAAAAAEOn-B188a6bk-Yr7wrRhf8yLT6j";

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                secret, captval));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (captchaResponse.Success == "false")
            {
                if (captchaResponse.ErrorCodes.Count <= 0)
                    return null;

                var error = captchaResponse.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        ViewBag.Message = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ViewBag.Message = "The secret parameter is invalid or malformed.";
                        break;

                    case ("missing-input-response"):
                        ViewBag.Message = "The response parameter is missing.";
                        break;
                    case ("invalid-input-response"):
                        ViewBag.Message = "The response parameter is invalid or malformed.";
                        break;

                    default:
                        ViewBag.Message = "Error occured. Please try again";
                        break;
                }

                return null;
            }
            else
            {
                // Nếu không phải là người máy
                if (chitiet != null)
                {
                    if (chitiet.Count <= 0)
                    {
                        return null;
                    }

                    DonHang dh = db.DonHang.Create();

                    if (User.Identity.IsAuthenticated)
                    {
                        dh.UserID = User.Identity.GetUserId();
                    }
                    else
                    {
                        // this is default account for user dont login
                        dh.UserID = null;
                    }

                    dh.SoDienThoai = dienthoai;
                    dh.ThoiGianGiao = DateTime.Parse(thoigian);
                    dh.TinhTrang = 1;
                    dh.DiaChiGiao = diachi;
                    dh.NgayTao = DateTime.Now;
                    dh.HoTen = hoten;
                    dh.Email = email;
                    db.DonHang.Add(dh);

                    for (int i = 0; i < chitiet.Count; i++)
                    {
                        int tempid = chitiet[i].id;

                        ChiTietDonHang item = new ChiTietDonHang();
                        item.IDDonHang = dh.ID;
                        item.IDSanPham = chitiet[i].id;
                        item.SoLuong = chitiet[i].soluong;
                        item.IDBoSanPham = chitiet[i].idbosanpham;
                        item.DioGia = chitiet[i].dongia;

                        item.Size = chitiet[i].size;
                        item.Color = chitiet[i].color;

                        db.ChiTietDonHang.Add(item);
                    }

                    await db.SaveChangesAsync();

                    await sendEmail(dh);
                    return dh.ID.ToString();
                }
            }

            return null;
        }

        private static async Task sendEmail(DonHang dh)
        {
            // sendmessage to admin
            GMailer gmail = new GMailer();
            string mylink = "http://inthef.vn/admin";
            Regex r = new Regex(@"(https?://[^\s]+)");
            mylink = r.Replace(mylink, "<a href=\"$1\">$1</a>");

            var messagebody = "Đơn đặt hàng mới: " + dh.ID + ". Ngày hết hạn: " + dh.ThoiGianGiao + ". Kiểm tra đơn hàng: " + mylink;
            await gmail.Send("Đơn đặt hàng mới: Mã " + dh.ID, messagebody);

            // send email to user
            if (dh.Email != null)
            {
                UpdateOrderToUserModel usermodel = new UpdateOrderToUserModel();
                usermodel.email = dh.Email;
                usermodel.hoten = dh.HoTen;
                usermodel.iddonhang = dh.ID.ToString();
                usermodel.tinhtrang = "Mới đặt";
                await gmail.SendWithCreateOrderToUserTemplate("Đơn hàng đã được đặt", usermodel);
            }
        }

        public ActionResult thankyou(string ID)
        {
            GMailer gmail = new GMailer();
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

    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}