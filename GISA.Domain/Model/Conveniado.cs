using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Conveniado : Cadastro
    {
        [Required]
        [StringLength(128, ErrorMessage = "Nome inválido", MinimumLength = 32)]
        public string Nome { get; set; }

        public Email Email { get; set; }

        public Endereco Endereco { get; set; }

        public TipoConveniado TipoConveniado { get; set; }
    }
}
