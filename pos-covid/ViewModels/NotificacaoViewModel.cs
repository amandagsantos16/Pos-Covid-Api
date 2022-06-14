namespace pos_covid_api.ViewModels;

public class NotificacaoViewModel
{
    public Guid Id { get; set; }
    public string? Mensagem { get; set; }
    public Guid? AgendamentoId { get; set; }
    public Guid? PacienteId { get; set; }
    public Guid? PsicologoId { get; set; }
}