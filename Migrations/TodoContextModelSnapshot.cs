﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskAdmin.Data;

#nullable disable

namespace TaskAdmin.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("TaskAdmin.Models.TodoTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ListType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TaskName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TodoTasks", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}