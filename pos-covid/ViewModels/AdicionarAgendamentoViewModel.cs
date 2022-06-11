namespace pos_covid_api.ViewModels;

public class AdicionarAgendamentoViewModel
{
    public Guid? PsicologoId { get; set; }
    public Guid? PacienteId { get; set; }
    public Guid? HorarioId { get; set; }
    public DateTime Data { get; set; }
}