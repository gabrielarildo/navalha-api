using TodoApi.Models.Entities;

namespace TodoApi.Data
{
    public static class DbSeeder
    {
        // Garante que o catálogo padrão de serviços da Navalha exista no banco
        public static void SeedServicos(AppDbContext context)
        {
            if (context.Servicos.Any()) return;

            var servicos = new List<Servico>
            {
                new() { Id = Guid.NewGuid(), Nome = "Corte Masculino", Descricao = "Corte tradicional masculino.", Valor = 45.00m, DuracaoMinutos = 30, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Barba", Descricao = "Modelagem e acabamento de barba.", Valor = 35.00m, DuracaoMinutos = 20, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Corte + Barba", Descricao = "Combo corte masculino com barba.", Valor = 70.00m, DuracaoMinutos = 50, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Degradê", Descricao = "Corte degradê (fade) com máquina.", Valor = 50.00m, DuracaoMinutos = 40, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Sobrancelha", Descricao = "Design e alinhamento de sobrancelha.", Valor = 20.00m, DuracaoMinutos = 15, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Pigmentação", Descricao = "Pigmentação de barba ou cabelo.", Valor = 60.00m, DuracaoMinutos = 45, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Hidratação Capilar", Descricao = "Tratamento de hidratação capilar.", Valor = 40.00m, DuracaoMinutos = 30, Ativo = true },
            };

            context.Servicos.AddRange(servicos);
            context.SaveChanges();
        }
    }
}
