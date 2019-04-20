using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Drones.Entities
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Farm Farms { get; set; }
        public string UserId { get; set; }
    }
}