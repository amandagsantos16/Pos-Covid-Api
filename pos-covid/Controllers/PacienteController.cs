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
        var paciente = await _context.Pacientes.Where(x => x.Id == request.PacienteId).FirstOrDefaultAsync();
        var psicologo = await _context.Psicologos.Where(x => x.Id == request.PsicologoId).FirstOrDefaultAsync();

        var agendamento = new Agenda
        {
            Id = new Guid(),
            PacienteId = request.PacienteId,
            PsicologoId = request.PsicologoId,
            HorarioId = request.HorarioId,
            Data = request.Data,
            StatusAgendamento = EnumStatusAgendamento.Pendente,
            Notificacoes = new List<Notificacao>()
            {
                new()
                {
                    Id = new Guid(),
                    Mensagem = $"{paciente.Nome} solicitou atendimento com {psicologo.Nome}"
                }
            }
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
                .Include(x => x.Psicologo)
                .Include(x => x.Paciente)
                .Include(x => x.Horario)
                .ToListAsync();

        return CustomResponse(agendamentos);
    }

    [HttpPut]
    [Route("agendamentos")]
    public async Task<IActionResult> AlterarAgendamento(AlterarAgendamentoViewModel request)
    {
        var agendamento = await _context.Agendamentos.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        var paciente = await _context.Pacientes.Where(x => x.Id == agendamento.PacienteId).FirstOrDefaultAsync();
        var psicologo = await _context.Psicologos.Where(x => x.Id == agendamento.PsicologoId).FirstOrDefaultAsync();

        if (agendamento is null)
        {
            AdicionarErroProcessamento("Agendamento não encontrado");
            return CustomResponse();
        }

        if (agendamento.StatusAgendamento == EnumStatusAgendamento.Cancelado)
        {
            AdicionarErroProcessamento("Agendamento cancelado, não é possível alterar");
            return CustomResponse();
        }

        var notificacao = new Notificacao
        {
            Id = new Guid(),
            Mensagem = $"{paciente.Nome} solicitou atendimento com {psicologo.Nome}"
        };
        
        agendamento.Data = request.Data;
        agendamento.HorarioId = request.HorarioId;
        agendamento.StatusAgendamento = EnumStatusAgendamento.Pendente;
        agendamento.Notificacoes.Add(notificacao);
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    [Route("agendamentos")]
    public async Task<IActionResult> ExclusaoAgendamento(Guid? agendamentoId)
    {
        var agendamento = await _context.Agendamentos.Where(x => x.Id == agendamentoId).FirstOrDefaultAsync();
        var paciente = await _context.Pacientes.Where(x => x.Id == agendamento.PacienteId).FirstOrDefaultAsync();
        var psicologo = await _context.Psicologos.Where(x => x.Id == agendamento.PsicologoId).FirstOrDefaultAsync();

        if (agendamento is null)
        {
            AdicionarErroProcessamento("Agendamento não encontrado");
            return CustomResponse();
        }

        if (agendamento.StatusAgendamento == EnumStatusAgendamento.Cancelado)
        {
            AdicionarErroProcessamento("Agendamento já foi cancelado");
            return CustomResponse();
        }

        var notificacao = new Notificacao
        {
            Id = new Guid(),
            Mensagem = $"{paciente.Nome} cancelou o atendimento com {psicologo.Nome}"
        };

        agendamento.StatusAgendamento = EnumStatusAgendamento.Cancelado;
        agendamento.Notificacoes.Add(notificacao);
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPost]
    [Route("agendamentos/confirmacao")]
    public async Task<IActionResult> ConfirmarAgendamento(Guid? agendamentoId)
    {
        var agendamento = await _context.Agendamentos.Where(x => x.Id == agendamentoId).FirstOrDefaultAsync();
        var paciente = await _context.Pacientes.Where(x => x.Id == agendamento.PacienteId).FirstOrDefaultAsync();
        var psicologo = await _context.Psicologos.Where(x => x.Id == agendamento.PsicologoId).FirstOrDefaultAsync();
        
        if (agendamento is null)
        {
            AdicionarErroProcessamento("Agendamento não encontrado");
            return CustomResponse();
        }

        if (agendamento.StatusAgendamento == EnumStatusAgendamento.Confirmado)
        {
            AdicionarErroProcessamento("Agendamento já foi confirmado");
            return CustomResponse();
        }
        
        var notificacao = new Notificacao
        {
            Id = new Guid(),
            Mensagem = $"{paciente.Nome} confirmou o atendimento com o psicólogo {psicologo.Nome}"
        };

        agendamento.StatusAgendamento = EnumStatusAgendamento.Confirmado;
        agendamento.Notificacoes.Add(notificacao);
        _context.Agendamentos.Update(agendamento);

        return Ok();
    }
}