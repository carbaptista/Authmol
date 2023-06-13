using Authmol.Persistence.DTOs;

namespace Authmol.Application.Services;
public interface IUserService
{
    Task CriarEndereco(InputModel input, string userId);
}