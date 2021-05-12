﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SideXC.WebUI.Models.Security;

namespace SideXC.WebUI.Models.Inventory
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }
        public virtual Material Material { get; set; }
        public virtual Location Location { get; set; }
        [Required(ErrorMessage = "Required.")]
        public double QuantityAvailable { get; set; }
        public DateTime Created { get; set; }
        public virtual ClientUser CreatedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual ClientUser ModifiedBy { get; set; }
    }
}
