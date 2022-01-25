using System;
using System.ComponentModel.DataAnnotations;

namespace GISA.Domain.Model
{
    public class Procedimento : Cadastro
    {
        /// <summary>
        /// Código do procedimento padronizado pela ANS
        /// http://www.ans.gov.br/images/stories/Legislacao/in/anexo_in34_dides.pdf
        /// </summary>
        [Required]
        [StringLength(16, ErrorMessage = "Código inválido", MinimumLength = 8)]
        public string CodigoTUSS { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Procedimento inválido", MinimumLength = 16)]
        public string Nome { get; set; }

        [Required]
        public SetorProcedimento Setor { get; set; }

        public Procedimento(long id, string codigoTUSS, string nome, SetorProcedimento setor, long criadoPor, DateTime criadoEm)
        {
            this.Id = id;
            this.CodigoTUSS = codigoTUSS;
            this.Nome = nome;
            this.Setor = setor;

            this.CriadoPor = criadoPor;
            this.CriadoEm = criadoEm;
        }
    }
}
