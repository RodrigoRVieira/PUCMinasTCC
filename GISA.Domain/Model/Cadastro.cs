using System;
using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public abstract class Cadastro
    {
        public long Id { get; set; }

        [Required]
        public long CriadoPor { get; set; }

        [Required]
        public DateTime CriadoEm { get; set; }

        public long? ModificadoPor { get; set; }

        public DateTime? ModificadoEm { get; set; }
    }
}
