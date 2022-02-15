using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroClimateControllSystem.Models
{
    [Table("Sensor")] 
    public class Sensor
    {
        [Key]
        public int SensorID { get; set; }

        [Display(Name = "Дата установки")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Расположение")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 30 символов")]
        public string Location { get; set; }

        [Display(Name = "Температура")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Range(-100, 100, ErrorMessage = "Недопустимое значение! Диапазон значений [-100 - +100]")]
        public int NormalTemp { get; set; }

        [Display(Name = "Влажность")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Range(0, 100, ErrorMessage = "Недопустимое значение! Диапазон значений [0-100]")]
        public int NormalHumidity{ get; set;}

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Поле должно быть установлено")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина пароля должна быть от 4 до 50 символов")]
        [UIHint("Password")]
        public string Password { get; set; }

 
        public List<SensorData> SensorData { get; set; }
    }
}