namespace pos_covid_api.Models;

public class Notificacao
{
    public Guid Id { get; set; }
    public string? Mensagem { get; set; }
    public Guid? AgendamentoId { get; set; }

    public Agenda? Agendamento { get; set; }
}