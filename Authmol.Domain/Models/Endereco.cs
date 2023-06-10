using Microsoft.AspNetCore.Identity;

namespace Authmol.Domain.Models;

public class Endereco
{
    public int EnderecoId { get; set; }
    public required string UserId { get; set; }
    public IdentityUser User { get; set; }
    public required string CEP { get; set; }
    public required string Estado { get; set; }
    public required string Cidade { get; set; }
    public required string Bairro { get; set; }
    public required string Logradouro { get; set; }
    public string? Complemento { get; set; }
}
