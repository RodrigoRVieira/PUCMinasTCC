using GISA.Domain.Model;

using System.Collections.Generic;

namespace SAF.Repository
{
    public abstract class BaseConfiguration
    {
        private static readonly Bogus.DataSets.Address addressGenerator = new Bogus.DataSets.Address("pt_BR");

        protected List<object> Emails { get; } = new List<object>();

        protected List<object> Enderecos { get; } = new List<object>();

        public BaseConfiguration(TipoPessoa tipoPessoa)
        {
            for (long pessoaId = 1; pessoaId < 11; pessoaId += 1)
            {
                Emails.Add(new { AssociadoId = pessoaId * 1000, PrestadorId = pessoaId * 1000, Endereco = $"{tipoPessoa.ToString().ToLower()}-{pessoaId * 1000}@boasaude.com.br" });
                Enderecos.Add(new
                {
                    AssociadoId = pessoaId * 1000,
                    PrestadorId = pessoaId * 1000,
                    Bairro = addressGenerator.County(),
                    Logradouro = addressGenerator.StreetAddress(),
                    Numero = int.Parse(addressGenerator.BuildingNumber()),
                    CEP = addressGenerator.ZipCode().Replace("-", ""),
                    Cidade = addressGenerator.City(),
                    Estado = addressGenerator.StateAbbr()
                });
            }
        }
    }
}
