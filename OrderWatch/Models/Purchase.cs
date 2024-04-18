using System.ComponentModel.DataAnnotations;

namespace OrderWatch.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo UserId é obrigatório.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O campo ProductId é obrigatório.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "O campo Status é obrigatório.")]
        public String Status {  get; set; }

        [Required(ErrorMessage = "O campo IsPaid é obrigatório.")]
        public Boolean IsPaid { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

    }
}
