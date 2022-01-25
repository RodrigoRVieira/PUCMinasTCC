namespace GISA.Domain.Model
{
    public enum GeneroPessoa
    {
        Feminino,
        Masculino
    }

    public enum TipoPessoa
    {
        Associado,
        Colaborador
    }

    public enum TipoContratacao
    {
        Empresarial,
        Individual
    }

    public enum StatusPlano
    {
        Ativo,
        Inativo,
        Suspenso
    }

    public enum CategoriaPlano
    {
        Apartamento,
        Enfermaria,
        VIP
    }

    public enum TipoConveniado
    {
        Clínica,
        Laboratório,
        Hospital
    }

    public enum QualificacaoPrestador
    {
        Doutorado,
        Especialista,
        Mestrado,
        PósDoutorado,
        PosGraduacao,
        Residência
    }

    public enum SetorProcedimento
    {
        Punção,
        UltraSom,
        Ressonância,
        Tomografia,
        Densitometria,
        Doppler,
        Mamografia
    }

    public enum StatusConsulta
    {
        Criada,
        Agendada,
        Rejeitada,
        Realizada
    }
}