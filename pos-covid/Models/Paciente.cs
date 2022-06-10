namespace pos_covid_api.Models;

public class Paciente
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid? UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }
}