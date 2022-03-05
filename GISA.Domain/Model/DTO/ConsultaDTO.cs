using System;

namespace GISA.Domain.Model.DTO
{
    public class ConsultaDTO
    {
        public long AssociadoId { get; set; }

        public long EspecialidadeId { get; set; }

        public long PrestadorId { get; set; }

        public DateTime Data { get; set; }

        public long CriadoPor { get; set; }
    }
}
