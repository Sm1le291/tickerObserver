using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TickerObserver.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TickerTopics",
                columns: table => new
                {
                    TickerTopicId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsSentAlready = table.Column<bool>(nullable: false),
                    Guid = table.Column<string>(nullable: true),
                    RssFeedSource = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    TickerName = table.Column<string>(nullable: true),
                    FullUrl = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TickerTopics", x => x.TickerTopicId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TickerTopics");
        }
    }
}
