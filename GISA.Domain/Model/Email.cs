using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Email
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Endereco { get; set; }

        public Email(string endereco)
        {
            this.Endereco = endereco;
        }
    }
}
