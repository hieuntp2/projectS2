using ProjectS3.Controllers.MyEngines;
using ProjectS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjectS3.Controllers
{
    public class HomeController : Controller
    {
        ProjectS3Entities db = new ProjectS3Entities();
        public ActionResult Index()
        {
            MyDynamicEngine dynamic = new MyDynamicEngine();
            dynamic.increaseValue("HOME_COUNT");
            return View();
        }

        public ActionResult getallproducts()
        {
            List<SanPhamReturn> sanphams = (from item in db.SanPham.AsNoTracking()
                                            select new SanPhamReturn
                                            {
                                                Ten = item.Ten,
                                                ID = item.ID,
                                                DonGia = item.DioGia,
                                                MoTa = item.MoTa,
                                                linkanh = item.linkanh
                                            }).ToList();
            return Json(sanphams, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getallgroupproducts()
        {
            List<BoSanPhamIndexModal> sanphams = (from item in db.BoSanPham.AsNoTracking()
                                                  select new BoSanPhamIndexModal
                                                  {
                                                      Ten = item.Ten,
                                                      id = item.ID,
                                                      img = item.ChiTietBoSanPham.FirstOrDefault().SanPham.linkanh
                                                  }).ToList();
            return Json(sanphams, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getproductinfor(int id)
        {
            SanPham item = db.SanPham.SingleOrDefault(t => t.ID == id);
            if (item != null)
            {
                SanPhamReturn sanpham = new SanPhamReturn
                {
                    Ten = item.Ten,
                    ID = item.ID,
                    DonGia = item.DioGia,
                    MoTa = item.MoTa,
                    linkanh = item.linkanh
                };
                return Json(sanpham, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public ActionResult getgroupproductinfo(int id)
        {
            BoSanPham item = db.BoSanPham.SingleOrDefault(t => t.ID == id);
            if (item != null)
            {
                BoSanPhamInfoIndexModal sanpham = new BoSanPhamInfoIndexModal
                {
                    Ten = item.Ten,
                    id = item.ID
                };

                List<ChiTietBoSanPham> chitiets = item.ChiTietBoSanPham.ToList();

                for (int i = 0; i < chitiets.Count; i++)
                {
                    SanPhamTrongBoSanPham pro = new SanPhamTrongBoSanPham();
                    pro.ID = chitiets[i].IDSanPham;
                    pro.linkanh = chitiets[i].SanPham.linkanh;
                    pro.MoTa = chitiets[i].SanPham.MoTa;
                    pro.Ten = chitiets[i].SanPham.Ten;

                    sanpham.products.Add(pro);
                }
                return Json(sanpham, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult bosanpham(int id)
        {
            BoSanPham item = db.BoSanPham.SingleOrDefault(t => t.ID == id);
            if (item != null)
            {
                BoSanPhamInfoIndexModal sanpham = new BoSanPhamInfoIndexModal
                {
                    Ten = item.Ten,
                    id = item.ID,
                    Mota = item.Mota
                };

                List<ChiTietBoSanPham> chitiets = item.ChiTietBoSanPham.ToList();
                sanpham.products = new List<SanPhamTrongBoSanPham>();
                for (int i = 0; i < chitiets.Count; i++)
                {
                    SanPhamTrongBoSanPham pro = new SanPhamTrongBoSanPham();
                    pro.ID = chitiets[i].IDSanPham;
                    pro.linkanh = chitiets[i].SanPham.linkanh;
                    pro.MoTa = chitiets[i].SanPham.MoTa;
                    pro.Ten = chitiets[i].SanPham.Ten;
                    pro.GiaThuongMua = chitiets[i].SoLuongThuongMua;
                    pro.DonGia = chitiets[i].SanPham.DioGia;

                    sanpham.products.Add(pro);
                }
                return View(sanpham);
            }

            return HttpNotFound();
        }

        public ActionResult sanpham(int id)
        {
            SanPham item = db.SanPham.SingleOrDefault(t => t.ID == id && t.TinhTrang == "ENABLE");
            if (item != null)
            {
                SanPhamReturn sanpham = new SanPhamReturn
                {
                    Ten = item.Ten,
                    ID = item.ID,
                    MoTa = item.MoTa,
                    DonGia = item.DioGia,
                    linkanh = item.linkanh,
                    branch = item.ProductBranches.Name,
                    isInstock = (item.SoLuong != 0 ? true : false),
                    color = item.Color,
                    size = item.Size
                };

                List<SanPhamReturn> sameProduct = (from prduc in db.SanPham
                                                       where prduc.Branches == item.Branches && prduc.TinhTrang == "ENABLE"
                                                       select new SanPhamReturn()
                                                       {
                                                           ID = prduc.ID,
                                                           Ten = prduc.Ten,
                                                           DonGia = prduc.DioGia,
                                                           brandID = prduc.ProductBranches.Id,
                                                           branch = prduc.ProductBranches.Name,
                                                           TypeID = (int)prduc.Type,
                                                           TypeName = prduc.ProductTypes.Name,
                                                           linkanh = prduc.linkanh
                                                       }
                                                    ).ToList();

                ViewBag.sameProduct = sameProduct;
                return View(sanpham);
            }
            return HttpNotFound();
        }

        public ActionResult baiviet(int id)
        {
            BaiViet item = db.BaiViet.SingleOrDefault(t => t.ID == id);
            if (item != null)
            {
                return View(item);
            }

            return RedirectToAction("index");
        }

        public ActionResult listsanphamrelated(int id, bool bsp, bool israndom = false)
        {
            List<RelativeObject> results = new List<RelativeObject>();
            // Nếu là bộ sản phẩm thì đưa ra ds sản phẩm
            if (bsp)
            {
                results = (from i in db.SanPham.AsNoTracking()
                           select new RelativeObject
                           {
                               id = i.ID,
                               name = i.Ten,
                               isgroup = false,
                               linkanh = i.linkanh
                           }).ToList();
            }
            // Nếu là sản phẩm thì đưa ra ds các bộ sản phẩm mà sản phẩm đó có trong
            else
            {
                SanPham sanpham = db.SanPham.SingleOrDefault(t => t.ID == id);
                results = (from i in db.ChiTietBoSanPham
                           where i.IDSanPham == id
                           select new RelativeObject
                           {
                               id = i.IDBoSanPham,
                               name = i.BoSanPham.Ten,
                               isgroup = true,
                               linkanh = sanpham.linkanh
                           }).ToList();
            }

            // Nếu random sản phẩm thì đưa ra ds sản phẩm random
            if (israndom)
            {
                List<RelativeObject> listrandom = new List<RelativeObject>();

                Random rnd = new Random();
                int xrd = rnd.Next(2, 5);
                listrandom = (from i in db.SanPham.AsNoTracking()
                              where (i.ID % xrd == 0)
                              select new RelativeObject
                              {
                                  id = i.ID,
                                  name = i.Ten,
                                  isgroup = false,
                                  linkanh = i.linkanh
                              }).ToList();
                results.AddRange(listrandom);
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult branch(string id)
        {
            ProductBranches branch = db.ProductBranches.SingleOrDefault(t => t.Name == id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            else
            {
                List<SanPhamReturn> list = (from sanpham in db.SanPham
                                                where sanpham.Branches == branch.Id && sanpham.TinhTrang == "ENABLE"
                                            select new SanPhamReturn()
                                                    {
                                                        ID = sanpham.ID,
                                                        Ten = sanpham.Ten,
                                                        DonGia = sanpham.DioGia,
                                                        brandID = sanpham.ProductBranches.Id,
                                                        branch = sanpham.ProductBranches.Name,
                                                        TypeID = (int)sanpham.Type,
                                                        TypeName = sanpham.ProductTypes.Name,
                                                        linkanh = sanpham.linkanh
                                                    }
                                                    ).ToList();

                ViewBag.Branch = branch.Name;
                ViewBag.BanngerImages = branch.linkBanerImage;

                return View(branch.DisplayView, list);
            }
        }
    }

    public class SanPhamReturn
    {
        public string Ten { get; set; }
        public int ID { get; set; }
        private double _dongia;

        public double DonGia
        {
            get
            {
                return this._dongia;
            }
            set
            {
                MyDynamicEngine dynamic = new MyDynamicEngine();
                float tygia_WonVND = MyStaticFunction.MyFloatParse(dynamic.getValue("tygia_WonVND"));
                double temp_Gia = value * tygia_WonVND;
                float hesonhan = 1;

                if (temp_Gia < 1000000)
                {
                    hesonhan = MyStaticFunction.MyFloatParse(dynamic.getValue("hesonhan_1"));
                }
                else
                    if (temp_Gia < 2000000)
                    {
                        hesonhan = MyStaticFunction.MyFloatParse(dynamic.getValue("hesonhan_1_2"));
                    }
                    else
                        if (temp_Gia < 3000000)
                        {
                            hesonhan = MyStaticFunction.MyFloatParse(dynamic.getValue("hesonhan_2_3"));
                        }
                        else
                        {
                            hesonhan = MyStaticFunction.MyFloatParse(dynamic.getValue("hesonhan_3"));
                        }
                this._dongia = hesonhan * temp_Gia;

                // Làm tròn đến trăm nghìn
                this._dongia = this._dongia / 1000;
                this._dongia = Math.Round(this._dongia, 0) * 1000;
            }
        }
        public string MoTa { get; set; }
        public string linkanh { get; set; }
        public string branch { get; set; }
        public bool isInstock { get; set; }
        public string color { get; set; }
        public string size { get; set; }

        public int brandID { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }

    }

    public class BoSanPhamIndexModal
    {
        public string Ten { get; set; }
        public int id { get; set; }
        public string img { get; set; }
    }

    public class BoSanPhamInfoIndexModal
    {
        public string Ten { get; set; }
        public int id { get; set; }
        public string Mota { get; set; }
        public List<SanPhamTrongBoSanPham> products { get; set; }
    }

    public class RelativeObject
    {
        public string name;
        public int id;
        public bool isgroup;
        public string linkanh;
    }

    public class SanPhamTrongBoSanPham
    {
        public string Ten { get; set; }
        public int ID { get; set; }
        public double DonGia { get; set; }
        public string MoTa { get; set; }
        public string linkanh { get; set; }
        public int GiaThuongMua { get; set; }
    }
}