using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_covid_api.Data;
using pos_covid_api.Enums;
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
            Especializacoes = request.Especializacoes,
            RegistroValido = true,
            UsuarioId = request.UsuarioId
        };

        await _context.Psicologos.AddAsync(psicologo);
        await _context.SaveChangesAsync();

        return Created("", psicologo);
    }

    [HttpGet]
    public async Task<IActionResult> ObterListagemDePsicologos(Guid? usuarioId)
    {
        var psicologos = await _context.Psicologos.Where(x => x.RegistroValido && x.UsuarioId != usuarioId).ToListAsync();

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
    [Route("horarios-por-data")]
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
            .Where(x => x.PsicologoId == psicologoId && x.DiaDaSemana == (int)diaDaSemana)
            .Include(s => s.Agendamentos.Where(x => x.StatusAgendamento != EnumStatusAgendamento.Cancelado))
            .OrderBy(x => x.Hora)
            .ToListAsync();

        if (data.Value.Date == DateTime.Now.Date)
        {
            horarios = horarios.Where(x => x.Hora.TimeOfDay >= horaAgora).ToList();
        }

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
    
    [HttpGet]
    [Route("horarios-por-dia")]
    public async Task<IActionResult> ObterHorariosPorDia([FromQuery] DayOfWeek diaDaSemana, [FromQuery] Guid? psicologoId)
    {
        var horarios = await _context.Horarios
            .Where(x => x.PsicologoId == psicologoId && x.DiaDaSemana == (int)diaDaSemana)
            .OrderBy(x => x.Hora)
            .ToListAsync();
        
        return Ok(horarios);
    }

    [HttpGet]
    [Route("agendamentos")]
    public async Task<IActionResult> ListagemDeAgendamentos(Guid? psicologoId)
    {
        var agendamentos =
            await _context.Agendamentos.
                Where(x => x.PsicologoId == psicologoId && x.Data.Date >= DateTime.Now.Date && 
                           x.StatusAgendamento != EnumStatusAgendamento.Cancelado)
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
        var agendamento = await _context.Agendamentos.Where(x => x.Id == request.Id)
            .Include(x => x.Notificacoes).FirstOrDefaultAsync();
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
            Mensagem = $"{psicologo.Nome} alterou o atendimento com o paciente {paciente.Nome}"
        };

        agendamento.Data = request.Data;
        agendamento.HorarioId = request.HorarioId;
        agendamento.StatusAgendamento = EnumStatusAgendamento.PendentePaciente;
        agendamento.Notificacoes.Add(notificacao);
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpDelete]
    [Route("agendamentos")]
    public async Task<IActionResult> ExclusaoAgendamento(Guid? agendamentoId)
    {
        var agendamento = await _context.Agendamentos.Where(x => x.Id == agendamentoId)
            .Include(x => x.Notificacoes).FirstOrDefaultAsync();
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
            Mensagem = $"{psicologo.Nome} cancelou o atendimento com o paciente {paciente.Nome}"
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
        var agendamento = await _context.Agendamentos.Where(x => x.Id == agendamentoId)
            .Include(x => x.Notificacoes).FirstOrDefaultAsync();
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
            Mensagem = $"{psicologo.Nome} confirmou o atendimento com o paciente {paciente.Nome}"
        };

        agendamento.StatusAgendamento = EnumStatusAgendamento.Confirmado;
        agendamento.Notificacoes.Add(notificacao);
        _context.Agendamentos.Update(agendamento);

        return Ok();
    }
}