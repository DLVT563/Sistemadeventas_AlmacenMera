using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemadeventas_AlmacenMera.Migrations
{
    /// <inheritdoc />
    public partial class MIGRACION1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id_categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__CD54BC5A783E9B06", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    id_proveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_proveedor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__proveedo__8D3DFE28D6467724", x => x.id_proveedor);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__roles__6ABCB5E03EFACA1E", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_producto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    id_proveedor = table.Column<int>(type: "int", nullable: true),
                    id_categoria = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__producto__FF341C0D2E6180F9", x => x.id_producto);
                    table.ForeignKey(
                        name: "FK__productos__id_ca__45F365D3",
                        column: x => x.id_categoria,
                        principalTable: "categorias",
                        principalColumn: "id_categoria");
                    table.ForeignKey(
                        name: "FK__productos__id_pr__44FF419A",
                        column: x => x.id_proveedor,
                        principalTable: "proveedores",
                        principalColumn: "id_proveedor");
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    contraseña = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_rol = table.Column<int>(type: "int", nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    estado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usuarios__4E3E04AD5D8725D1", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK__usuarios__id_rol__3D5E1FD2",
                        column: x => x.id_rol,
                        principalTable: "roles",
                        principalColumn: "id_rol");
                });

            migrationBuilder.CreateTable(
                name: "almacen",
                columns: table => new
                {
                    id_almacen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    fecha_entrada = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__almacen__098D5D13B1E3EE71", x => x.id_almacen);
                    table.ForeignKey(
                        name: "FK__almacen__id_prod__49C3F6B7",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "historial_entradas_salidas",
                columns: table => new
                {
                    id_historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    tipo_movimiento = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    fecha_movimiento = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__historia__76E6C5028A10B7E2", x => x.id_historial);
                    table.ForeignKey(
                        name: "FK__historial__id_pr__68487DD7",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "historial_precios",
                columns: table => new
                {
                    id_precio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_cambio = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__historia__A70EF6EDD3220AF5", x => x.id_precio);
                    table.ForeignKey(
                        name: "FK__historial__id_pr__59063A47",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                });

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    id_venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_usuario = table.Column<int>(type: "int", nullable: true),
                    fecha_venta = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "pendiente"),
                    tipo_venta = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "contado")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ventas__459533BF50DF49F8", x => x.id_venta);
                    table.ForeignKey(
                        name: "FK__ventas__id_usuar__5165187F",
                        column: x => x.id_usuario,
                        principalTable: "usuarios",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "detalle_ventas",
                columns: table => new
                {
                    id_detalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<int>(type: "int", nullable: true),
                    id_producto = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__detalle___4F1332DED72C0490", x => x.id_detalle);
                    table.ForeignKey(
                        name: "FK__detalle_v__id_pr__5535A963",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto");
                    table.ForeignKey(
                        name: "FK__detalle_v__id_ve__5441852A",
                        column: x => x.id_venta,
                        principalTable: "ventas",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateTable(
                name: "fiados",
                columns: table => new
                {
                    id_fiado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<int>(type: "int", nullable: true),
                    monto_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    monto_pagado = table.Column<decimal>(type: "decimal(10,2)", nullable: true, defaultValue: 0.00m),
                    saldo_pendiente = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    fecha_vencimiento = table.Column<DateTime>(type: "datetime", nullable: true),
                    estado = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "activo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__fiados__420FD58DC3369324", x => x.id_fiado);
                    table.ForeignKey(
                        name: "FK__fiados__id_venta__5FB337D6",
                        column: x => x.id_venta,
                        principalTable: "ventas",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateTable(
                name: "historial_ventas_usuario",
                columns: table => new
                {
                    id_historial_venta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<int>(type: "int", nullable: true),
                    nombre_usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fecha_venta = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    total_venta = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__historia__B96FB70281153004", x => x.id_historial_venta);
                    table.ForeignKey(
                        name: "FK__historial__id_ve__6C190EBB",
                        column: x => x.id_venta,
                        principalTable: "ventas",
                        principalColumn: "id_venta");
                });

            migrationBuilder.CreateTable(
                name: "pagos_fiados",
                columns: table => new
                {
                    id_pago_fiado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_fiado = table.Column<int>(type: "int", nullable: true),
                    monto_pago = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    fecha_pago = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    metodo_pago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__pagos_fi__CDDE6AE73CCAB987", x => x.id_pago_fiado);
                    table.ForeignKey(
                        name: "FK__pagos_fia__id_fi__6383C8BA",
                        column: x => x.id_fiado,
                        principalTable: "fiados",
                        principalColumn: "id_fiado");
                });

            migrationBuilder.CreateIndex(
                name: "IX_almacen_id_producto",
                table: "almacen",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ventas_id_producto",
                table: "detalle_ventas",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_ventas_id_venta",
                table: "detalle_ventas",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_fiados_id_venta",
                table: "fiados",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_historial_entradas_salidas_id_producto",
                table: "historial_entradas_salidas",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_historial_precios_id_producto",
                table: "historial_precios",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_historial_ventas_usuario_id_venta",
                table: "historial_ventas_usuario",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_pagos_fiados_id_fiado",
                table: "pagos_fiados",
                column: "id_fiado");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_categoria",
                table: "productos",
                column: "id_categoria");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_proveedor",
                table: "productos",
                column: "id_proveedor");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_id_rol",
                table: "usuarios",
                column: "id_rol");

            migrationBuilder.CreateIndex(
                name: "UQ__usuarios__AB6E6164A612CDC6",
                table: "usuarios",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ventas_id_usuario",
                table: "ventas",
                column: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "almacen");

            migrationBuilder.DropTable(
                name: "detalle_ventas");

            migrationBuilder.DropTable(
                name: "historial_entradas_salidas");

            migrationBuilder.DropTable(
                name: "historial_precios");

            migrationBuilder.DropTable(
                name: "historial_ventas_usuario");

            migrationBuilder.DropTable(
                name: "pagos_fiados");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "fiados");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "ventas");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
