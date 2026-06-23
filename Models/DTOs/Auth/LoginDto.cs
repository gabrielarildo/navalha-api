using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo e-mail deve ser um endereço de e-mail válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        public string Senha { get; set; }
    }
}