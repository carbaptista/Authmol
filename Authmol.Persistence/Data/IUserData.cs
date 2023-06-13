using Authmol.Persistence.DTOs;

namespace Authmol.Persistence.Data;
public interface IUserData
{
    Task CriarEndereco(InputModel input, string userId);
}