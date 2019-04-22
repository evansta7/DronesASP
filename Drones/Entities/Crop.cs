using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Drones.Entities
{
    public class Crop
    {
        [Key]
        public int CropId { get; set; }

        public string CropDescription { get; set; }

        public string CropName { get; set; }

        public int IdealClimateLowerRange { get; set; }

        public int IdealClimateUpperRange { get; set; }

        public string IdealSoil { get; set; }

        public string MostCommonPest { get; set; }

        public string SoilDescription { get; set; }

        public Farm Farms { get; set; }
    }
}