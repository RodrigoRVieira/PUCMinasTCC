using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Plano : Cadastro
    {
        [Required]
        [StringLength(128, ErrorMessage = "Nome comercial inválido", MinimumLength = 16)]
        public string NomeComercial { get; set; }

        public CategoriaPlano CategoriaPlano { get; set; }

        /// <summary>
        /// Contempla Plano Odontológico?
        /// </summary>
        [Required]
        public bool PlanoOdontologico { get; set; }

        /// <summary>
        /// Número de registro do Plano junto à Agência Nacional de Saúde Suplementar
        /// </summary>
        [Required]
        [StringLength(8, ErrorMessage = "Número ANS inválido", MinimumLength = 6)]
        public string NumeroANS { get; set; }
    }
}
