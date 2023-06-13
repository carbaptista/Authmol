using System.ComponentModel.DataAnnotations;

namespace Authmol.Persistence.DTOs;
public class InputModel
{
    [Required(ErrorMessage = "*Obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "*Obrigatório")]
    [StringLength(100, ErrorMessage = "A {0} precisa ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Senha")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "*Obrigatório")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirme a senha")]
    [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não são iguais.")]
    public string ConfirmPassword { get; set; } = "";

    [Required(ErrorMessage = "*Obrigatório")]
    [RegularExpression(@"^.*\d{5}-?\d{3}$", ErrorMessage = "CEP inválido")]
    public string CEP { get; set; } = "";

    [Required(ErrorMessage = "*Obrigatório")]
    [DataType(DataType.Text)]
    [Display(Name = "Logradouro")]
    public string Logradouro { get; set; } = "";

    [DataType(DataType.Text)]
    [Display(Name = "Complemento")]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "*Obrigatório")]
    [DataType(DataType.Text)]
    [Display(Name = "Cidade")]
    public string Cidade { get; set; } = "";

    [Required(ErrorMessage = "*Obrigatório")]
    [DataType(DataType.Text)]
    [Display(Name = "Bairro")]
    public string Bairro { get; set; } = "";

    [Required(ErrorMessage = "*Obrigatório")]
    [DataType(DataType.Text)]
    [Display(Name = "Estado")]
    public string Estado { get; set; } = "";
}
