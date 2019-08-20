﻿// <auto-generated />
using DoorUnlocker.API.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoorUnlocker.API.Application.Domain.Migrations
{
    [DbContext(typeof(DoorsContext))]
    [Migration("20190817091801_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("DoorUnlocker.API.Application.Domain.Models.Door", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("OfficeId");

                    b.HasKey("Id");

                    b.HasIndex("OfficeId");

                    b.ToTable("Doors");
                });

            modelBuilder.Entity("DoorUnlocker.API.Application.Domain.Models.DoorPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DoorId");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DoorId");

                    b.ToTable("DoorPermissions");
                });

            modelBuilder.Entity("DoorUnlocker.API.Application.Domain.Models.Office", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("DoorUnlocker.API.Application.Domain.Models.Door", b =>
                {
                    b.HasOne("DoorUnlocker.API.Application.Domain.Models.Office", "Office")
                        .WithMany("Doors")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DoorUnlocker.API.Application.Domain.Models.DoorPermission", b =>
                {
                    b.HasOne("DoorUnlocker.API.Application.Domain.Models.Door", "Door")
                        .WithMany()
                        .HasForeignKey("DoorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
