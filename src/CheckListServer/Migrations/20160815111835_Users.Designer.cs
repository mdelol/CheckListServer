using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DAL.Contexts;

namespace CheckListServer.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20160815111835_Users")]
    partial class Users
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL.Models.Task", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsCompleted");

                    b.Property<int?>("UserId");

                    b.HasKey("TaskId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("DAL.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PasswordHash");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL.Models.UserToken", b =>
                {
                    b.Property<int>("UserTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Token");

                    b.Property<int?>("UserId");

                    b.HasKey("UserTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("DAL.Models.Task", b =>
                {
                    b.HasOne("DAL.Models.User")
                        .WithMany("Tasks")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DAL.Models.UserToken", b =>
                {
                    b.HasOne("DAL.Models.User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId");
                });
        }
    }
}
