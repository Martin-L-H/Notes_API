﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notes_API_DAL.Context;

#nullable disable

namespace Notes_API_DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Notes_API_CORE.Entities.Note", b =>
                {
                    b.Property<int>("Note_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Note_Id"));

                    b.HasKey("Note_Id");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Notes_API_CORE.Entities.User", b =>
                {
                    b.Property<int>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("User_Id"));

                    b.Property<bool>("User_Disabled")
                        .HasColumnType("bit");

                    b.Property<string>("User_Mail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User_Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("User_Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("User_Verified")
                        .HasColumnType("bit");

                    b.HasKey("User_Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Notes_API_CORE.Entities.UserNote", b =>
                {
                    b.Property<int>("User_Id")
                        .HasColumnType("int");

                    b.Property<int>("Note_Id")
                        .HasColumnType("int");

                    b.Property<string>("Note_Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("User_Id", "Note_Id");

                    b.HasIndex("Note_Id");

                    b.ToTable("UsersNotes");
                });

            modelBuilder.Entity("Notes_API_CORE.Entities.UserNote", b =>
                {
                    b.HasOne("Notes_API_CORE.Entities.Note", "_note")
                        .WithMany("_usersnotes")
                        .HasForeignKey("Note_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Notes_API_CORE.Entities.User", "_user")
                        .WithMany("_usersnotes")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("_note");

                    b.Navigation("_user");
                });

            modelBuilder.Entity("Notes_API_CORE.Entities.Note", b =>
                {
                    b.Navigation("_usersnotes");
                });

            modelBuilder.Entity("Notes_API_CORE.Entities.User", b =>
                {
                    b.Navigation("_usersnotes");
                });
#pragma warning restore 612, 618
        }
    }
}
