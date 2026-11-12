using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVCproject.Models
{
    public class SignUpViewModel 
    {
        // Datos mínimos para el registro
        [Required(ErrorMessage = "Debe completar el nombre")]
        [DisplayName("Nombre")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Debe escribir un correo electrónico")]
        [DisplayName("Correo electrónico")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 dígitos")]
        [DisplayName("Contraseña")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar la Contraseña.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmPassword { get; set; }

        [DisplayName("Recuérdame")]
        public bool RememberMe { get; set; } = false;

        // Datos personales

        //[Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "Debe ingresar un número de teléfono válido")]
        [DisplayName("Teléfono")]
        public string? PhoneNumber { get; set; }

    }
}
