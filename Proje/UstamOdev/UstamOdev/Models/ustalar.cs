//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UstamOdev.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class ustalar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ustalar()
        {
            this.is_ilanlari = new HashSet<is_ilanlari>();
        }

        public int usta_id { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "Telefon")]
        [DataType(DataType.PhoneNumber)]
        public string usta_tel { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "Sertifika")]
        public string usta_sertifika { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "E-Posta")]
        [DataType(DataType.EmailAddress)]
        public string usta_email { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "�ifre")]
        [DataType(DataType.Password)]
        public string usta_sifre { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "�sim Soyisim")]
        public string usta_isim_soyisim { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "�nvan")]
        public string usta_unvan { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "Onay")]
        public string usta_onay { get; set; }
        public HttpPostedFileBase sertifika { get; set; }

        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "�l")]
        public string usta_il { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<is_ilanlari> is_ilanlari { get; set; }
    }
}
