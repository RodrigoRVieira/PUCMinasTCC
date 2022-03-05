using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Endereco
    {
        [Required]
        [StringLength(128, ErrorMessage = "Logradouro inválido", MinimumLength = 8)]
        public string Logradouro { get; set; }

        [StringLength(128, ErrorMessage = "Complemento inválido", MinimumLength = 0)]
        public string Complemento { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Logradouro inválido", MinimumLength = 8)]
        public string Bairro { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "CEP inválido", MinimumLength = 8)]
        public string CEP { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Cidade inválida", MinimumLength = 4)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Estado inválido", MinimumLength = 8)]
        public string Estado { get; set; }

        public Endereco()
        {

        }
        public Endereco(string logradouro, string complemento, int numero, string cep, string cidade, string estado)
        {
            this.Logradouro = logradouro;
            this.Complemento = complemento;
            this.Numero = numero;
            this.CEP = cep;
            this.Cidade = cidade;
            this.Estado = estado;
        }
    }
}
