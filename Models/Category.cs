using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopsApi.Models
{
    //tem que ficar de fora da classe
    [Table("categories")]

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres!")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres!")]
        public string Title { get; set; }
    }
}