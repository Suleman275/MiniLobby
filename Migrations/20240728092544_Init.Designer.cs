﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniLobby.Data;

#nullable disable

namespace MiniLobby.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240728092544_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MiniLobby.Models.Lobby", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<int>("MemberLimit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lobbies");
                });

            modelBuilder.Entity("MiniLobby.Models.LobbyData", b =>
                {
                    b.Property<Guid>("LobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("LobbyId", "Key");

                    b.ToTable("LobbyData");
                });

            modelBuilder.Entity("MiniLobby.Models.LobbyMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CurrentLobbyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("LobbyMembers");
                });

            modelBuilder.Entity("MiniLobby.Models.MemberData", b =>
                {
                    b.Property<Guid>("MemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Visibility")
                        .HasColumnType("int");

                    b.HasKey("MemberId", "Key");

                    b.ToTable("MemberData");
                });
#pragma warning restore 612, 618
        }
    }
}
