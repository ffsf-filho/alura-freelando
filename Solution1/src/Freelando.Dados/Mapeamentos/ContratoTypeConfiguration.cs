using Freelando.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freelando.Dados.Mapeamentos
{
    internal class ContratoTypeConfiguration : IEntityTypeConfiguration<Contrato>
    {
        public void Configure(EntityTypeBuilder<Contrato> entity)
        {
            entity.ToTable("TB_Contratos");
            entity.HasKey(e => e.Id).HasName("PK_TB_Contratos");
            entity.Property(e => e.Id).HasColumnName("Id_Contrato");
            entity.Property(e => e.Valor).HasColumnType("float").HasColumnName("Valor");
            entity.OwnsOne(e => e.Vigencia, vigencia =>
            {
                vigencia.Property(v => v.DataInicio).HasColumnName("Data_Inicio");
                vigencia.Property(v => v.DataEncerramento).HasColumnName("Data_Encerramento");
            });
        }
    }
}
