﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ParkyApi.Models
{
    public class NationalPark
    {   
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string State { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Created { get; set; }
        public DateTime Estblished { get; set; }
    }
}
