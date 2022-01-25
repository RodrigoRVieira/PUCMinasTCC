using System;
using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Mensalidade : Cadastro
    {
        [Required]
        public short MesReferencia { get; set; }

        [Required]
        public DateTime DataVencimento { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Valor { get; set; }
    }
}
