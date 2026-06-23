using System.ComponentModel.DataAnnotations;
using TodoApi.Models.Entities;

namespace TodoApi.Models.DTOs.AgendamentoDto
{
    // Usado para confirmar, concluir ou cancelar um agendamento
    public class AgendamentoStatusUpdateDto
    {
        [Required(ErrorMessage = "O status é obrigatório.")]
        public StatusAgendamento Status { get; set; }
    }
}
