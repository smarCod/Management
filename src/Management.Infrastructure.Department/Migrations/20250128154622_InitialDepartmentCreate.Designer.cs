﻿// <auto-generated />
using Management.Infrastructure.Department.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Management.Infrastructure.Department.Migrations
{
    [DbContext(typeof(AppDbContextDepartment))]
    [Migration("20250128154622_InitialDepartmentCreate")]
    partial class InitialDepartmentCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.DepartmentMod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PostDepartment");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SectionSingleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SectionSingleId");

                    b.ToTable("PostDevice");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Port", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PostPort");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DepartmentModId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentModId");

                    b.ToTable("PostSection");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.SectionSingle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SectionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SectionTypId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Size")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.HasIndex("SectionTypId");

                    b.ToTable("PostSectionSingle");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.SectionSinglePort", b =>
                {
                    b.Property<int>("SectionSingleId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PortId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SectionSingleId", "PortId");

                    b.HasIndex("PortId");

                    b.ToTable("PostSectionSinglePort");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.SectionTyp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PostSectionTyp");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Device", b =>
                {
                    b.HasOne("Management.Core.Models.DepartmentModels.SectionSingle", "SectionSingle")
                        .WithMany("Devices")
                        .HasForeignKey("SectionSingleId");

                    b.Navigation("SectionSingle");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Section", b =>
                {
                    b.HasOne("Management.Core.Models.DepartmentModels.DepartmentMod", null)
                        .WithMany("Sections")
                        .HasForeignKey("DepartmentModId");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.SectionSingle", b =>
                {
                    b.HasOne("Management.Core.Models.DepartmentModels.Section", "Section")
                        .WithMany("SectionSingles")
                        .HasForeignKey("SectionId");

                    b.HasOne("Management.Core.Models.DepartmentModels.SectionTyp", "SectionTyp")
                        .WithMany()
                        .HasForeignKey("SectionTypId");

                    b.Navigation("Section");

                    b.Navigation("SectionTyp");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.SectionSinglePort", b =>
                {
                    b.HasOne("Management.Core.Models.DepartmentModels.Port", "Port")
                        .WithMany("SectionSinglePorts")
                        .HasForeignKey("PortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Management.Core.Models.DepartmentModels.SectionSingle", "SectionSingle")
                        .WithMany("SectionSinglePorts")
                        .HasForeignKey("SectionSingleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Port");

                    b.Navigation("SectionSingle");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.DepartmentMod", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Port", b =>
                {
                    b.Navigation("SectionSinglePorts");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.Section", b =>
                {
                    b.Navigation("SectionSingles");
                });

            modelBuilder.Entity("Management.Core.Models.DepartmentModels.SectionSingle", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("SectionSinglePorts");
                });
#pragma warning restore 612, 618
        }
    }
}
