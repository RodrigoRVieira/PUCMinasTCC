using GISA.Domain.Model.DTO;

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISA.Domain.Model
{
    public class Consulta : Cadastro
    {
        [ForeignKey("AssociadoId")]
        public long AssociadoId { get; set; }

        [ForeignKey("EspecialidadeId")]
        public long EspecialidadeId { get; set; }

        [ForeignKey("PrestadorId")]
        public long PrestadorId { get; set; }

        public DateTime Data { get; set; }

        public StatusConsulta Status { get; set; }

        public Consulta() { }

        public Consulta(ConsultaDTO consulta)
        {
            this.AssociadoId = consulta.AssociadoId;
            this.EspecialidadeId = consulta.EspecialidadeId;
            this.PrestadorId = consulta.PrestadorId;

            this.Data = consulta.Data;
            this.CriadoPor = this.AssociadoId;
        }
    }
}
