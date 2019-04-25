using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Drones.Entities
{
    public class UAV
    {
        [Key]
        public int DroneId { get; set; }

        public string DroneStatus { get; set; }

        public string  DroneType { get; set; }
        public ICollection<Farm> FarmId { get; set; }

        public Farm Farms { get; set; }
    }
}