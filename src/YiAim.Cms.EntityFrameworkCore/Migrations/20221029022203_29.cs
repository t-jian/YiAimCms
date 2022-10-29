using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YiAim.Cms.Migrations
{
    public partial class _29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cms_blog_cms_category_CategoryId",
                table: "cms_blog");

            migrationBuilder.DropIndex(
                name: "IX_cms_blog_CategoryId",
                table: "cms_blog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cms_blog_CategoryId",
                table: "cms_blog",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_cms_blog_cms_category_CategoryId",
                table: "cms_blog",
                column: "CategoryId",
                principalTable: "cms_category",
                principalColumn: "Id");
        }
    }
}
