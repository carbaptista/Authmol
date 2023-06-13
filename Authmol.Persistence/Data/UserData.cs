using Authmol.Persistence.DTOs;
using Authmol.Persistence.Models;

namespace Authmol.Persistence.Data;
public class UserData : IUserData
{
    private readonly ApplicationDbContext _context;

    public UserData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CriarEndereco(InputModel input, string userId)
    {
        var newEndereco = new Endereco
        {
            UserId = userId,
            CEP = input.CEP,
            Estado = input.Estado,
            Cidade = input.Cidade,
            Bairro = input.Bairro,
            Logradouro = input.Logradouro,
            Complemento = input.Complemento
        };

        _context.Enderecos.Add(newEndereco);
        await _context.SaveChangesAsync();
    }
}
