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

            modelBuilder.Entity("InternalApi.Models.CurrenciesOnDate", b =>
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
#pragma warning restore 612, 618
        }
    }
}