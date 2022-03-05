using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Associado : Pessoa
    {
        /// <summary>
        /// Representa o Identificador do Titular do Plano
        /// </summary>
        public long? IdentificadorTitular { get; set; }

        [StringLength(16, ErrorMessage = "Número de carteirinha inválido", MinimumLength = 16)]
        public string NumeroCarteirinha { get; set; }

        public List<Mensalidade> Mensalidades { get; set; }

        [Required]
        public DateTime DataAdesao { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public StatusPlano StatusPlano { get; set; }

        [Required]
        public Plano Plano { get; set; }

        [Required]
        public TipoContratacao TipoPlano { get; set; }

        public string UltimaLocalizacao { get; set; }
    }
}
