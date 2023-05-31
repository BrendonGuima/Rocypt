using System.ComponentModel.DataAnnotations;

namespace Rocypt.Models
{
    public class PasswordModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Password é nescessario ter um nome.")]
        public string Name { get; set; }
        public Guid? GrupoId { get; set; }
        public GrupoModel? grupo { get; set; }

    }
}
