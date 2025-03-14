using System;
using System.Collections.Generic;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using MyEvent.Models;

namespace MyEvent.Models;

public partial class MyEventContext : DbContext
{
    public MyEventContext(DbContextOptions<MyEventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Credentials> Credentials { get; set; }

    public virtual DbSet<CreditCard> CreditCard { get; set; }

    public virtual DbSet<Event> Event { get; set; }

    public virtual DbSet<EventHolder> EventHolder { get; set; }

    public virtual DbSet<ECredentials> ECredentials { get; set; }

    public virtual DbSet<EventTag> EventTag { get; set; }

    public virtual DbSet<EventType> EventType { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberTel> MemberTel { get; set; }
    public virtual DbSet<RoleList> RoleList { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<Payment> Payment { get; set; }

    public virtual DbSet<Seat> Seat { get; set; }

    public virtual DbSet<Ticket> Ticket { get; set; }

    public virtual DbSet<TicketType> TicketType { get; set; } 

    public virtual DbSet<TicketTypeList> TicketTypeList { get; set; }

    public virtual DbSet<Venue> Venue { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Credentials>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__Credenti__B0C3AC476950C594");

            entity.Property(e => e.Account).HasMaxLength(40);
            entity.Property(e => e.MemberID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Password).HasMaxLength(30);

            entity.HasOne(d => d.Member).WithOne(p => p.Credentials)
                .HasForeignKey<Credentials>(c => c.MemberID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Credentia__Membe__5DCAEF64");
        });

        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.CardNo).HasName("PK__CreditCa__55FF25F1B9F394E2");

            entity.Property(e => e.CardNo)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.MemberID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SecurityNo)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.ValidMonth)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.ValidYear)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.CreditCard)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CreditCar__Membe__628FA481");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventID).HasName("PK__Event__7944C870358F73FF");

            entity.Property(e => e.EventID)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.EventHolderID)
                .HasMaxLength(36);
            entity.Property(e => e.EventName).HasMaxLength(50);
            entity.Property(e => e.EventTypeID)
                .HasMaxLength(3)
                .IsFixedLength();
            entity.Property(e => e.Pic).HasMaxLength(20);
            entity.Property(e => e.Price).HasMaxLength(10);
            entity.Property(e => e.VenueID)
                .HasMaxLength(5)
                .IsFixedLength();

            entity.HasOne(d => d.EventHolder).WithMany(p => p.Event)
                .HasForeignKey(d => d.EventHolderID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__EventHold__2EDAF651");

            entity.HasOne(d => d.EventType).WithMany(p => p.Event)
                .HasForeignKey(d => d.EventTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__EventType__534D60F1");

            entity.HasOne(d => d.Venue).WithMany(p => p.Event)
                .HasForeignKey(d => d.VenueID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Event__VenueID__2DE6D218");

            entity.HasMany(e => e.EventTag)
                .WithOne(et => et.Event)
                .HasForeignKey(et => et.EventID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EventHolder>(entity =>
        {
            entity.HasKey(e => e.EventHolderID).HasName("PK__EventHol__ACE2A24FC6F83D0A");

            entity.Property(e => e.EventHolderID)
                .HasMaxLength(36);
            entity.Property(e => e.Address).HasMaxLength(30);
            entity.Property(e => e.Area)
                .HasMaxLength(10);
            entity.Property(e => e.City)
                .HasMaxLength(5);
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.EventHolderName).HasMaxLength(50);
            entity.Property(e => e.Tel).HasMaxLength(15);
            entity.Property(e => e.ZipCode).HasMaxLength(6);

            entity.HasOne(m => m.ECredentials)
           .WithOne(c => c.EventHolder)
           .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ECredentials>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__ECredent__B0C3AC47011D1485");

            entity.Property(e => e.Account).HasMaxLength(40);
            entity.Property(e => e.EventHolderID)
                .HasMaxLength(36);
            entity.Property(e => e.Password).HasMaxLength(30);

            entity.HasOne(d => d.EventHolder).WithOne(p => p.ECredentials)
                .HasForeignKey<ECredentials>(c => c.EventHolderID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ECredenti__Event__3864608B");
        });

        modelBuilder.Entity<EventTag>(entity =>
        {
            entity.HasKey(et => new { et.EventID, et.Tag });

            entity.Property(e => e.EventID)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.Tag).HasMaxLength(20);

            entity.HasOne(d => d.Event).WithMany(e => e.EventTag)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventTag__EventI__5535A963");
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeID).HasName("PK__EventTyp__A9216B1FABFF90B6");

            entity.Property(e => e.EventTypeID)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.EventType1)
                .HasMaxLength(50)
                .HasColumnName("EventType");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B38B532B3D5");

            entity.Property(e => e.MemberID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Address).HasMaxLength(30);
            entity.Property(e => e.Area)
                .HasMaxLength(10);
            entity.Property(e => e.City)
                .HasMaxLength(5);
            entity.Property(e => e.JoinDate).HasColumnType("datetime");
            entity.Property(e => e.MemberName).HasMaxLength(50);
            entity.Property(e => e.ZipCode).HasMaxLength(6);
            entity.Property(e => e.Role).HasMaxLength(1).IsFixedLength();

            entity.HasOne(m => m.Credentials)
                   .WithOne(c => c.Member)
                   .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.RoleList).WithMany(e => e.Member)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Member__Role__245D67DE");
        });

        modelBuilder.Entity<MemberTel>(entity =>
        {
            entity.HasKey(mt => new { mt.MemberID, mt.Tel });

            entity.Property(e => e.MemberID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Tel).HasMaxLength(15);

            entity.HasOne(d => d.Member).WithMany(e => e.MemberTel)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberTel__Membe__5AEE82B9");
        });

        modelBuilder.Entity<RoleList>(entity =>
        {
            entity.HasKey(e => e.RoleID).HasName("PK__RoleList");

            entity.Property(e => e.RoleID).HasMaxLength(1).IsFixedLength();
            entity.Property(e => e.RoleName).HasMaxLength(15);
        });

            modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("PK__Order__C3905BAFC90BF40E");

            entity.Property(e => e.OrderID)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.EventID)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.MemberID)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Qty).HasDefaultValue(1);

            entity.HasOne(d => d.Event).WithMany(p => p.Order)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__EventID__2FCF1A8A");

            entity.HasOne(d => d.Member).WithMany(p => p.Order)
                .HasForeignKey(d => d.MemberID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__MemberID__30C33EC3");

            entity.HasOne(d => d.Payment).WithMany(p => p.Order)
                .HasForeignKey(d => d.PaymentID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__PaymentID__31B762FC");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.SeatID }).HasName("PK__OrderDet__E0812A924EAEBB87");

            entity.Property(e => e.OrderID)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.SeatID)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.TicketID).HasMaxLength(15);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => d.OrderID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__7C4F7684");

            entity.HasOne(d => d.Seat).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => d.SeatID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__SeatI__7D439ABD");

            entity.HasOne(d => d.Ticket).WithMany(p => p.OrderDetail)
                .HasForeignKey(d => d.TicketID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Ticke__7E37BEF6");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentID).HasName("PK__Payment__9B556A5875A298C7");

            entity.Property(e => e.PaymentID).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(15);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatID).HasName("PK__Seat__311713D3EA7FE4C2");

            entity.Property(e => e.SeatID)
                .HasMaxLength(7)
                .IsFixedLength();

            entity.Property(e => e.Number)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Row)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.SeatType).HasMaxLength(20);
            entity.Property(e => e.VenueID)
                .HasMaxLength(5)
                .IsFixedLength();
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.Venue).WithMany(p => p.Seat)
                .HasForeignKey(d => d.VenueID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seat__VenueID__6A30C649");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketID).HasName("PK__Ticket__712CC6275F174448");

            entity.Property(e => e.TicketID).HasMaxLength(15);
            entity.Property(e => e.TicketTypeID)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.HasOne(d => d.TicketTypeList).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__TypeID__71D1E811");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            
            entity.HasKey(tt => new { tt.EventID, tt.TicketTypeID });

            entity.Property(e => e.EventID)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.TicketTypeID)
                .HasMaxLength(2)
                .IsFixedLength();

            entity.HasOne(d => d.Event).WithMany(e => e.TicketType)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketTyp__Event__7849DB76");

            entity.HasOne(d => d.TicketTypeList).WithMany(e => e.TicketType)
                .HasForeignKey(d => d.TicketTypeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketTyp__Ticke__793DFFAF");
        });
       

        modelBuilder.Entity<TicketTypeList>(entity =>
        {
            entity.HasKey(e => e.TicketTypeID).HasName("PK__TicketTy__516F0395D3F1980E");

            entity.Property(e => e.TicketTypeID)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(10);
        });

        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.VenueID).HasName("PK__Venue__3C57E5D275B2916E");

            entity.Property(e => e.VenueID)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Address).HasMaxLength(30);
            entity.Property(e => e.Area)
                .HasMaxLength(10);
            entity.Property(e => e.City)
                .HasMaxLength(5);

            entity.Property(e => e.VenueName).HasMaxLength(50);
            entity.Property(e => e.Region)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.ZipCode).HasMaxLength(6);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
