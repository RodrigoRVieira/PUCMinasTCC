using System;
using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public abstract class Pessoa : Cadastro
    {
        [Required]
        [StringLength(128, ErrorMessage = "Nome inválido", MinimumLength = 32)]
        public string Nome { get; set; }

        public string NomeSocial { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "CPF inválido", MinimumLength = 11)]
        public string CPF { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "RG inválido", MinimumLength = 5)]
        public string RG { get; set; }

        public Email Email { get; set; }

        public GeneroPessoa Genero { get; set; }

        public Endereco Endereco { get; set; }

        public DateTime DataNascimento { get; set; }

        public TipoPessoa TipoPessoa { get; set; }
    }
}