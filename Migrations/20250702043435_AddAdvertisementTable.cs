using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Retalling_Portal.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertisementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    advertisementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    redirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.advertisementID);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parentCategoryId = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    deleteAt = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_parentCategoryId",
                        column: x => x.parentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    statusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    displayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    birthday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    province = table.Column<int>(type: "int", nullable: false),
                    district = table.Column<int>(type: "int", nullable: false),
                    ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    firstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    externalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    externalType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Accounts_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    createTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    endTime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    paymentStatus = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    note = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    province = table.Column<int>(type: "int", nullable: false),
                    district = table.Column<int>(type: "int", nullable: false),
                    ward = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    createdAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shopDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    statusId = table.Column<int>(type: "int", nullable: false),
                    province = table.Column<int>(type: "int", nullable: false),
                    district = table.Column<int>(type: "int", nullable: false),
                    ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shops_Accounts_accountId",
                        column: x => x.accountId,
                        principalTable: "Accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shops_Statuses_statusId",
                        column: x => x.statusId,
                        principalTable: "Statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    startDate = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    endDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    value = table.Column<int>(type: "int", nullable: false),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    deleteAt = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Discounts_Shops_shopId",
                        column: x => x.shopId,
                        principalTable: "Shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    shopId = table.Column<int>(type: "int", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    weight = table.Column<int>(type: "int", nullable: false),
                    isVariation = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    deleteAt = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    createAt = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    vectorEmbaddingJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Shops_shopId",
                        column: x => x.shopId,
                        principalTable: "Shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    imageName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    productCoveredId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.id);
                    table.ForeignKey(
                        name: "FK_Images_Products_productCoveredId",
                        column: x => x.productCoveredId,
                        principalTable: "Products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Images_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    imageId = table.Column<int>(type: "int", nullable: false),
                    attribute = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    deleteAt = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductItems_Images_imageId",
                        column: x => x.imageId,
                        principalTable: "Images",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductItems_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    productItemId = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    shippingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    price = table.Column<double>(type: "float", nullable: false),
                    shippingFee = table.Column<int>(type: "int", nullable: false),
                    transactionFee = table.Column<double>(type: "float", nullable: false),
                    externalOrderCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_orderId",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_ProductItems_productItemId",
                        column: x => x.productItemId,
                        principalTable: "ProductItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductDiscounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    discountId = table.Column<int>(type: "int", nullable: false),
                    productItemId = table.Column<int>(type: "int", nullable: true),
                    productId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_Discounts_discountId",
                        column: x => x.discountId,
                        principalTable: "Discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_ProductItems_productItemId",
                        column: x => x.productItemId,
                        principalTable: "ProductItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductDiscounts_Products_productId",
                        column: x => x.productId,
                        principalTable: "Products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "id", "deleteAt", "name", "parentCategoryId" },
                values: new object[,]
                {
                    { 1, null, "Clothes", null },
                    { 2, null, "Watch", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "id", "roleName" },
                values: new object[,]
                {
                    { 1, "Customer" },
                    { 2, "Shop Owner" },
                    { 3, "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "id", "name", "value" },
                values: new object[] { 1, "fee", "10%" });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "id", "statusName" },
                values: new object[] { 1, "active" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "address", "birthday", "displayName", "district", "email", "firstName", "gender", "lastName", "phoneNumber", "province", "ward" },
                values: new object[,]
                {
                    { 1, "", "2000-05-04", "kienhocgioi", 1454, "abc@gmail.com", "first", "Female", "last", "0326075641", 202, "21211" },
                    { 2, "mhjyy", "2000-01-04", "anh", 1454, "quynxhe186459@fpt.edu.vn", "first", "Female", "last", "0123459145", 202, "21211" },
                    { 3, "", "2004-01-04", "phien", 1454, "hoangphien46@gmail.com", "duc", "Male", "phien", "0968059984", 202, "21211" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "id", "externalId", "externalType", "password", "roleId", "userId", "username" },
                values: new object[,]
                {
                    { 1, null, null, "123", 2, 1, "admin" },
                    { 2, null, null, "123", 1, 2, "anh" },
                    { 3, null, null, "123", 2, 2, "seller" },
                    { 4, null, null, "123", 1, 3, "phien47" },
                    { 5, null, null, "123", 3, 3, "manager" },
                    { 6, null, null, "123", 1, 1, "admin" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "id", "deleteAt", "name", "parentCategoryId" },
                values: new object[,]
                {
                    { 3, null, "Jean", 1 },
                    { 4, null, "Shirt", 1 },
                    { 6, null, "abc", 1 },
                    { 5, null, "Jacket", 3 }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "id", "accountId", "address", "createdAt", "district", "name", "province", "shopDescription", "statusId", "ward" },
                values: new object[,]
                {
                    { 1, 1, "address", "2000-05-04", 1644, "shopname", 249, "sd", 1, "190116" },
                    { 2, 3, "address", "2000-05-04", 1644, "shopname", 249, "sd", 1, "190116" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "id", "deleteAt", "endDate", "name", "shopId", "startDate", "value" },
                values: new object[,]
                {
                    { 1, null, "2024-06-30", "Summer Sale", 1, "2024-10-31", 15 },
                    { 2, null, "2024-07-15", "Birthday Discount", 1, "2024-07-01", 20 },
                    { 3, null, "2024-08-31", "Buy One Get One Free", 1, "2024-08-01", 50 },
                    { 4, null, "2024-11-09", "Holiday Discount", 1, "2024-10-31", 50 },
                    { 5, null, "2024-11-09", "End of Year Sale", 1, "2024-10-31", 80 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "id", "categoryId", "createAt", "deleteAt", "desc", "isVariation", "name", "price", "quantity", "shopId", "status", "vectorEmbaddingJson", "weight" },
                values: new object[,]
                {
                    { 1, 3, null, null, "<p>this is a good product<p/>", true, "Sport Shoe", 11.0, 0, 1, 1, null, 1000 },
                    { 2, 4, null, null, "<p>this is a good product<p/>", false, "T-shirt", 12.0, 5, 1, 1, null, 1000 },
                    { 3, 4, null, null, "<p>this is a good product<p/>", true, "Jacket", 13.0, 0, 1, 1, null, 1000 },
                    { 4, 6, null, null, "<p>this is a good product<p/>", false, "Jacketooo", 14.0, 6, 1, 1, null, 1000 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "id", "imageName", "productCoveredId", "productId" },
                values: new object[,]
                {
                    { 1, "Screenshot 2024-09-13 154422.png", 1, 1 },
                    { 2, "Screenshot 2024-09-14 075432.png", null, 1 },
                    { 3, "Screenshot 2024-09-14 075432.png", null, 1 },
                    { 4, "Screenshot 2024-09-14 075432.png", 2, 2 },
                    { 5, "Screenshot 2024-09-14 080322.png", null, 2 },
                    { 6, "Screenshot 2024-09-14 080322.png", null, 2 },
                    { 7, "Screenshot 2024-09-16 214607.png", 3, 3 },
                    { 8, "Screenshot 2024-09-16 214607.png", null, 3 },
                    { 9, "Screenshot 2024-09-16 214607.png", null, 3 },
                    { 10, "Screenshot 2024-09-16 214607.png", 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "ProductDiscounts",
                columns: new[] { "id", "discountId", "productId", "productItemId" },
                values: new object[,]
                {
                    { 1, 4, 2, null },
                    { 2, 4, 4, null }
                });

            migrationBuilder.InsertData(
                table: "ProductItems",
                columns: new[] { "id", "attribute", "deleteAt", "imageId", "price", "productId", "quantity" },
                values: new object[,]
                {
                    { 1, "L", null, 1, 1500000.0, 1, 5 },
                    { 2, "S", null, 3, 1003000.0, 1, 7 },
                    { 3, "X", null, 8, 2006.0, 3, 10 },
                    { 4, "X", null, 9, 10005.0, 3, 10 },
                    { 5, "M", null, 8, 20000.0, 3, 10 },
                    { 6, "L", null, 9, 1004.0, 3, 13 }
                });

            migrationBuilder.InsertData(
                table: "ProductDiscounts",
                columns: new[] { "id", "discountId", "productId", "productItemId" },
                values: new object[,]
                {
                    { 5, 4, 3, 3 },
                    { 6, 5, 3, 4 },
                    { 7, 4, 3, 5 },
                    { 8, 4, 3, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_roleId",
                table: "Accounts",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_userId",
                table: "Accounts",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_parentCategoryId",
                table: "Categories",
                column: "parentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_shopId",
                table: "Discounts",
                column: "shopId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_productCoveredId",
                table: "Images",
                column: "productCoveredId",
                unique: true,
                filter: "[productCoveredId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Images_productId",
                table: "Images",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_orderId",
                table: "OrderItems",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_productId",
                table: "OrderItems",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_productItemId",
                table: "OrderItems",
                column: "productItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_userId",
                table: "Orders",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_discountId",
                table: "ProductDiscounts",
                column: "discountId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_productId",
                table: "ProductDiscounts",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscounts_productItemId",
                table: "ProductDiscounts",
                column: "productItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_imageId",
                table: "ProductItems",
                column: "imageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductItems_productId",
                table: "ProductItems",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_categoryId",
                table: "Products",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_shopId",
                table: "Products",
                column: "shopId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_accountId",
                table: "Shops",
                column: "accountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shops_statusId",
                table: "Shops",
                column: "statusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductDiscounts");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "ProductItems");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
