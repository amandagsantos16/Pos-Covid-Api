using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_covid_api.Data;
using pos_covid_api.Enums;
using pos_covid_api.Models;
using pos_covid_api.ViewModels;

namespace pos_covid_api.Controllers;

[Authorize]
[Route("api/pacientes")]
public class PacienteController : MainController
{
    private readonly ApplicationDbContext _context;

    public PacienteController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Route("agendamentos")]
    public async Task<IActionResult> NovoAgendamento(AdicionarAgendamentoViewModel request)
    {
        var agendamento = new Agenda
        {
            PacienteId = request.PacienteId,
            PsicologoId = request.PsicologoId,
            HorarioId = request.HorarioId,
            Data = request.Data,
            StatusAgendamento = EnumStatusAgendamento.Pendente
        };

        await _context.Agendamentos.AddAsync(agendamento);
        await _context.SaveChangesAsync();

        return Created("", agendamento);
    }

    [HttpGet]
    [Route("agendamentos")]
    public async Task<IActionResult> ListagemDeAgendamentos(Guid? pacienteId)
    {
        var agendamentos =
            await _context.Agendamentos.Where(x => x.PacienteId == pacienteId && x.Data.Date >= DateTime.Now.Date)
                .ToListAsync();

        return CustomResponse(agendamentos);
    }
}