﻿// <auto-generated />
using Fuse8_ByteMinds.SummerSchool.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230820072712_AddSettings")]
    partial class AddSettings
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("user")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrencyRoundCount")
                        .HasColumnType("integer")
                        .HasColumnName("currency_round_count");

                    b.Property<int>("DefaultCurrency")
                        .HasColumnType("integer")
                        .HasColumnName("default_currency");

                    b.HasKey("Id")
                        .HasName("pk_settings");

                    b.ToTable("settings", "user");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            CurrencyRoundCount = 2,
                            DefaultCurrency = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
