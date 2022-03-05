using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Especialidade : Cadastro
    {
        [Required]
        [StringLength(64, ErrorMessage = "Especialidade inválida", MinimumLength = 8)]
        public string Nome { get; set; }

        public ICollection<Prestador> Prestadores { get; set; }

        public Especialidade() { }

        public Especialidade(long id, string nome, long criadoPor, DateTime criadoEm)
        {
            this.Id = id;
            this.Nome = nome;
            this.CriadoPor = criadoPor;
            this.CriadoEm = criadoEm;
        }
    }
}
