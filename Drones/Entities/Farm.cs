using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Drones.Entities
{
    public class Farm
    {
        private int farmId;

        public int FarmId
        {
            get { return farmId; }
            set { farmId = value; }
        }

        public int FarmSize { get; set; }

        public string PostalCode { get; set; }

        public string StreetAddress { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Suburb { get; set; }
        public ICollection<UAV> UAVId { get; set; }

        public Farm()
        {

        }

        public Farm( Farm farm)
        {
            this.FarmId = farm.farmId;
            this.FarmSize = farm.FarmSize;
            this.Latitude = farm.Latitude;
            this.Longitude = farm.Longitude;
          
        }

    }
}