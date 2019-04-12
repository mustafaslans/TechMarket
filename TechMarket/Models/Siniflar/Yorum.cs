using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechMarket.Models.Siniflar
{
    public class Yorum
    {
        [Key]
        public int YorumId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string Aciklama { get; set; }

    }
}