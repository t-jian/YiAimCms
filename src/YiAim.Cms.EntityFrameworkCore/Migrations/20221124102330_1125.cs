using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YiAim.Cms.Migrations
{
    public partial class _1125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_cms_anthology_in_blog_BlogId",
                table: "cms_anthology_in_blog",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_cms_anthology_in_blog_cms_anthology_AnthologyId",
                table: "cms_anthology_in_blog",
                column: "AnthologyId",
                principalTable: "cms_anthology",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cms_anthology_in_blog_cms_blog_BlogId",
                table: "cms_anthology_in_blog",
                column: "BlogId",
                principalTable: "cms_blog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cms_anthology_in_blog_cms_anthology_AnthologyId",
                table: "cms_anthology_in_blog");

            migrationBuilder.DropForeignKey(
                name: "FK_cms_anthology_in_blog_cms_blog_BlogId",
                table: "cms_anthology_in_blog");

            migrationBuilder.DropIndex(
                name: "IX_cms_anthology_in_blog_BlogId",
                table: "cms_anthology_in_blog");
        }
    }
}
