﻿// <auto-generated />
using System;
using InternalApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InternalApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("cur")
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InternalApi.Models.Entities.ChangeCacheTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("CacheTaskStatus")
                        .HasColumnType("integer")
                        .HasColumnName("cache_task_status");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_time");

                    b.Property<int>("NewBaseCurrency")
                        .HasColumnType("integer")
                        .HasColumnName("new_base_currency");

                    b.HasKey("Id")
                        .HasName("pk_change_cache_tasks");

                    b.ToTable("change_cache_tasks", "cur");
                });

            modelBuilder.Entity("InternalApi.Models.Entities.CurrenciesOnDate", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<string>("CurrenciesAsJson")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("currencies_as_json");

                    b.HasKey("Date")
                        .HasName("pk_currencies_on_dates");

                    b.ToTable("currencies_on_dates", "cur");
                });

            modelBuilder.Entity("InternalApi.Models.Entities.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BaseCurrency")
                        .HasColumnType("integer")
                        .HasColumnName("base_currency");

                    b.HasKey("Id")
                        .HasName("pk_settings");

                    b.ToTable("settings", "cur");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            BaseCurrency = 0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
