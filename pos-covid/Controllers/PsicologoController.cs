using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_covid_api.Data;
using pos_covid_api.Models;
using pos_covid_api.ViewModels;

namespace pos_covid_api.Controllers;

[Route("api/psicologos")]
public class PsicologoController : MainController
{
    private readonly ApplicationDbContext _context;

    public PsicologoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AdicionarPsicologo(AdicionarPsicologoViewModel request)
    {
        var psicologo = await _context.Psicologos.Where(x => request.Crp.Equals(x.CRP)).FirstOrDefaultAsync();

        if (psicologo is not null)
        {
            AdicionarErroProcessamento("Psicólogo já cadastrado no sistema");
            return CustomResponse();
        }
        
        psicologo = new Psicologo
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            DataNascimento = request.DataNascimento,
            CRP = request.Crp,
            Resumo = request.Resumo,
            Especializacoes = request.Resumo
        };
        
        await _context.Psicologos.AddAsync(psicologo);
        await _context.SaveChangesAsync();

        return Created("", psicologo);
    }
}