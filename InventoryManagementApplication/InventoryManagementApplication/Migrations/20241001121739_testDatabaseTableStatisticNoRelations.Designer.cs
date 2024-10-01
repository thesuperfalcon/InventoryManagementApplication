﻿// <auto-generated />
using System;
using InventoryManagementApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventoryManagementApplication.Migrations
{
    [DbContext(typeof(InventoryManagementApplicationContext))]
    [Migration("20241001121739_testDatabaseTableStatisticNoRelations")]
    partial class testDatabaseTableStatisticNoRelations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FullAccess")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "fullAccess");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "roleName");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "created");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("EmployeeNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "employeeNumber");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "firstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "lastName");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "roleId");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updated");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasAnnotation("Relational:JsonPropertyName", "User");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.InventoryTracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "modified");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "productId");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "quantity");

                    b.Property<int?>("StorageId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "storageId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StorageId");

                    b.ToTable("InventoryTracker");

                    b.HasAnnotation("Relational:JsonPropertyName", "inventoryTrackers");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "action");

                    b.Property<string>("EntityDetails")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "entityDetails");

                    b.Property<string>("EntityName")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "entityName");

                    b.Property<string>("EntityType")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "entityType");

                    b.Property<DateTime?>("TimeStamp")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "timeStamp");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "userId");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArticleNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "articleNumber");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "created");

                    b.Property<int?>("CurrentStock")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "currentStock");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "description");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "isDeleted");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)")
                        .HasAnnotation("Relational:JsonPropertyName", "price");

                    b.Property<int?>("TotalStock")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "totalStock");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updated");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasAnnotation("Relational:JsonPropertyName", "product");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.Statistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DestinationStorageId")
                        .HasColumnType("int");

                    b.Property<string>("DestinationStorageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InitialStorageId")
                        .HasColumnType("int");

                    b.Property<string>("IntitialStorageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Moved")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.StatisticRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("Completed")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "completed");

                    b.Property<int?>("DestinationStorageId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "destinationStorageId");

                    b.Property<DateTime?>("FinishedTime")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "finishedTime");

                    b.Property<int?>("InitialStorageId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "intitialStorageId");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "notes");

                    b.Property<DateTime?>("OrderTime")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "orderTime");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "productId");

                    b.Property<int>("ProductQuantity")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "productQuantity");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "userId");

                    b.HasKey("Id");

                    b.HasIndex("DestinationStorageId");

                    b.HasIndex("InitialStorageId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("StatisticRelation");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.Storage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "created");

                    b.Property<int?>("CurrentStock")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "currentStock");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit")
                        .HasAnnotation("Relational:JsonPropertyName", "isDeleted");

                    b.Property<int?>("MaxCapacity")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "maxCapacity");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updated");

                    b.HasKey("Id");

                    b.ToTable("Storages");

                    b.HasAnnotation("Relational:JsonPropertyName", "initialStorage");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.InventoryTracker", b =>
                {
                    b.HasOne("InventoryManagementApplication.Models.Product", "Product")
                        .WithMany("InventoryTrackers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InventoryManagementApplication.Models.Storage", "Storage")
                        .WithMany("InventoryTrackers")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Product");

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.StatisticRelation", b =>
                {
                    b.HasOne("InventoryManagementApplication.Models.Storage", "DestinationStorage")
                        .WithMany()
                        .HasForeignKey("DestinationStorageId");

                    b.HasOne("InventoryManagementApplication.Models.Storage", "InitialStorage")
                        .WithMany()
                        .HasForeignKey("InitialStorageId");

                    b.HasOne("InventoryManagementApplication.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", "User")
                        .WithMany("StatisticUsers")
                        .HasForeignKey("UserId");

                    b.Navigation("DestinationStorage");

                    b.Navigation("InitialStorage");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InventoryManagementApplication.Areas.Identity.Data.InventoryManagementUser", b =>
                {
                    b.Navigation("StatisticUsers");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.Product", b =>
                {
                    b.Navigation("InventoryTrackers");
                });

            modelBuilder.Entity("InventoryManagementApplication.Models.Storage", b =>
                {
                    b.Navigation("InventoryTrackers");
                });
#pragma warning restore 612, 618
        }
    }
}
