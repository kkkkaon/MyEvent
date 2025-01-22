using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyEvent.DTOs;

namespace MyEvent.Models;

public partial class MyEventContext2 : MyEventContext
{
    public MyEventContext2(DbContextOptions<MyEventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EventDTO> EventDTO { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventDTO>(entity => entity.HasNoKey());

        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
