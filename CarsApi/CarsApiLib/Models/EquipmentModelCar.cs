using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib.Models
{
    public class EquipmentModelCar
    {
        public string Name{ get;set;}
        public string Date{ get;set;}
        public string Engine{ get;set;}
        public string Body{ get;set;}
        public string Grade{ get;set;}
        public string Transmission{ get;set;}
        public string GearShift{ get;set; }
        public string DriverPosition{ get;set; }
        public string NumOfDoor{ get;set; }
        public string Destination1{ get;set; }
        public string Destination2{ get;set; }

        public string ComponentCod { get; set; }

        public ModelCar ModelCar { get; set; }
    }
}
