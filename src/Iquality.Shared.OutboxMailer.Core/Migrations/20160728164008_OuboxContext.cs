using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iquality.Shared.OutboxMailer.Core.Migrations
{
    public partial class OuboxContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    OutboxMessageId = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    FromAddress = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    ToAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.OutboxMessageId);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    OutboxMessageId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Attachment_Messages_OutboxMessageId",
                        column: x => x.OutboxMessageId,
                        principalTable: "Messages",
                        principalColumn: "OutboxMessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_OutboxMessageId",
                table: "Attachment",
                column: "OutboxMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
