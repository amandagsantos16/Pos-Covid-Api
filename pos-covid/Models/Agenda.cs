using pos_covid_api.Enums;

namespace pos_covid_api.Models;

public class Agenda
{
    public Guid Id { get; set; }
    public Guid PsicologoId { get; set; }
    public Guid PacienteId { get; set; }
    public Guid HorarioId { get; set; }
    public EnumStatusAgendamento StatusAgendamento { get; set; }

    public Paciente? Paciente { get; set; }
    public Psicologo? Psicologo { get; set; }
    public Horario? Horario { get; set; }
}