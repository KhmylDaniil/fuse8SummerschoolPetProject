﻿// <auto-generated />
using Fuse8_ByteMinds.SummerSchool.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("user")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "citext");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities.FavoriteCurrency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseCurrency")
                        .HasColumnType("integer")
                        .HasColumnName("base_currency");

                    b.Property<int>("Currency")
                        .HasColumnType("integer")
                        .HasColumnName("currency");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("citext")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_favorite_currencies");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_favorite_currencies_name");

                    b.HasIndex("Currency", "BaseCurrency")
                        .IsUnique()
                        .HasDatabaseName("ix_favorite_currencies_currency_base_currency");

                    b.ToTable("favorite_currencies", "user");
                });

            modelBuilder.Entity("Fuse8_ByteMinds.SummerSchool.PublicApi.Models.Entities.Settings", b =>
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
