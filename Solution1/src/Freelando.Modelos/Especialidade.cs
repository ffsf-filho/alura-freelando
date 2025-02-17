﻿using Freelando.Modelos;

namespace Freelando.Modelo;

public class Especialidade
{
    public Guid Id { get; set; }
    public string? Descricao { get; set; }
    public ICollection<Projeto> Projetos { get; set; }
    public ICollection<ProjetoEspecialidade> ProjetosEspecialidades { get; } = [];
    public ICollection<Profissional> Profissionais { get; set; }
    public ICollection<ProfissionalEspecialidade> ProfissionaisEspecialidades { get; } = [];

    public Especialidade()
    {

    }

    public Especialidade(Guid id, string? descricao, ICollection<Projeto> projetos, ICollection<Profissional> profissionals)
    {
        Id = id;
        Descricao = descricao;
        Projetos = projetos;
        Profissionais = profissionals;
    }
}