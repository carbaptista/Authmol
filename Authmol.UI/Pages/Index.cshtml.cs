using Authmol.Persistence.Data;
using Authmol.Persistence.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Authmol.UI.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public EnderecoDto? Endereco { get; set; }

    public async Task OnGet()
    {
        var userId = _userManager.GetUserId(User);
        var endereco = await _context.Enderecos
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync();
        if (endereco is not null)
        {
            Endereco = new EnderecoDto
            {
                CEP = endereco.CEP,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Logradouro = endereco.Logradouro,
                Complemento = endereco.Complemento
            };
        }
    }
}
