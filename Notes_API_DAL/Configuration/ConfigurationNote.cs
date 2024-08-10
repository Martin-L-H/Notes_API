using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Notes_API_CORE.Entities;

namespace PRUEBA_DAL.Configuration
{
    public class ConfigurationNote : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasKey(p => p.Note_Id);
        }
    }
}
