namespace pos_covid_api.ViewModels;

public class AdicionarHorariosPsicologo
{
    public Guid PsicologoId { get; set; }
    public IEnumerable<AdicionarHorariosPsicologoData> Horarios { get; set; }
}

public class AdicionarHorariosPsicologoData
{
    public DayOfWeek DiaDaSemana { get; set; }
    public IList<string> Horarios { get; set; }
}