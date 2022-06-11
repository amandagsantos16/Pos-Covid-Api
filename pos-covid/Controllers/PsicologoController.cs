using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_covid_api.Data;
using pos_covid_api.Models;
using pos_covid_api.ViewModels;

namespace pos_covid_api.Controllers;

[Authorize]
[Route("api/psicologos")]
public class PsicologoController : MainController
{
    private readonly ApplicationDbContext _context;

    public PsicologoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
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

    [HttpGet]
    public async Task<IActionResult> ObterListagemDePsicologos()
    {
        var psicologos = await _context.Psicologos.Where(x => x.RegistroValido).ToListAsync();

        return CustomResponse(psicologos);
    }

    [HttpPost]
    [Route("horarios")]
    public async Task<IActionResult> AdicionarHorarios(AdicionarHorariosPsicologo request)
    {
        foreach (var horarioRequest in request.Horarios)
        {
            foreach (var hora in horarioRequest.Horarios)
            {
                var horaFormatada = DateTime.ParseExact(hora, "HH:mm",
                    CultureInfo.CreateSpecificCulture("pt-BR"));

                var horario = new Horario
                {
                    Id = Guid.NewGuid(),
                    Hora = horaFormatada,
                    PsicologoId = request.PsicologoId,
                    DiaDaSemana = (int) horarioRequest.DiaDaSemana
                };

                await _context.Horarios.AddAsync(horario);
                await _context.SaveChangesAsync();
            }
        }

        var retorno = await _context.Horarios.Where(x => x.PsicologoId == request.PsicologoId).ToListAsync();

        return Created("", retorno);
    }

    [HttpGet]
    [Route("horarios")]
    public async Task<IActionResult> ObterHorariosDisponiveisPorData([FromQuery] DateTime? data, [FromQuery] Guid? psicologoId)
    {
        if (data is null)
            AdicionarErroProcessamento("Data é obrigatória para a busca de horários.");
        if (psicologoId is null)
            AdicionarErroProcessamento("Psicologo é obrigatório para a busca de horários.");
        if (IsValid == false)
            return CustomResponse();

        var diaDaSemana = data.Value.DayOfWeek;
        
        var horaAgora = DateTime.Now.TimeOfDay;

        var horarios = await _context.Horarios
            .Where(x => x.PsicologoId == psicologoId && x.DiaDaSemana == (int)diaDaSemana && x.Hora.TimeOfDay >= horaAgora)
            .Include(s => s.Agendamentos)
            .OrderBy(x => x.Hora)
            .ToListAsync();

        var horariosToBeRemoved = new List<Horario>();
        foreach (var horario in horarios)
        {
            if (horario.Agendamentos.Any())
            {
                horariosToBeRemoved.Add(horario);
            }
        }

        foreach (var horarioBeRemove in horariosToBeRemoved)
        {
            horarios.Remove(horarioBeRemove);
        }
        
        return Ok(horarios);
    }
}