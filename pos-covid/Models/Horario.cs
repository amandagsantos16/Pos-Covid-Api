namespace pos_covid_api.Models;

public class Horario
{
    public Guid Id { get; set; }
    public int DiaDaSemana { get; set; }
    public DateTime Hora { get; set; }
    public Guid? PsicologoId { get; set; }

    public Psicologo? Psicologo { get; set; }
    public IEnumerable<Agenda> Agendamentos { get; set; }
}