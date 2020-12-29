﻿// <auto-generated />
using System;
using ChatJS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatJS.Data.Migrations.ApplicationMigrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ChatJS.Domain.Chatrooms.Chatroom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameCaption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Chatrooms");
                });

            modelBuilder.Entity("ChatJS.Domain.Deliveries.Delivery", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ReadAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReceivedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("UserId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("ChatJS.Domain.Memberships.Membership", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ChatroomId");

                    b.HasIndex("ChatroomId");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("ChatJS.Domain.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Attachment")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatJS.Domain.Posts.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChatroomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChatroomId");

                    b.HasIndex("MessageId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ChatJS.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DisplayNameUid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DisplayName", "DisplayNameUid")
                        .IsUnique()
                        .HasFilter("[DisplayName] IS NOT NULL AND [DisplayNameUid] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatJS.Domain.Deliveries.Delivery", b =>
                {
                    b.HasOne("ChatJS.Domain.Messages.Message", "Message")
                        .WithMany("Deliveries")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatJS.Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatJS.Domain.Memberships.Membership", b =>
                {
                    b.HasOne("ChatJS.Domain.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("Memberships")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatJS.Domain.Users.User", "User")
                        .WithMany("Memberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatJS.Domain.Messages.Message", b =>
                {
                    b.HasOne("ChatJS.Domain.Users.User", "CreatedByUser")
                        .WithMany("Messages")
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("ChatJS.Domain.Posts.Post", b =>
                {
                    b.HasOne("ChatJS.Domain.Chatrooms.Chatroom", "Chatroom")
                        .WithMany("Posts")
                        .HasForeignKey("ChatroomId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatJS.Domain.Messages.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Chatroom");

                    b.Navigation("Message");
                });

            modelBuilder.Entity("ChatJS.Domain.Chatrooms.Chatroom", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("ChatJS.Domain.Messages.Message", b =>
                {
                    b.Navigation("Deliveries");
                });

            modelBuilder.Entity("ChatJS.Domain.Users.User", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
