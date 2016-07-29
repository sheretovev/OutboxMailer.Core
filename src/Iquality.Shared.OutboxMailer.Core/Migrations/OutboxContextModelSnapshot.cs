using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Iquality.Shared.OutboxMailer.Core.Models;

namespace Iquality.Shared.OutboxMailer.Core.Migrations
{
    [DbContext(typeof(OutboxContext))]
    partial class OutboxContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("Iquality.Shared.OutboxMailer.Core.Models.Attachment", b =>
                {
                    b.Property<Guid>("AttachmentId");

                    b.Property<string>("Content");

                    b.Property<Guid>("OutboxMessageId");

                    b.Property<string>("Title");

                    b.HasKey("AttachmentId");

                    b.HasIndex("OutboxMessageId");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("Iquality.Shared.OutboxMailer.Core.Models.OutboxMessage", b =>
                {
                    b.Property<Guid>("OutboxMessageId");

                    b.Property<string>("Body");

                    b.Property<string>("FromAddress");

                    b.Property<string>("Subject");

                    b.Property<string>("ToAddress");

                    b.HasKey("OutboxMessageId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Iquality.Shared.OutboxMailer.Core.Models.Attachment", b =>
                {
                    b.HasOne("Iquality.Shared.OutboxMailer.Core.Models.OutboxMessage", "OutboxMessages")
                        .WithMany("Attachments")
                        .HasForeignKey("OutboxMessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
