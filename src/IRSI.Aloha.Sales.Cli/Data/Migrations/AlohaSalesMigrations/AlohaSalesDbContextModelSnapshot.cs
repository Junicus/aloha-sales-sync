﻿// <auto-generated />
using System;
using IRSI.Aloha.Sales.Cli.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IRSI.Aloha.Sales.Cli.Data.Migrations.AlohaSalesMigrations
{
    [DbContext(typeof(AlohaSalesDbContext))]
    partial class AlohaSalesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IRSI.Aloha.Sales.Cli.Entities.BusinessDateSales", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BusinessDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BusinessDateKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ConceptId")
                        .HasColumnType("int");

                    b.Property<decimal>("GrossSales")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("NetSales")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<decimal>("Sales")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BusinessDateKey");

                    b.ToTable("BusinessDateSales");
                });
#pragma warning restore 612, 618
        }
    }
}
