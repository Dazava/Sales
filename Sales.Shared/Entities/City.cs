using System.ComponentModel.DataAnnotations;

namespace Sales.Shared.Entities
{
    public class City
    {
        public int Id { get; set; }

        [Display(Name = "Ciudad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100, ErrorMessage = "El campo {0} debe de tener másimo {1} caractéres")]
        public string Name { get; set; } = null!;

        public int StateId { get; set; }

        public State? State { get; set; }

        public ICollection<User>? Users { get; set; }

    }
}
