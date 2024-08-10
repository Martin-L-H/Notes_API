using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes_API_CORE.Entities;

namespace PRUEBA_DAL.Configuration
{
    public class ConfigurationUserNote : IEntityTypeConfiguration<UserNote>
    {
        public void Configure(EntityTypeBuilder<UserNote> builder)
        {
            builder.HasKey(p => new {p.User_Id,p.Note_Id});
            builder.HasOne(p => p._user).WithMany(p => p._usersnotes).HasForeignKey(p => p.User_Id);
            builder.HasOne(p => p._note).WithMany(p => p._usersnotes).HasForeignKey(p => p.Note_Id);
        }
    }
}
