using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace OrderWatch.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo Nome deve ter entre 2 e 100 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Description é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo Descrição deve ter entre 2 e 100 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O campo Preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O campo Preço deve ser maior que zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "O campo Image é obrigatório.")]
        public string Image { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        // Relacionamento para fazer muitos para muitos
        public List<User> Users { get; } = [];
    }
}
