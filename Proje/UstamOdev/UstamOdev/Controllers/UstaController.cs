using UstamOdev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Data.Entity;
using System.Diagnostics;

namespace UstamOdev.Controllers
{
    public class UstaController : Controller
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
        public ActionResult GirisYap(ustalar u)
        {
            if (!ModelState.IsValid)
            {
                var bulunan_usta = db.ustalar.Where(usta => usta.usta_email.Equals(u.usta_email) && usta.usta_sifre.Equals(u.usta_sifre));
                if (bulunan_usta.Count() > 0)
                {


                    if (bulunan_usta.FirstOrDefault().usta_onay.Equals("H"))
                    {
                        string error = "Üyeliğiniz henüz onaylanmamıştır.";
                        return RedirectToAction("GirisYap", new { err = error });
                    }

                   

                    Session["id"] = bulunan_usta.FirstOrDefault().usta_id;
                    Session["isim"] = bulunan_usta.FirstOrDefault().usta_isim_soyisim;
                    Session["email"] = bulunan_usta.FirstOrDefault().usta_email;

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

        public ActionResult KayıtOl()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KayıtOl(ustalar u, string usta_il)
        {

            if (!ModelState.IsValid)
            {
               
                var kontrol = db.ustalar.FirstOrDefault(usta => usta.usta_email == u.usta_email);
                if (kontrol == null && u.sertifika != null && u.sertifika.ContentLength > 0)
                {
                    var dosyaAdi = Path.GetFileNameWithoutExtension(u.sertifika.FileName);
                    var uzanti = Path.GetExtension(u.sertifika.FileName);

                    dosyaAdi = "" + DateTime.Now.ToString("yymmssfff") + uzanti;
                    u.usta_sertifika = "~/Content/Sertifikalar/" + dosyaAdi;

                    var dosyaYolu = Path.Combine(Server.MapPath("~/Content/Sertifikalar/"), dosyaAdi);
                    u.sertifika.SaveAs(dosyaYolu);

              
                    u.usta_onay = "H";
                    u.usta_il = usta_il;
                    db.ustalar.Add(u);
                    db.SaveChanges();
                    ModelState.Clear();

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

        public ActionResult IsEkle()
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsEkle(is_ilanlari i, string is_tur)
        {
            if (ModelState.IsValid)
            {
                i.usta_id = (int)Session["id"];
                i.is_tur = is_tur;
                db.is_ilanlari.Add(i);
                db.SaveChanges();

                ModelState.Clear();
                return RedirectToAction("Aktifler", "Usta");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Aktifler()
        {
            if (Session["id"] != null)
            {
                int id = (int)Session["id"];
                var aktifler = db.is_ilanlari.Where(i => i.usta_id == id);
                return View(aktifler);
            }
            else
            {
                return RedirectToAction("GirisYap");
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

                return RedirectToAction("Aktifler", "Usta");
            }
            else
            {
                return View(i);
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
        public ActionResult IsSil(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                is_ilanlari i = db.is_ilanlari.Find(id);
                db.is_ilanlari.Remove(i);
                db.SaveChanges();
                return RedirectToAction("Aktifler");
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
        public ActionResult Onaylilar()
        {
            if (Session["id"] != null)
            {
                int id = (int)Session["id"];
                var teklifalanisler = db.onayli_isler.AsEnumerable().Where(t => t.onay_durum == "Beklemede").Select(t => t.is_id).ToList();

                var islerim = db.is_ilanlari.Where(i => i.usta_id == id && teklifalanisler.Contains(i.is_id));
                return View(islerim);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }
        }
        public ActionResult Tamamlanmamislar()
        {
            if (Session["id"] != null)
            {
                int id = (int)Session["id"];
                var teklifalanisler = db.onayli_isler.AsEnumerable().Where(t => t.onay_durum == "Onaylı").Select(t => t.is_id).ToList();

                var islerim = db.is_ilanlari.Where(i => i.usta_id == id && teklifalanisler.Contains(i.is_id));
                return View(islerim);
            }
            else
            {
                return RedirectToAction("GirisYap");
            }
        }
        public ActionResult TamamlanmamisMusteriler(int id)
        {
            if (Session["id"] != null)
            {
                UstamOdevEntities entities = new UstamOdevEntities();
                int ustaid = (int)Session["id"];

      
                var teklifler = from onayli_isler in entities.onayli_isler.Where(x => x.is_ilanlari.usta_id == ustaid && x.is_ilanlari.is_id == id && x.onay_durum == "Onaylı") select onayli_isler;

                return View(teklifler);
            }
            else return RedirectToAction("GirisYap"); 
        }

        public ActionResult OnayGoruntule(int id)
        {
            if (Session["id"] != null)
            {
                UstamOdevEntities entities = new UstamOdevEntities();
                int ustaid = (int)Session["id"];

           
                var teklifler = from onayli_isler in entities.onayli_isler.Where(x => x.is_ilanlari.usta_id == ustaid && x.is_ilanlari.is_id == id && x.onay_durum =="Beklemede") select onayli_isler;

                return View(teklifler);
            }
            else return RedirectToAction("GirisYap");
        }
        public ActionResult MusteriProfil(int? musteri_id)
        {
            if (musteri_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                musteriler m = db.musteriler.Find(musteri_id);

                if (m == null)
                {
                    return HttpNotFound();
                }
                return View(m);
            }
        }
        public ActionResult TamamlanmamisProfil(int? musteri_id)
        {
            if (musteri_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                musteriler m = db.musteriler.Find(musteri_id);

                if (m == null)
                {
                    return HttpNotFound();
                }
                return View(m);
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
                ustalar u = db.ustalar.Find(id);

                if (u == null)
                {
                    return HttpNotFound();
                }
                return View(u);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilDuzenle(ustalar u, string usta_il)
        {

            u.usta_id = (int)Session["id"];
            if (ModelState.IsValid)
            {
                u.usta_onay = "E";
                u.usta_il = usta_il;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Profil", "Usta");
            }
            else
            {
                return View(u);
            }
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
                ustalar u = db.ustalar.Find(id);

                if (u == null)
                {
                    return HttpNotFound();
                }
                return View(u);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Onay(int is_id, int musteri_id, int onay_id)
        {
            int usta = (int)Session["id"];
            if(usta != null)
            {
            var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri_id && t.onay_id == onay_id).FirstOrDefault();
            teklifler.onay_id = onay_id;
            teklifler.onay_durum = "Onaylı";
            teklifler.musteri_id = musteri_id;
            teklifler.is_id = is_id;
                db.Entry(teklifler).State = EntityState.Modified;
                db.SaveChanges();
            return RedirectToAction("Onaylilar");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Onay(int? musteri_id, int is_id)
        {
            if (musteri_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int usta = (int)Session["id"];
                var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri_id && t.is_id == is_id).FirstOrDefault();

                if (teklifler == null)
                {
                    return HttpNotFound();
                }
                return View(teklifler);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Red(int musteri_id, int onay_id)
        {
            int usta = (int)Session["id"];
            if (usta != null)
            {

                var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri_id && t.onay_id == onay_id).FirstOrDefault();
                db.onayli_isler.Remove(teklifler);
                db.SaveChanges();
                return RedirectToAction("Onaylilar");
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Red(int? musteri_id, int is_id)
        {
            if (musteri_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int usta = (int)Session["id"];
                var teklifler = db.onayli_isler.Where(t => t.musteri_id == musteri_id && t.is_id == is_id).FirstOrDefault();

                if (teklifler == null)
                {
                    return HttpNotFound();
                }
                return View(teklifler);
            }
        }
    }
}