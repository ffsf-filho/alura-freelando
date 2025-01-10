using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelando.Dados.Mapeamentos;

internal class ProjetoTypeConfiguration : IEntityTypeConfiguration<Projeto>
{
    public void Configure(EntityTypeBuilder<Projeto> entity)
    {
        entity.ToTable("TB_Projetos");
        entity.HasKey(e => e.Id).HasName("PK_Projeto");
        entity.Property(e => e.Id).HasColumnName("Id_Projeto");
        entity.Property(e => e.Descricao).HasColumnType("nvarchar(200)").HasColumnName("DS_Projeto");
        entity.Property(e => e.Status)
            //.HasColumnType("nvarchar(max)")
            //.IsRequired()
            //.HasColumnName("Status")
            .HasConversion(
                fromObj => fromObj.ToString(), 
                fromDB => (StatusProjeto)Enum.Parse(typeof(StatusProjeto), fromDB)
            );

        entity.Property(e => e.Titulo).HasColumnName("Titulo").HasColumnType("nvarchar(max)");
    }
}
