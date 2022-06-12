namespace pos_covid_api.ViewModels;

public class AlterarAgendamentoViewModel
{
    public Guid? Id { get; set; }
    public Guid? HorarioId { get; set; }
    public DateTime Data { get; set; }
}