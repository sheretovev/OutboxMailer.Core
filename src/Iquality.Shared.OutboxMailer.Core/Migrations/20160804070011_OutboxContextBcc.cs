using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Iquality.Shared.OutboxMailer.Core.Migrations
{
    public partial class OutboxContextBcc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BccAddress",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CcAddress",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BccAddress",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CcAddress",
                table: "Messages");
        }
    }
}
