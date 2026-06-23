using Microsoft.EntityFrameworkCore;
using TodoApi.Models.Entities;

namespace TodoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Barbeiro> Barbeiros { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Armazena o enum de status como texto no banco, facilitando leitura/consulta
            modelBuilder.Entity<Agendamento>()
                .Property(a => a.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Agendamento>()
                .Property(a => a.Valor)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Servico>()
                .Property(s => s.Valor)
                .HasColumnType("decimal(10,2)");

            // Agendamento -> Cliente (1 Cliente para N Agendamentos)
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);

            // Agendamento -> Barbeiro (1 Barbeiro para N Agendamentos)
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Barbeiro)
                .WithMany(b => b.Agendamentos)
                .HasForeignKey(a => a.BarbeiroId)
                .OnDelete(DeleteBehavior.NoAction);

            // Agendamento -> Serviço (1 Serviço para N Agendamentos)
            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Servico)
                .WithMany(s => s.Agendamentos)
                .HasForeignKey(a => a.ServicoId)
                .OnDelete(DeleteBehavior.NoAction);

            // Índice para acelerar a verificação de disponibilidade de horário por barbeiro
            modelBuilder.Entity<Agendamento>()
                .HasIndex(a => new { a.BarbeiroId, a.DataHora });

            // E-mail de usuário deve ser único
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
