using System.Collections.Generic;

namespace GISA.Domain.Model
{
    public class Prestador : Pessoa
    {
        public ICollection<Especialidade> Especialidades { get; set; }

        public QualificacaoPrestador Qualificacao { get; set; }
    }
}
