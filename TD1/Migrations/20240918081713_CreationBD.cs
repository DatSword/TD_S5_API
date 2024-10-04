using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TD1.Migrations
{
    /// <inheritdoc />
    public partial class CreationBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "marque",
                columns: table => new
                {
                    idmarque = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nommarque = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_marque", x => x.idmarque);
                });

            migrationBuilder.CreateTable(
                name: "typeproduit",
                columns: table => new
                {
                    idtypeproduit = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nomtypeproduit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_typeproduit", x => x.idtypeproduit);
                });

            migrationBuilder.CreateTable(
                name: "produit",
                columns: table => new
                {
                    idtypeproduit = table.Column<int>(type: "integer", nullable: false),
                    idmarque = table.Column<int>(type: "integer", nullable: false),
                    idproduit = table.Column<int>(type: "integer", nullable: false),
                    nomproduit = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    idnomphoto = table.Column<string>(type: "text", nullable: false),
                    iduriproduit = table.Column<string>(type: "text", nullable: false),
                    stockreel = table.Column<int>(type: "integer", nullable: false),
                    stockmin = table.Column<int>(type: "integer", nullable: false),
                    stockmax = table.Column<int>(type: "integer", nullable: false),
                    idmarque1 = table.Column<int>(type: "integer", nullable: false),
                    idtypeproduit1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_produit", x => new { x.idmarque, x.idtypeproduit });
                    table.ForeignKey(
                        name: "fk_produit_marque",
                        column: x => x.idmarque1,
                        principalTable: "marque",
                        principalColumn: "idmarque");
                    table.ForeignKey(
                        name: "fk_produit_typeproduit",
                        column: x => x.idtypeproduit1,
                        principalTable: "typeproduit",
                        principalColumn: "idtypeproduit");
                });

            migrationBuilder.CreateIndex(
                name: "IX_produit_idmarque1",
                table: "produit",
                column: "idmarque1");

            migrationBuilder.CreateIndex(
                name: "IX_produit_idtypeproduit1",
                table: "produit",
                column: "idtypeproduit1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produit");

            migrationBuilder.DropTable(
                name: "marque");

            migrationBuilder.DropTable(
                name: "typeproduit");
        }
    }
}
