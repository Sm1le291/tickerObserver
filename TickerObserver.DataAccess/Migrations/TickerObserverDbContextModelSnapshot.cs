﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TickerObserver.DataAccess;

namespace TickerObserver.DataAccess.Migrations
{
    [DbContext(typeof(TickerObserverDbContext))]
    partial class TickerObserverDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799");

            modelBuilder.Entity("TickerObserver.DataAccess.Models.TickerTopic", b =>
                {
                    b.Property<int>("TickerTopicId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("FullUrl");

                    b.Property<string>("Guid");

                    b.Property<bool>("IsSentAlready");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("RssFeedSource");

                    b.Property<string>("Source");

                    b.Property<string>("TickerName");

                    b.Property<string>("Title");

                    b.HasKey("TickerTopicId");

                    b.ToTable("TickerTopics");
                });
#pragma warning restore 612, 618
        }
    }
}