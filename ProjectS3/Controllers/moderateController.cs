using ProjectS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProjectS3.Controllers
{
    [Authorize(Roles = "Moderate")]
    public class moderateController : Controller
    {
        ProjectS3Entities db = new ProjectS3Entities();

        // GET: moderate
        public ActionResult Index(int id = 0)
        {
            List<SanPham> sanphams = new List<SanPham>();
            if (id == null || id == 0)
            {
                sanphams = db.SanPham.OrderByDescending(t => t.ID).ToList();
            }
            else
            {
                sanphams = db.SanPham.Where(t => t.ProductBranches.Id == id).OrderByDescending(t => t.ID).ToList();
            }
            return View(sanphams);
        }

        //////////////////////////
        /////// Product //////////
        //////////////////////////
        public ActionResult addproduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addproduct([Bind(Include = "ID,Ten,DioGia,linkanh,MoTa,TinhTrang,SoLuong,Color,Size,Branches,Type")] SanPham sanpham)
        {
            SanPham item = db.SanPham.Create();
            item.DioGia = sanpham.DioGia;
            item.linkanh = sanpham.linkanh;
            item.MoTa = sanpham.MoTa;
            item.SoLuong = sanpham.SoLuong;
            item.Ten = sanpham.Ten;
            item.TinhTrang = sanpham.TinhTrang;
            item.Color = sanpham.Color;
            item.Branches = sanpham.Branches;
            item.Type = sanpham.Type;
            item.Size = sanpham.Size;
            item.LastUpdateDate = DateTime.Now;
            item.UserUpdate = User.Identity.GetUserId();
            db.SanPham.Add(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult deleteproduct(int id)
        {
            SanPham sp = db.SanPham.SingleOrDefault(t => t.ID == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            sp.TinhTrang = "DISABLE";
            db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult editproduct(int id)
        {
            SanPham item = db.SanPham.SingleOrDefault(t => t.ID == id);
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
        [ValidateInput(false)]
        public ActionResult editproduct([Bind(Include = "ID,Ten,DioGia,linkanh,MoTa,TinhTrang,SoLuong,Color,Size,Branches,Type")] SanPham sanpham)
        {
            SanPham item = db.SanPham.SingleOrDefault(t => t.ID == sanpham.ID);
            if (item != null)
            {
                item.DioGia = sanpham.DioGia;
                item.linkanh = sanpham.linkanh;
                item.MoTa = sanpham.MoTa;
                item.SoLuong = sanpham.SoLuong;
                item.Ten = sanpham.Ten;
                item.TinhTrang = sanpham.TinhTrang;
                item.Color = sanpham.Color;
                item.Branches = sanpham.Branches;
                item.Size = sanpham.Size;
                item.Type = sanpham.Type;
                item.LastUpdateDate = DateTime.Now;
                item.UserUpdate = User.Identity.GetUserId();

                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        //////////////////////////
        /////// Group Product ////
        //////////////////////////
        public ActionResult removegroupproduct(int id)
        {
            BoSanPham item = db.BoSanPham.SingleOrDefault(t => t.ID == id);
            if (item != null)
            {
                db.ChiTietBoSanPham.RemoveRange(item.ChiTietBoSanPham);
                db.BoSanPham.Remove(item);
                db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public ActionResult addgroupproduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addgroupproduct(AddGroupProductModel model)
        {
            if (model.products.Count <= 0)
            {
                return null;
            }
            BoSanPham groups = db.BoSanPham.Create();
            groups.Ten = model.Ten;
            groups.Mota = model.Mota;
            groups.NgayTao = DateTime.Now;

            db.BoSanPham.Add(groups);
            for (int i = 0; i < model.products.Count; i++)
            {
                ChiTietBoSanPham item = db.ChiTietBoSanPham.Create();
                item.IDBoSanPham = groups.ID;
                item.IDSanPham = model.products[i].id;
                item.SoLuongThuongMua = model.products[i].number;

                db.ChiTietBoSanPham.Add(item);
            }
            db.SaveChangesAsync();
            return View();
        }

        public ActionResult editgroupproduct(int id)
        {
            if (id != null)
            {
                ViewBag.id = id;
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult editgroupproduct(EditGroupProductModel model)
        {
            if (model.products.Count <= 0)
            {
                return null;
            }
            BoSanPham groups = db.BoSanPham.SingleOrDefault(t => t.ID == model.id);

            if (groups != null)
            {
                groups.Ten = model.Ten;
                groups.Mota = model.Mota;
                groups.NgayTao = DateTime.Now;

                db.Entry(groups).State = System.Data.Entity.EntityState.Modified;

                for (int i = 0; i < groups.ChiTietBoSanPham.Count; i++)
                {
                    db.ChiTietBoSanPham.RemoveRange(groups.ChiTietBoSanPham);
                }

                for (int i = 0; i < model.products.Count; i++)
                {
                    ChiTietBoSanPham item = db.ChiTietBoSanPham.Create();
                    item.IDBoSanPham = groups.ID;
                    item.IDSanPham = model.products[i].id;
                    item.SoLuongThuongMua = model.products[i].number;

                    db.ChiTietBoSanPham.Add(item);
                }
                db.SaveChangesAsync();
                return View();
            }

            return HttpNotFound();
        }

        public ActionResult getGroupProduct(int id)
        {
            BoSanPham item = db.BoSanPham.SingleOrDefault(t => t.ID == id);

            if (item != null)
            {
                EditGroupProductModel model = new EditGroupProductModel();
                model.id = item.ID;
                model.Mota = item.Mota;
                model.Ten = item.Ten;
                model.products = new List<ProductInfo>();
                List<ChiTietBoSanPham> listpr = item.ChiTietBoSanPham.ToList();

                for (int i = 0; i < listpr.Count; i++)
                {
                    ProductInfo prod = new ProductInfo();
                    prod.id = listpr[i].IDSanPham;
                    prod.number = listpr[i].SoLuongThuongMua;
                    prod.Ten = listpr[i].SanPham.Ten;
                    prod.DioGia = listpr[i].SanPham.DioGia;

                    model.products.Add(prod);
                }

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //////////////////////////
        /////// Bài viết ////
        //////////////////////////
        public ActionResult addbaiviet()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addbaiviet(BaiViet model)
        {
            model.NgayDang = DateTime.Now;
            if (ModelState.IsValid)
            {
                BaiViet item = db.BaiViet.Create();
                item.linkHinh = model.linkHinh;
                item.NgayDang = DateTime.Now;
                item.NoiDung = model.NoiDung;
                item.TieuDe = model.TieuDe;

                db.BaiViet.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult editbaiviet(int id)
        {
            BaiViet baiviet = db.BaiViet.SingleOrDefault(t => t.ID == id);
            if (baiviet != null)
            {
                return View(baiviet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editbaiviet(BaiViet model)
        {
            model.NgayDang = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult deletebaiviet(int id)
        {
            BaiViet baiviet = db.BaiViet.SingleOrDefault(t => t.ID == id);
            if (baiviet != null)
            {
                db.BaiViet.Remove(baiviet);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //////////////////////////
        /////// Product Branches ////
        //////////////////////////
        [HttpPost]
        public ActionResult deleteproductbranch(int idbrach)
        {
            ProductBranches br = db.ProductBranches.SingleOrDefault(t => t.Id == idbrach);
            if (br == null)
            {
                return HttpNotFound();
            }

            db.ProductBranches.Remove(br);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult addproductbranch(string name)
        {
            ProductBranches br = db.ProductBranches.Create();
            br.Name = name;
            db.ProductBranches.Add(br);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult editproductbranch(int id, string displayview, string linkBannerImages)
        {
            ProductBranches br = db.ProductBranches.SingleOrDefault(t => t.Id == id);
            if (br == null)
            {
                return HttpNotFound();
            }

            br.DisplayView = displayview;
            br.linkBanerImage = linkBannerImages;
            db.Entry(br).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //////////////////////////
        /////// Product Type /////
        //////////////////////////
        [HttpPost]
        public ActionResult deleteproducttype(int idbrach)
        {
            ProductTypes br = db.ProductTypes.SingleOrDefault(t => t.Id == idbrach);
            if (br == null)
            {
                return HttpNotFound();
            }

            db.ProductTypes.Remove(br);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult addproducttype(string name)
        {
            ProductTypes br = db.ProductTypes.Create();
            br.Name = name;
            db.ProductTypes.Add(br);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}