using System;
using Bogus;
using Identity.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    /// <inheritdoc />
    public partial class addArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ID);
                });
            Randomizer.Seed =  new Random(8675309);
            var fakeArticle =  new Faker<Article>();
            fakeArticle.RuleFor(a=> a.Title, f => f.Lorem.Sentence(5,5));
            fakeArticle.RuleFor(a=> a.Content, f => f.Lorem.Sentence(1,4));
            fakeArticle.RuleFor(a=> a.PublishDate, f => f.Date.Between(new DateTime(2023,3,1),new DateTime(2023,3,31)));

            for(int i = 0; i< 150;i++){
                Article article = fakeArticle.Generate();
                migrationBuilder.InsertData(
                    table:"Articles",
                    columns: new[]{"Title","PublishDate","Content"},
                    values: new object[]{
                        article.Title,
                        article.PublishDate,
                        article.Content,
                    }
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }
    }
}
