namespace pos_covid_api.Models;

public class Psicologo
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public string CRP { get; set; }
    public string Resumo { get; set; }
    public string Especializacoes { get; set; }
    public Guid? UsuarioId { get; set; }
    public bool RegistroValido { get; private set; }

    public Usuario? Usuario { get; set; }
}