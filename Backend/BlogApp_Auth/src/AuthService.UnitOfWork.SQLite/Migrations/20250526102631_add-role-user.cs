﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.UnitOfWork.SQLite.Migrations
{
    /// <inheritdoc />
    public partial class addroleuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AppUser",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AppUser");
        }
    }
}
