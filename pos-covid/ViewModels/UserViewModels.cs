using System.ComponentModel.DataAnnotations;

namespace pos_covid_api.ViewModels;

public class UsuarioRegistro
{
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string SenhaConfirmacao { get; set; }

    public string Nome { get; set; }
}

public class UsuarioLogin
{
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; }
}

public class UsuarioRespostaLogin
{
    public string AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UsuarioToken UsuarioToken { get; set; }
}

public class UsuarioToken
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IEnumerable<UsuarioClaim> Claims { get; set; }
    public Guid? PacienteId { get; set; }
    public Guid? PsicologoId { get; set; }
}

public class UsuarioClaim
{
    public string Value { get; set; }
    public string Type { get; set; }
}