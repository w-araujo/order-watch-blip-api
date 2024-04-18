using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OrderWatch.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo Nome deve ter entre 2 e 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "A Senha deve ter pelo menos 3 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório.")]
        [DataType(DataType.Date, ErrorMessage = "O campo Data de Nascimento deve ser uma data válida.")]
        public DateTime Birthdate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        // Relacionamento para fazer muitos para muitos
        public List<Product> Products { get; } = [];
    }
}
