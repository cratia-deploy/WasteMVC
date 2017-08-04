using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WasteMVC.Data;

namespace WasteMVC.Migrations
{
    [DbContext(typeof(SystemContext))]
    [Migration("20170804140829_ADD_Entitys_WasteType_Waste_Partner")]
    partial class ADD_Entitys_WasteType_Waste_Partner
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WasteMVC.Models.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_At");

                    b.Property<DateTime>("Deleted_At");

                    b.Property<double>("Percentage");

                    b.Property<int?>("PersonId");

                    b.Property<DateTime>("Updated_At");

                    b.Property<int?>("WasteId");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("WasteId");

                    b.ToTable("Partner");
                });

            modelBuilder.Entity("WasteMVC.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_At");

                    b.Property<DateTime>("Deleted_At");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("Updated_At");

                    b.HasKey("Id");

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique();

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("WasteMVC.Models.Waste", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<DateTime>("Created_At");

                    b.Property<DateTime>("DateTime");

                    b.Property<DateTime>("Deleted_At");

                    b.Property<double>("SalePrice");

                    b.Property<DateTime>("Updated_At");

                    b.Property<int?>("WasteTypeId");

                    b.Property<double>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("WasteTypeId");

                    b.ToTable("Waste");
                });

            modelBuilder.Entity("WasteMVC.Models.WasteType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created_At");

                    b.Property<DateTime>("Deleted_At");

                    b.Property<string>("Description");

                    b.Property<DateTime>("Updated_At");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    b.ToTable("WasteType");
                });

            modelBuilder.Entity("WasteMVC.Models.Partner", b =>
                {
                    b.HasOne("WasteMVC.Models.Person", "Person")
                        .WithMany("Business")
                        .HasForeignKey("PersonId");

                    b.HasOne("WasteMVC.Models.Waste", "Waste")
                        .WithMany("Partners")
                        .HasForeignKey("WasteId");
                });

            modelBuilder.Entity("WasteMVC.Models.Waste", b =>
                {
                    b.HasOne("WasteMVC.Models.WasteType", "WasteType")
                        .WithMany("Wastes")
                        .HasForeignKey("WasteTypeId");
                });
        }
    }
}
