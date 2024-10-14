using UstamOdev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

namespace UstamOdev.Controllers
{
    public class UserController : Controller
    {

        UstamOdevEntities db = new UstamOdevEntities();
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("GirisYap");
            }
        }

        public ActionResult GirisYap(string err)
        {
            ViewBag.error = err;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GirisYap(musteriler m)
        {
            if (!ModelState.IsValid)
            {
                var bulunan_musteri = db.musteriler.Where(musteri => musteri.musteri_email.Equals(m.musteri_email) && musteri.musteri_sifre.Equals(m.musteri_sifre));
                if (bulunan_musteri.Count() > 0)
                {
              

                    Session["id"] = bulunan_musteri.FirstOrDefault().musteri_id;
                    Session["isim"] = bulunan_musteri.FirstOrDefault().musteri_isim_soyisim;
                    Session["email"] = bulunan_musteri.FirstOrDefault().musteri_email;

                    return RedirectToAction("Index");
                }
                else
                {
                
                    string error = "Kullanıcı adı veya şifre hatalı";
                    return RedirectToAction("GirisYap", new { err = error });
                }


            }

            return View();
        }


        public ActionResult KayıtOl()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KayıtOl(musteriler m, string musteri_il)
        {

            if (ModelState.IsValid)
            {
             
                var kontrol = db.musteriler.FirstOrDefault(musteri => musteri.musteri_email == m.musteri_email);
                if (kontrol == null)
                {
                    m.musteri_il = musteri_il;
                    db.musteriler.Add(m);
                    db.SaveChanges();

                    return RedirectToAction("GirisYap");
                }

                else
                {
                    ViewBag.error = "E-Posta adı zaten mevcut";
                    return View();
                }
            }
            return View();
        }

        public ActionResult CikisYap()
        {
            Session.Clear();
            return RedirectToAction("GirisYap");
        }

        public ActionResult Profil()
        {
            int id = (int)Session["id"];
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
        public ActionResult Teklifler()
        {
            if (Session["id"] != null)
            {
                int id = (int)Session["id"];
                var isler = db.onayli_isler.Where(t => t.musteri_id == id && t.onay_durum == "Beklemede").AsEnumerable().Select(t => t.is_id).ToList();

                var teklifler = db.is_ilanlari.Where(i => isler.Contains(i.is_id));

                return View(teklifler);

            }
            else
            {
                return RedirectToAction("GirisYap");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeklifSil(int id)
        {
            int musteri = (int)Session["id"];
            var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri && t.is_id == id).FirstOrDefault();
            db.onayli_isler.Remove(teklifler);
            db.SaveChanges();
            return RedirectToAction("Teklifler");
        }

        public ActionResult TeklifSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int musteri = (int)Session["id"];
                var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri && t.is_id == id).FirstOrDefault();

                if (teklifler == null)
                {
                    return HttpNotFound();
                }
                return View(teklifler);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeklifOnayla(int id)
        {
            int musteri = (int)Session["id"];
            var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri && t.is_id == id).FirstOrDefault();
            db.onayli_isler.Remove(teklifler);
            db.SaveChanges();
            return RedirectToAction("Onaylilar");
        }

        public ActionResult TeklifOnayla(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int musteri = (int)Session["id"];
                var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri && t.is_id == id).FirstOrDefault();

                if (teklifler == null)
                {
                    return HttpNotFound();
                }
                return View(teklifler);
            }
        }

        public ActionResult Onaylilar()
        {
            if (Session["id"] != null)
            {
                int id = (int)Session["id"];
                var isler = db.onayli_isler.Where(t => t.musteri_id == id && t.onay_durum == "Onaylı").AsEnumerable().Select(t => t.is_id).ToList();

                var teklifler = db.is_ilanlari.Where(i => isler.Contains(i.is_id));
              
                return View(teklifler);

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

        public ActionResult IsDetay(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int musteriler = (int)Session["id"];
                is_ilanlari i = db.is_ilanlari.Find(id);
                var onayli = db.onayli_isler.Where(x => x.musteri_id == musteriler && x.is_id == id).FirstOrDefault();
               if(onayli != null)
                {
                    ViewBag.Error = "Bu ilana zaten teklif verdiniz.";
                }
                if (i == null)
                {
                    return HttpNotFound();
                }
                return View(i);
            }
        }
        public ActionResult Teklif(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int musteri_id = (int)Session["id"];
                onayli_isler kontrol = db.onayli_isler.Where(t => t.is_id == id && t.musteri_id == musteri_id).FirstOrDefault();

                if (kontrol == null)
                {
                    onayli_isler yeniTeklif = new onayli_isler();
                    yeniTeklif.musteri_id = musteri_id;
                    yeniTeklif.is_id = (int)id;
                    yeniTeklif.onay_durum = "Beklemede";
                    db.onayli_isler.Add(yeniTeklif);
                    db.SaveChanges();
                }
                return RedirectToAction("Isler");
            }

        }
        public ActionResult ProfilDuzenle(int? id)
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
        public ActionResult ProfilDuzenle(musteriler m, string musteri_il)
        {

            m.musteri_id = (int)Session["id"];
            if (ModelState.IsValid)
            {
                m.musteri_il = musteri_il;
                db.Entry(m).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Profil", "User");
            }
            else
            {
                return View(m);
            }
        }


    }
}