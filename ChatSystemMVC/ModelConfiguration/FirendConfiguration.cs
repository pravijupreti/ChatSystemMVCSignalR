using ChatSystemMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatSystemMVC.ModelConfiguration
{
    public class FirendConfiguration : IEntityTypeConfiguration<Friends>
    {
        public void Configure(EntityTypeBuilder<Friends> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("VARCHAR(50)").HasMaxLength(50).IsRequired();
            builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(x => x.FriendId).HasColumnName("friend_id").HasColumnType("VARCHAR(50)").HasMaxLength(50).IsRequired();
        }
    }
}
