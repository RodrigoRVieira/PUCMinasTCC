using GISA.Domain.Model;

using NUnit.Framework;

using System;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [TestFixture]
    public class SAFRepositoryTests : RepositoryTests
    {
        private static readonly Bogus.DataSets.Name nameGenerator = new Bogus.DataSets.Name("pt_BR");
        private static readonly Bogus.DataSets.Address addressGenerator = new Bogus.DataSets.Address("pt_BR");

        #region Associado

        [TestCase]
        public async Task AssociadoIncluir()
        {
            await _associadoRepository.Incluir(
                Associado("12346578909", new Random(1).Next(999999999).ToString()));

            Assert.IsTrue(context.SaveChanges() > 0);
        }

        [Test]
        public async Task AssociadoAlterar()
        {
            var associado = await _associadoRepository.Incluir(
                Associado("99988877765", new Random(2).Next(999999999).ToString()));

            context.SaveChanges();

            associado.CPF = "88877766655";
            associado = _associadoRepository.Alterar(associado);

            Assert.IsTrue(associado.CPF == "88877766655");
        }

        [Test]
        public async Task AssociadoExcluir()
        {
            var associado = await _associadoRepository.Incluir(
                Associado("77766655544", new Random(3).Next(999999999).ToString()));

            context.SaveChanges();

            _associadoRepository.Excluir(associado);

            context.SaveChanges();

            associado = await _associadoRepository.RecuperarPorCPF("77766655544");

            Assert.IsTrue(associado == null);
        }

        [Test]
        public async Task AssociadoRecuperarPorCPF()
        {
            var associado = await _associadoRepository.Incluir(
                Associado("66655544433", new Random(4).Next(999999999).ToString()));

            context.SaveChanges();

            associado = await _associadoRepository.RecuperarPorCPF("66655544433");

            Assert.IsTrue(associado != null);
        }

        #endregion

        #region Especialidade

        [TestCase]
        public void EspecialidadeRecuperarTodas()
        {
            var especialidades = _especialidadeRepository.RecuperarTodas();

            Assert.IsTrue(especialidades != null);
        }

        #endregion

        #region Procedimento

        [TestCase]
        public void ProcedimentoRecuperarTodos()
        {
            var procedimentos = _procedimentoRepository.RecuperarTodos();

            Assert.IsTrue(procedimentos != null);
        }

        #endregion

        #region Helpers

        internal static Associado Associado(string cpf, string numeroCarteirinha)
        {
            return new Associado()
            {
                CPF = cpf,
                RG = "999999999",
                CriadoEm = DateTime.UtcNow,
                CriadoPor = 1,
                DataAdesao = DateTime.UtcNow,
                DataNascimento = DateTime.UtcNow.AddYears(-30),
                Email = new Email("email@email.com"),
                Endereco = new Endereco(addressGenerator.StreetAddress(),
                "",
                int.Parse(addressGenerator.BuildingNumber()),
                addressGenerator.ZipCode().Replace("-", ""),
                addressGenerator.City(),
                addressGenerator.StateAbbr()),
                Genero = GeneroPessoa.Masculino,
                NumeroCarteirinha = numeroCarteirinha,
                Nome = $"{nameGenerator.FirstName()} {nameGenerator.LastName()}",
                StatusPlano = StatusPlano.Suspenso
            };
        }

        #endregion
    }
}