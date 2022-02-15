using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MicroClimateControllSystem.Models
{
    [Table("SensorData")]  
    public class SensorData
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Display(Name = "Температура")]
        public int Temp { get; set; }

        [Display(Name = "Влажность")]
        public int Humidity { get; set; }

        [Display(Name = "Загрязнение воздуха")]
        public bool Gas { get; set; }

        [Display(Name = "Дата/Время")]
        public DateTime DateTime { get; set; }

        [ForeignKey("Sensor")]
        public int SensorID { get; set; }

        //
        [InverseProperty("SensorData")]
        public Sensor Sensor { get; set; }
    }
}