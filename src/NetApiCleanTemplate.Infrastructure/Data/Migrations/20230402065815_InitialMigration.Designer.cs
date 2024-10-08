﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetApiCleanTemplate.Infrastructure.Data;

#nullable disable

namespace NetApiCleanTemplate.Infrastructure.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230402065815_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("NetApiCleanTemplate.Core.Entities.DemoEntity.DemoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DemoParentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DemoString")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DemoParentId");

                    b.ToTable("DemoEntities");
                });

            modelBuilder.Entity("NetApiCleanTemplate.Core.Entities.DemoEntity.DemoEntity", b =>
                {
                    b.HasOne("NetApiCleanTemplate.Core.Entities.DemoEntity.DemoEntity", "DemoParent")
                        .WithMany("DemoChildren")
                        .HasForeignKey("DemoParentId");

                    b.Navigation("DemoParent");
                });

            modelBuilder.Entity("NetApiCleanTemplate.Core.Entities.DemoEntity.DemoEntity", b =>
                {
                    b.Navigation("DemoChildren");
                });
#pragma warning restore 612, 618
        }
    }
}
