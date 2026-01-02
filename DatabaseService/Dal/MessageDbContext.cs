using Dal.Message.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

public class MessageDbContext : DbContext
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<MessageDal> Messages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageMetadataDal>(b =>
        {
            b.HasKey(x => new { x.MessageId, x.Key });
            b.Property(x => x.Key)
             .HasMaxLength(200);

            b.Property(x => x.Value)
             .HasMaxLength(2000);
            b.HasOne(x => x.Message)
             .WithMany(x => x.Metadata)
             .HasForeignKey(x => x.MessageId);
        });

        modelBuilder.Entity<MessageStatusDal>(b =>
        {
            b.HasKey(x => new { x.MessageId, x.Status });

            b.Property(x => x.Status)
             .HasConversion<string>();

            b.Property(x => x.TimeStamp)
             .IsRequired();

            b.HasIndex(x => x.MessageId);

            b.HasOne(x => x.Message)
               .WithMany(m => m.StatusHistory)
               .HasForeignKey(x => x.MessageId);
        });
    }
}
