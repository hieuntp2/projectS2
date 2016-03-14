﻿using ProjectS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectS3.Controllers.MyEngines;
using Newtonsoft.Json;
using System.Net;
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
            return View();
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
        public string xacnhan(string diachi, string dienthoai, string thoigian, string hoten,
            List<SanPhamTrongGioHang> chitiet)
        {
            // captcha
            var response = Request["g-recaptcha-response"];
            //secret that was generated in key value pair
            const string secret = "6LdcvhoTAAAAACcr5w_9ShJ5eI98k4oveNDYy1EX";

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                secret, response));

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
                    MyDynamicValues dynamic = new MyDynamicValues();
                    float tygia_WonVND = MyStaticFunction.MyFloatParse(dynamic.getValue("tygia_WonVND"));
                    float hesonhan = MyStaticFunction.MyFloatParse(dynamic.getValue("hesonhan"));

                    DonHang dh = db.DonHang.Create();

                    if(User.Identity.IsAuthenticated)
                    {
                        dh.UserID = User.Identity.GetUserId();
                    }
                 
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

    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}