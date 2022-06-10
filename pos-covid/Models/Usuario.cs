namespace pos_covid_api.Models;

public class Usuario
{
    public Guid Id { get; set; }
    public string Senha { get; set; }
    public string Email { get; set; }

    public Paciente? Paciente { get; set; }
    public Psicologo? Psicologo { get; set; }
}