namespace pos_covid_api.ViewModels;

public class AlterarHorarioPsicologo
{
    public Guid PsicologoId { get; set; }
    public DayOfWeek DiaDaSemana { get; set; }
    public IList<string> Horarios { get; set; }
}