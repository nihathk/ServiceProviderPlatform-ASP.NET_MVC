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

    public partial class is_ilanlari
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public is_ilanlari()
        {
            this.onayli_isler = new HashSet<onayli_isler>();
        }

        public int is_id { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "�� T�r�")]
        public string is_tur { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "�� A��klamas�")]
        public string is_aciklama { get; set; }
        [Required(ErrorMessage = "Alan bo� ge�ilemez")]
        [Display(Name = "Ortalama Fiyat")]
        [Range(0, int.MaxValue, ErrorMessage = "L�tfen ge�erli bir de�er giriniz.")]
        public string is_ortalama_fiyat { get; set; }
        public Nullable<int> usta_id { get; set; }

        public virtual ustalar ustalar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<onayli_isler> onayli_isler { get; set; }
    }
}
