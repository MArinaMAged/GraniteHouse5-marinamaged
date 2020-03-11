﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse5.Models
{
    public class Products
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public String Image { get; set; }
        public String ShadeColor { get; set; }

        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Product Type")]
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }


        [Display(Name = "Special Tag")]
        public int SpecialTagsId { get; set; }


        [Display(Name = "Special Tag")]
        [ForeignKey("SpecialTagsId")]
        public virtual SpecialTags SpecialTags { get; set; }

    }
}
