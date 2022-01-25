using GISA.Domain.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using SAF.Repository;

namespace IntegrationTests
{
    [SetUpFixture]
    public abstract class RepositoryTests
    {
        private IConfiguration _config;

        protected SAFDbContext context;

        protected IAssociadoRepository _associadoRepository;
        protected IEspecialidadeRepository _especialidadeRepository;
        protected IProcedimentoRepository _procedimentoRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(Configuration);

            context = new SAFDbContext(new DbContextOptionsBuilder<SAFDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("ConnectionString"))
                .Options);

            context.Database.EnsureCreated();

            _associadoRepository = new AssociadoRepository(context);
            _especialidadeRepository = new EspecialidadeRepository(context);
            _procedimentoRepository = new ProcedimentoRepository(context);
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            context.Database.EnsureDeleted();
        }

        internal IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"testsettings.json", optional: false);

                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}
