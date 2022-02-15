using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroClimateControllSystem.Models
{
    [Table("Auth")] 
    public class Auth
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 30 символов")]
        public string Login { get; set;}

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 30 символов")]
        public string Password { get; set;}

    }
}