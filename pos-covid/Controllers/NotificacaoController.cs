using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pos_covid_api.Data;
using pos_covid_api.ViewModels;

namespace pos_covid_api.Controllers;

[Authorize]
[Route("api/notificacoes")]
public class NotificacaoController : MainController
{
    private readonly ApplicationDbContext _context;

    public NotificacaoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObterNotificacoes(Guid? usuarioId)
    {
        var query = from notificacao in _context.Notificacoes
            join agendamento in _context.Agendamentos on notificacao.AgendamentoId equals agendamento.Id
            join paciente in _context.Pacientes on agendamento.PacienteId equals paciente.Id into pacientes
            from paciente in pacientes.DefaultIfEmpty()
            join psicologo in _context.Psicologos on agendamento.PsicologoId equals psicologo.Id into psicologos
            from psicologo in psicologos.DefaultIfEmpty()
            where psicologo.UsuarioId == usuarioId || paciente.UsuarioId == usuarioId
            select new NotificacaoViewModel
            {
                Id = notificacao.Id,
                Mensagem = notificacao.Mensagem,
                AgendamentoId = notificacao.AgendamentoId,
                PacienteId = paciente.Id,
                PsicologoId = psicologo.Id
            };
        
        return Ok(await query.ToListAsync());
    }
}