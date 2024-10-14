using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UstamOdev.Models;

namespace UstamOdev.Controllers
{
    public class AdminController : Controller
    {
        
        UstamOdevEntities db = new UstamOdevEntities();
        
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                return View();
            }
            return RedirectToAction("GirisYap");
        }

        public ActionResult GirisYap(string hata)
        {
            ViewBag.hata = hata;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GirisYap(adminler admin)
        {
            if (ModelState.IsValid)
            {                              
                var kayit = db.adminler.Where(a => a.admin_email == admin.admin_email && a.admin_sifre == admin.admin_sifre);
                if (kayit.Count() > 0)
                {
                    
                    Session["id"] = kayit.FirstOrDefault().admin_email;
                    return RedirectToAction("Index");
                }
                else
                {
                   
                    string hataMesaji = "E-Posta veya şifre hatalı";
                    return RedirectToAction("GirisYap", new { hata = hataMesaji }); 
                }
            }

            return View();
        }

        public ActionResult CikisYap()
        {
            Session.Clear();
            return RedirectToAction("Index","Home");
        }
        public ActionResult Ustalar()
        {
            if (Session["id"] != null)
            {
                var ustalar = db.ustalar;
                return View(ustalar);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }

        }
        public ActionResult Musteriler()
        {
            if (Session["id"] != null)
            {
                var musteriler = db.musteriler;
                return View(musteriler);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }

        }
        public ActionResult OnaysizUstalar()
        {
            if (Session["id"] != null)
            {
                var ustalar = db.ustalar.Where(i => i.usta_onay == "H");
                return View(ustalar);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }

        }
        public ActionResult MusteriDuzenle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                musteriler m = db.musteriler.Find(id);

                if (m == null)
                {
                    return HttpNotFound();
                }
                return View(m);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MusteriDuzenle(musteriler m, int id, string musteri_il)
        {

            m.musteri_id = id;
            if (ModelState.IsValid)
            {
                m.musteri_il = musteri_il;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Musteriler", "Admin");
            }
            else
            {
                return View(m);
            }
        }
        public ActionResult UstaDuzenle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ustalar m = db.ustalar.Find(id);

                if (m == null)
                {
                    return HttpNotFound();
                }
                return View(m);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UstaDuzenle(ustalar u, int id, string usta_il, string usta_onay)
        {

            u.usta_id = id;
            if (ModelState.IsValid)
            {
                u.usta_il = usta_il;
                u.usta_onay = usta_onay;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Ustalar", "Admin");
            }
            else
            {
                return View(u);
            }
        }
        public ActionResult SertifikaGoruntule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ustalar m = db.ustalar.Find(id);

                if (m == null)
                {
                    return HttpNotFound();
                }
                return View(m);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UstaSil(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ustalar i = db.ustalar.Find(id);
                db.ustalar.Remove(i);
                db.SaveChanges();
                return RedirectToAction("Ustalar");
            }
        }

        public ActionResult UstaSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ustalar i = db.ustalar.Find(id);

                if (i == null)
                {
                    return HttpNotFound();
                }
                return View(i);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MusteriSil(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                musteriler i = db.musteriler.Find(id);
                db.musteriler.Remove(i);
                db.SaveChanges();
                return RedirectToAction("Musteriler");
            }
        }

        public ActionResult MusteriSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                musteriler i = db.musteriler.Find(id);

                if (i == null)
                {
                    return HttpNotFound();
                }
                return View(i);
            }
        }
        public ActionResult Isler()
        {
            if (Session["id"] != null)
            {
                var isler = db.is_ilanlari;
                return View(isler);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }

        }
        public ActionResult Kategori(string kategori)
        {
            if (Session["id"] != null)
            {
                var isler = db.is_ilanlari.Where(i => i.is_tur == kategori);
                ViewBag.kategori = kategori;
                return View(isler);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsSil(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var onaylar = from onayli_isler in db.onayli_isler.Where(x => x.is_id == id) select onayli_isler;
               if(onaylar != null)
                {
                    foreach (onayli_isler o in onaylar)
                    {
                        db.onayli_isler.Remove(o);
                    }
                }

                is_ilanlari i = db.is_ilanlari.Find(id);
                db.is_ilanlari.Remove(i);
                db.SaveChanges();
                return RedirectToAction("Isler");
            }
        }

        public ActionResult IsSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                is_ilanlari i = db.is_ilanlari.Find(id);

                if (i == null)
                {
                    return HttpNotFound();
                }
                return View(i);
            }
        }
        public ActionResult IsDuzenle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                is_ilanlari i = db.is_ilanlari.Find(id);

                if (i == null)
                {
                    return HttpNotFound();
                }
                return View(i);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsDuzenle(is_ilanlari i, string is_tur)
        {

            i.usta_id = (int)Session["id"];
            if (ModelState.IsValid)
            {
                i.is_tur = is_tur;
                db.Entry(i).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Isler", "Admin");
            }
            else
            {
                return View(i);
            }
        }
    }
}