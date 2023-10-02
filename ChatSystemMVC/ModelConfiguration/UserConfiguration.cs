using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ChatSystemMVC.Models;

namespace ChatSystemMVC.ModelConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("VARCHAR(50)").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("VARCHAR(100)").HasMaxLength(100).IsRequired();
            builder.Property(x => x.ChatRoom).HasColumnName("column_name").HasColumnType("VARCHAR(100)").IsRequired();
            builder.HasMany(x => x.Friends).WithOne(x => x.Users).HasForeignKey(x => x.FriendId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}