using CashFlow.Domain.Entity.Financial;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CashFlow.Infrastructure.Data.Mapping.Financial
{
    public class CashMovimentMap : IEntityTypeConfiguration<CashMoviment>
    {
        public void Configure(EntityTypeBuilder<CashMoviment> builder)
        {
            builder
                .ToTable("CashMoviment");
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.CreateAt)
                .HasDefaultValueSql("Now()");
            builder
                .Property(x => x.Historic);
            builder
                .Property(x => x.Value)
                .HasPrecision(18, 2);
            builder
                .Property(x => x.Nature);
        }
    }
}
