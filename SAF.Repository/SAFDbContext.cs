using GISA.Domain.Model;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

namespace SAF.Repository
{
    public class SAFDbContext : DbContext
    {
        private static readonly Bogus.DataSets.Name nameGenerator = new Bogus.DataSets.Name("pt_BR");
        private static readonly Bogus.DataSets.Address addressGenerator = new Bogus.DataSets.Address("pt_BR");

        public SAFDbContext(DbContextOptions<SAFDbContext> options)
            : base(options) { }

        public DbSet<Associado> Associados { get; set; }

        public DbSet<Consulta> Consultas { get; set; }

        public DbSet<Conveniado> Conveniados { get; set; }

        public DbSet<Especialidade> Especialidades { get; set; }

        public DbSet<Mensalidade> Mensalidades { get; set; }

        public DbSet<Prestador> Prestadores { get; set; }

        public DbSet<Procedimento> Procedimentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Associado>().ToTable(nameof(Associado));
            modelBuilder.Entity<Consulta>().ToTable(nameof(Consulta));
            modelBuilder.Entity<Conveniado>().ToTable(nameof(Conveniado));
            modelBuilder.Entity<Especialidade>().ToTable(nameof(Especialidade));
            modelBuilder.Entity<Mensalidade>().ToTable(nameof(Mensalidade));
            modelBuilder.Entity<Prestador>().ToTable(nameof(Prestador));
            modelBuilder.Entity<Procedimento>().ToTable(nameof(Procedimento));

            // Garante a persistência dos enumeradores como String
            modelBuilder.Entity<Associado>().Property(e => e.Genero).HasConversion<string>();
            modelBuilder.Entity<Associado>().Property(e => e.StatusPlano).HasConversion<string>();
            modelBuilder.Entity<Associado>().Property(e => e.TipoPessoa).HasConversion<string>();
            modelBuilder.Entity<Associado>().Property(e => e.TipoPlano).HasConversion<string>();

            modelBuilder.Entity<Consulta>().Property(e => e.Status).HasConversion<string>();
            modelBuilder.Entity<Conveniado>().Property(e => e.TipoConveniado).HasConversion<string>();
            modelBuilder.Entity<Plano>().Property(e => e.CategoriaPlano).HasConversion<string>();
            modelBuilder.Entity<Prestador>().Property(e => e.Qualificacao).HasConversion<string>();
            modelBuilder.Entity<Procedimento>().Property(e => e.Setor).HasConversion<string>();

            // Configura os objetos endereço/email como propriedades
            modelBuilder.ApplyConfiguration(new AssociadoConfiguration());
            modelBuilder.ApplyConfiguration(new ConveniadoConfiguration());
            modelBuilder.ApplyConfiguration(new PrestadorConfiguration());

            #region Consulta

            modelBuilder.Entity<Consulta>(e =>
            {
                e.HasIndex("PrestadorId", "Data").IsUnique();
            });

            #endregion

            #region Associado e Plano

            modelBuilder.Entity<Associado>(e =>
            {
                e.HasIndex(e => e.CPF).IsUnique();
                e.HasIndex(e => e.NumeroCarteirinha).IsUnique();
            });

            Random r = new Random();

            modelBuilder.Entity<Plano>(p =>
            {
                // Seed Planos
                p.HasData(
                    new
                    {
                        Id = (long)1,
                        CategoriaPlano = CategoriaPlano.Enfermaria,
                        CriadoEm = DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        CriadoPor = (long)1,
                        NomeComercial = "ENFERMARIA PADRÃO",
                        NumeroANS = r.Next(999999).ToString().PadLeft(6, '0'),
                        PlanoOdontologico = false
                    },
                    new
                    {
                        Id = (long)2,
                        CategoriaPlano = CategoriaPlano.Enfermaria,
                        CriadoEm = DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        CriadoPor = (long)1,
                        NomeComercial = "ENFERMARIA PADRÃO COM ODONTO",
                        NumeroANS = r.Next(999999).ToString().PadLeft(6, '0'),
                        PlanoOdontologico = true
                    },
                    new
                    {
                        Id = (long)3,
                        CategoriaPlano = CategoriaPlano.Apartamento,
                        CriadoEm = DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        CriadoPor = (long)1,
                        NomeComercial = "APARTAMENTO PADRÃO",
                        NumeroANS = r.Next(999999).ToString().PadLeft(6, '0'),
                        PlanoOdontologico = false
                    },
                    new
                    {
                        Id = (long)4,
                        CategoriaPlano = CategoriaPlano.Apartamento,
                        CriadoEm = DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        CriadoPor = (long)1,
                        NomeComercial = "APARTAMENTO PADRÃO COM ODONTO",
                        NumeroANS = r.Next(999999).ToString().PadLeft(6, '0'),
                        PlanoOdontologico = true
                    },
                    new
                    {
                        Id = (long)5,
                        CategoriaPlano = CategoriaPlano.VIP,
                        CriadoEm = DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        CriadoPor = (long)1,
                        NomeComercial = "ALTO PADRÃO - VIP ODONTO",
                        NumeroANS = r.Next(999999).ToString().PadLeft(6, '0'),
                        PlanoOdontologico = true
                    });
            });

            List<object> listaAssociados = new List<object>();

            long idPlano = 0;

            for (long pessoaId = 1; pessoaId < 11; pessoaId++)
            {
                int randomGender = r.Next(2);

                idPlano = ++idPlano > 5 ? idPlano -= 5 : idPlano;

                listaAssociados.Add(new
                {
                    PlanoId = idPlano,
                    Id = pessoaId * 1000,
                    CPF = $"{r.Next(999999999).ToString().PadLeft(11, '0')}",
                    CriadoEm = DateTime.UtcNow,
                    CriadoPor = (long)1,
                    DataAdesao = DateTime.UtcNow.AddDays(-90),
                    DataNascimento = DateTime.UtcNow.AddYears(-35),
                    Genero = randomGender == 0 ? GeneroPessoa.Masculino : GeneroPessoa.Feminino,
                    Nome = $"{nameGenerator.FirstName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)} {nameGenerator.LastName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)}",
                    NumeroCarteirinha = $"{r.Next(99999999).ToString().PadLeft(8, '0')}{r.Next(99999999).ToString().PadLeft(8, '0')}",
                    RG = $"{r.Next(999999999).ToString().PadLeft(9, '0')}",
                    StatusPlano = StatusPlano.Ativo,
                    TipoPessoa = TipoPessoa.Associado,
                    TipoPlano = randomGender == 0 ? TipoContratacao.Individual : TipoContratacao.Empresarial
                });
            }

            modelBuilder.Entity<Associado>(p =>
            {
                // Seed Associados
                p.HasData(listaAssociados.ToArray());
            });

            #endregion

            #region Procedimento

            modelBuilder.Entity<Procedimento>(p =>
            {
                // Seed Procedimentos
                p.HasData(
                        new Procedimento(1, "00000039", "ANATOMO PATOLOGICO (BIOPSIA DE ORGAOS - PATHOS)", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(2, "00000065", "USG ABDOME TOTAL E PROSTATA", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(3, "00000065", "USG ABDOME TOTAL E PELVE", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(4, "40808033", "MAMOGRAFIA BILATERAL", SetorProcedimento.Mamografia, 1, DateTime.UtcNow),
                        new Procedimento(5, "40808041", "MAMOGRAFIA DIGITALIZADA", SetorProcedimento.Mamografia, 1, DateTime.UtcNow),
                        new Procedimento(6, "40808092", "CORE BIOPSIA GUIADA POR UltraSom", SetorProcedimento.Punção, 1, DateTime.UtcNow),
                        new Procedimento(7, "40808092", "BIOPSIA PERCUTANEA DE FRAGMENTO MAMARIO", SetorProcedimento.Punção, 1, DateTime.UtcNow),
                        new Procedimento(8, "40808130", "DENSITOMETRIA OSSEA COLUNA LOMBAR E FEMUR", SetorProcedimento.Densitometria, 1, DateTime.UtcNow),
                        new Procedimento(9, "40808149", "DENSITOMETRIA OSSEA CORPO INTEIRO", SetorProcedimento.Densitometria, 1, DateTime.UtcNow),
                        new Procedimento(10, "40809099", "PUNCAO ASPIRATIVA ORIENTADA POR USG ( PAAF )", SetorProcedimento.Punção, 1, DateTime.UtcNow),
                        new Procedimento(11, "40809099", "PUNCAO OU BIOPSIA MAMARIA PERCUTANEA POR AGULHA FINA", SetorProcedimento.Punção, 1, DateTime.UtcNow),
                        new Procedimento(12, "40901114", "USG MAMAS", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(13, "40901122", "USG ABDOME TOTAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(14, "40901122", "USG ABDOME TOTAL COM PROVA DE BOYDEN", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(15, "40901130", "USG ABDOME SUPERIOR", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(16, "40901130", "USG HIPOCONDRIO DIREITO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(17, "40901181", "USG PELVICA", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(18, "40901203", "USG BOLSA ESCROTAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(19, "40901203", "USG ORBITA", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(20, "40901203", "USG ORGAOS SUPERFICIAIS", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(21, "40901203", "USG TIREOIDE", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(22, "40901211", "USG ANTEBRACO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(23, "40901211", "USG AXILAS", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(24, "40901211", "USG BRACO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(25, "40901211", "USG CERVICAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(26, "40901211", "USG COXA", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(27, "40901211", "USG ORGAOS E ESTRUTURAS SUPERFICIAIS", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(28, "40901211", "USG MUSCULO / TENDAO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(29, "40901211", "USG PAREDE ABDOMINAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(30, "40901211", "USG PERNA", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(31, "40901211", "USG REGIAO INGUINAL UNILATERAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(32, "40901211", "USG TENDÃO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(33, "40901220", "USG ARTICULACOES", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(34, "40901220", "USG COTOVELO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(35, "40901220", "USG JOELHO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(36, "40901220", "USG MAO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(37, "40901220", "USG OMBRO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(38, "40901220", "USG PE", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(39, "40901220", "USG PUNHO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(40, "40901220", "USG QUADRIL INFANTIL UNILATERAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(41, "40901220", "USG QUADRIL UNILATERAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(42, "40901220", "USG TORNOZELO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(43, "40901300", "USG TRANSVAGINAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(44, "40901319", "USG TRANSVAGINAL PARA CONTROLE DE OVULACAO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(45, "40901360", "DOPPLER COLOR DE CAROTIDAS OU VERTEBRAIS", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(46, "40901378", "DOPPLER COLOR DE SUBCLAVIAS E JUGULARES", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(47, "40901386", "DOPPLER COLOR DE ORGAOS OU ESTRUTURAS", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(48, "40901386", "DOPPLER COLOR TIREOIDE", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(49, "40901408", "DOPPLER COLOR DE AORTA", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(50, "40901408", "DOPPLER COLOR DE ILIACAS", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(51, "40901432", "DOPPLER COLOR DE VEIA CAVA", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(52, "40901459", "DOPPLER COLOR ARTERIAL DE MEMBRO SUPERIOR", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(53, "40901467", "DOPPLER COLOR VENOSO DE MEMBRO SUPERIOR", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(54, "40901475", "DOPPLER COLOR ARTERIAL DE MEMBRO INFERIOR", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(55, "40901483", "DOPPLER COLOR VENOSO DE MEMBRO INFERIOR", SetorProcedimento.Doppler, 1, DateTime.UtcNow),
                        new Procedimento(56, "40901750", "USG PROSTATA VIA ABDOMINAL", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(57, "40901769", "USG APARELHO URINARIO", SetorProcedimento.UltraSom, 1, DateTime.UtcNow),
                        new Procedimento(58, "41001010", "TC CRANIO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(59, "41001010", "TC ORBITAS S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(60, "41001010", "TC SELA TURCICA S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(61, "41001028", "TC MASTOIDES S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(62, "41001028", "TC OSSOS TEMPORAIS S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(63, "41001028", "TC OUVIDO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(64, "41001036", "TC FACE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(65, "41001036", "TC SEIOS DA FACE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(66, "41001044", "TC MANDIBULA OU ATM S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(67, "41001060", "TC FARINGE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(68, "41001060", "TC LARINGE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(69, "41001060", "TC PESCOCO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(70, "41001060", "TC TIREOIDE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(71, "41001079", "TC TORAX S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(72, "41001095", "TC ABDOME TOTAL (SUPERIOR + PELVE + RETROP) S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(73, "41001109", "TC ABDOME SUPERIOR S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(74, "41001117", "TC BACIA S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(75, "41001117", "TC PELVE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(76, "41001125", "TC COCCIX S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(77, "41001125", "TC COLUNA CERVICAL S/CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(78, "41001125", "TC COLUNA DORSAL S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(79, "41001125", "TC COLUNA LOMBAR S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(80, "41001125", "TC COLUNA SACRAL S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(81, "41001133", "TC COLUNA - SEGMENTO ADICIONAL", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(82, "41001141", "TC ARTICULACAO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(83, "41001141", "TC COTOVELO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(84, "41001141", "TC COXO FEMURAL S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(85, "41001141", "TC ESTERNO CLAVICULAR S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(86, "41001141", "TC JOELHO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(87, "41001141", "TC OMBRO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(88, "41001141", "TC TORNOZELO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(89, "41001141", "TC PUNHO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(90, "41001141", "TC TAGT (TC QUADRIL + TC JOELHO)", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(91, "41001141", "TC QUADRIL UNILATERAL S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(92, "41001141", "TC SACRO ILIACA S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(93, "41001150", "TC ANTEBRACO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(94, "41001150", "TC BRACO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(95, "41001150", "TC COXA S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(96, "41001150", "TC MAO S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(97, "41001150", "TC PE S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(98, "41001150", "TC PERNA S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(99, "41001150", "TC SEGMENTO APENDICULAR S/ CONTRASTE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(100, "41001192", "ESCANOMETRIA DE MMIIS BILATERAL POR TOMOGRAFIA", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(101, "41001206", "RECONSTRUCAO TRIDIMENCIONAL - TC - ACRESCENTAR O EXAME BASE", SetorProcedimento.Tomografia, 1, DateTime.UtcNow),
                        new Procedimento(102, "41101014", "RM CRANIO S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(103, "41101022", "RM HIPOFISE S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(104, "41101022", "RM SELA HIPOFISE S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(105, "41101022", "RM SELA TURCICA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(106, "41101030", "RM BASE DE CRANIO S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(107, "41101073", "RM ORBITA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(108, "41101081", "RM MASTOIDES S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(109, "41101090", "RM FACE S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(110, "41101103", "RM ARTICULACAO TEMPORO MANDIBULAR (BILATERAL) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(111, "41101111", "RM FARINGE, LARINGE, TRAQUEIA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(112, "41101111", "RM PESCOCO (CAROTIDAS) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(113, "41101120", "RM ARCOS COSTAIS S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(114, "41101120", "RM PAREDE TORACICA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(115, "41101120", "RM TORAX S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(116, "41101170", "RM ABDOMEN SUPERIOR S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(117, "41101170", "RM PAREDE ABDOMINAL S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(118, "41101189", "RM PELVE S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(119, "41101189", "RM REGIAO GLUTEA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(120, "41101189", "RM SINFISE PUBICA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(121, "41101200", "RM PENIS s/CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(122, "41101219", "RM BOLSA ESCROTAL", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(123, "41101227", "RM COLUNA CERVICAL S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(124, "41101227", "RM COLUNA LOMBO-SACRA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(125, "41101227", "RM COLUNA TORACICA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(126, "41101227", "RM REGIAO CERVICAL S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(127, "41101243", "RM PLEXO BRAQUIAL (UNILATERAL) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(128, "41101251", "RM ANTEBRACO S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(129, "41101251", "RM BRACO S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(130, "41101251", "RM SEGMENTO APENDICULAR (UNILATERAl) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(131, "41101260", "RM MAO S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(132, "41101278", "RM BACIA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(133, "41101278", "RM SACRO COCCIX S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(134, "41101278", "RM SACRO ILIACAS BILATERAL S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(135, "41101286", "RM COXA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(136, "41101294", "RM PERNA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(137, "41101308", "RM PE S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(138, "41101316", "RM ARTICULACAO", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(139, "41101316", "RM CLAVICULA S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(140, "41101316", "RM COTOVELO (UNILATERAL) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(141, "41101316", "RM COXO-FEMURAL UNILATERAL S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(142, "41101316", "RM JOELHO (UNILATERAL) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(143, "41101316", "RM OMBRO (UNILATERAL) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(144, "41101316", "RM PUNHO S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(145, "41101316", "RM QUADRIL UNILATERAL S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(146, "41101316", "RM TORNOZELO (CALCANEO) (UNILATERAL) S/ CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(147, "41101324", "ANGIO-RESSONANCIA ENCEFALO S/C", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(148, "41101324", "ANGIO-RESSONANCIA CRANIO S/C", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(149, "41101359", "HIDRO-RM (COLANGIO-RM OU URO-RM, OU MIELO-RM) C/CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(150, "41101359", "HIDRO-RM (COLANGIO-RM OU URO-RM, OU MIELO-RM) S/CONTRASTE", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(151, "41101537", "ANGIO-RESSONANCIA ARTERIAL CRANIO S/C", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(152, "41101545", "ANGIO-RESSONANCIA VENOSA CRANIO S/C", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(153, "41101618", "ANGIO-RESSONANCIA PESCOÇO ARTERIAL S/C", SetorProcedimento.Ressonância, 1, DateTime.UtcNow),
                        new Procedimento(154, "41101626", "ANGIO-RESSONANCIA PESCOÇO VENOSA S/C", SetorProcedimento.Ressonância, 1, DateTime.UtcNow)
                    );
            });

            #endregion

            #region Especialidade

            List<Especialidade> listaEspecialidades = new List<Especialidade>
            {
                new Especialidade(1, "Acupuntura", 1, DateTime.UtcNow),
                new Especialidade(2, "Alergia e Imunologia", 1, DateTime.UtcNow),
                new Especialidade(3, "Anestesiologia", 1, DateTime.UtcNow),
                new Especialidade(4, "Angiologia", 1, DateTime.UtcNow),
                new Especialidade(5, "Cancerologia", 1, DateTime.UtcNow),
                new Especialidade(6, "Cardiologia", 1, DateTime.UtcNow),
                new Especialidade(7, "Cirurgia Cardiovascular", 1, DateTime.UtcNow),
                new Especialidade(8, "Cirurgia da Mão", 1, DateTime.UtcNow),
                new Especialidade(9, "Cirurgia de Cabeça e Pescoço", 1, DateTime.UtcNow),
                new Especialidade(10, "Cirurgia do Aparelho Digestivo", 1, DateTime.UtcNow),
                new Especialidade(11, "Cirurgia Geral", 1, DateTime.UtcNow),
                new Especialidade(12, "Cirurgia Pediátrica", 1, DateTime.UtcNow),
                new Especialidade(13, "Cirurgia Plástica", 1, DateTime.UtcNow),
                new Especialidade(14, "Cirurgia Torácica", 1, DateTime.UtcNow),
                new Especialidade(15, "Cirurgia Vascular", 1, DateTime.UtcNow),
                new Especialidade(16, "Clínica Médica", 1, DateTime.UtcNow),
                new Especialidade(17, "Coloproctologia", 1, DateTime.UtcNow),
                new Especialidade(18, "Dermatologia", 1, DateTime.UtcNow),
                new Especialidade(19, "Endocrinologia e Metabologia", 1, DateTime.UtcNow),
                new Especialidade(20, "Endoscopia", 1, DateTime.UtcNow),
                new Especialidade(21, "Gastroenterologia", 1, DateTime.UtcNow),
                new Especialidade(22, "Genética Médica", 1, DateTime.UtcNow),
                new Especialidade(23, "Geriatria", 1, DateTime.UtcNow),
                new Especialidade(24, "Ginecologia e Obstetrícia", 1, DateTime.UtcNow),
                new Especialidade(25, "Hematologia e Hemoterapia", 1, DateTime.UtcNow),
                new Especialidade(26, "Homeopatia", 1, DateTime.UtcNow),
                new Especialidade(27, "Infectologia", 1, DateTime.UtcNow),
                new Especialidade(28, "Mastologia", 1, DateTime.UtcNow),
                new Especialidade(29, "Medicina de Família e Comunidade", 1, DateTime.UtcNow),
                new Especialidade(30, "Medicina do Trabalho", 1, DateTime.UtcNow),
                new Especialidade(31, "Medicina de Tráfego", 1, DateTime.UtcNow),
                new Especialidade(32, "Medicina Esportiva", 1, DateTime.UtcNow),
                new Especialidade(33, "Medicina Física e Reabilitação", 1, DateTime.UtcNow),
                new Especialidade(34, "Medicina Intensiva", 1, DateTime.UtcNow),
                new Especialidade(35, "Medicina Legal e Perícia Médica", 1, DateTime.UtcNow),
                new Especialidade(36, "Medicina Nuclear", 1, DateTime.UtcNow),
                new Especialidade(37, "Medicina Preventiva e Social", 1, DateTime.UtcNow),
                new Especialidade(38, "Nefrologia", 1, DateTime.UtcNow),
                new Especialidade(39, "Neurocirurgia", 1, DateTime.UtcNow),
                new Especialidade(40, "Neurologia", 1, DateTime.UtcNow),
                new Especialidade(41, "Nutrologia", 1, DateTime.UtcNow),
                new Especialidade(42, "Oftalmologia", 1, DateTime.UtcNow),
                new Especialidade(43, "Ortopedia e Traumatologia", 1, DateTime.UtcNow),
                new Especialidade(44, "Otorrinolaringologia", 1, DateTime.UtcNow),
                new Especialidade(45, "Patologia", 1, DateTime.UtcNow),
                new Especialidade(46, "Patologia Clínica/Medicina Laboratorial", 1, DateTime.UtcNow),
                new Especialidade(47, "Pediatria", 1, DateTime.UtcNow),
                new Especialidade(48, "Pneumologia", 1, DateTime.UtcNow),
                new Especialidade(49, "Psiquiatria", 1, DateTime.UtcNow),
                new Especialidade(50, "Radiologia e Diagnóstico por Imagem", 1, DateTime.UtcNow),
                new Especialidade(51, "Radioterapia", 1, DateTime.UtcNow),
                new Especialidade(52, "Reumatologia", 1, DateTime.UtcNow),
                new Especialidade(53, "Urologia", 1, DateTime.UtcNow)
            };

            modelBuilder.Entity<Especialidade>(e =>
            {
                // Seed Especialidades
                e.HasData(listaEspecialidades.ToArray());
            });

            #endregion

            #region Prestador

            List<object> listaPrestadores = new List<object>();

            for (long pessoaId = 1; pessoaId < 11; pessoaId++)
            {
                int randomGender = r.Next(2);

                QualificacaoPrestador qualificacaoPrestador = QualificacaoPrestador.Especialista;

                Enum.TryParse(r.Next(6).ToString(), out qualificacaoPrestador);

                listaPrestadores.Add(new
                {
                    Qualificacao = qualificacaoPrestador,
                    Id = pessoaId * 10000,
                    CPF = $"{r.Next(999999999).ToString().PadLeft(11, '0')}",
                    CriadoEm = DateTime.UtcNow,
                    CriadoPor = (long)1,
                    DataAdesao = DateTime.UtcNow.AddDays(-90),
                    DataNascimento = DateTime.UtcNow.AddYears(-35),
                    Genero = randomGender == 0 ? GeneroPessoa.Masculino : GeneroPessoa.Feminino,
                    Nome = $"{nameGenerator.FirstName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)} {nameGenerator.LastName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)}",
                    NumeroCarteirinha = $"{r.Next(99999999).ToString().PadLeft(8, '0')}{r.Next(99999999).ToString().PadLeft(8, '0')}",
                    RG = $"{r.Next(999999999).ToString().PadLeft(9, '0')}",
                    TipoPessoa = TipoPessoa.Colaborador
                });
            }

            modelBuilder.Entity<Prestador>(p =>
            {
                // Seed Prestadores
                p.HasData(listaPrestadores.ToArray());
            });

            #endregion

            #region Especialidade x Prestador

            List<object> especialidadePrestador = new List<object>();

            foreach (Especialidade especialidade in listaEspecialidades)
            {
                especialidadePrestador.Add(new
                {
                    EspecialidadeId = especialidade.Id,
                    PrestadorId = (long)r.Next(1, 11) * 10000
                });
            }

            // Entidade de relacionamento Especialidade x Prestador
            modelBuilder.Entity<Prestador>()
                        .HasMany(p => p.Especialidades)
                        .WithMany(p => p.Prestadores)
                        .UsingEntity<Dictionary<string, object>>(
                            "EspecialidadePrestador",
                            j => j
                                .HasOne<Especialidade>()
                                .WithMany()
                                .HasForeignKey("EspecialidadeId"),
                            j => j
                                .HasOne<Prestador>()
                                .WithMany()
                                .HasForeignKey("PrestadorId")).HasData(especialidadePrestador.ToArray());

            #endregion
        }
    }
}