namespace Authmol.Persistence.DTOs;
public class EnderecoDto
{
    public required string CEP { get; set; }
    public required string Estado { get; set; }
    public required string Cidade { get; set; }
    public required string Bairro { get; set; }
    public required string Logradouro { get; set; }
    public string? Complemento { get; set; }
}
