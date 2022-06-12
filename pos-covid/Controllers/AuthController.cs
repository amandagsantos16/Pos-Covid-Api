using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pos_covid_api.Data;
using pos_covid_api.Extensions;
using pos_covid_api.Funcoes;
using pos_covid_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using pos_covid_api.ViewModels;

namespace pos_covid_api.Controllers;

[Route("api/identidades")]
public class AuthController : MainController
{
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;
    private readonly FuncoesSenha _funcoesSenha;

    public AuthController(IOptions<AppSettings> appSettings, ApplicationDbContext context, FuncoesSenha funcoesSenha)
    {
        _appSettings = appSettings.Value;
        _context = context;
        _funcoesSenha = funcoesSenha;
    }

    [HttpPost]
    [Route("nova-conta")]
    public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (ModelState.IsValid == false) 
            return CustomResponse(ModelState);

        var user = await _context.Usuarios.Where(x => usuarioRegistro.Email.Equals(x.Email)).FirstOrDefaultAsync();
        
        if (user is not null)
        {
            AdicionarErroProcessamento("Usuário já cadastrado.");
            return CustomResponse();
        }
        
        user = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = usuarioRegistro.Email,
            Senha = _funcoesSenha.CriptografarSenha(string.Concat(usuarioRegistro.Senha))
        };

        var paciente = new Paciente()
        {
            Id = Guid.NewGuid(),
            Nome = usuarioRegistro.Nome,
            UsuarioId = user.Id
        };

        await _context.Usuarios.AddAsync(user);
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();

        return CustomResponse(await GerarJwt(user.Email));
    }

    [HttpPost]
    [Route("autenticar")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (ModelState.IsValid == false) 
            return CustomResponse(ModelState);
        
        var usuario = await _context.Usuarios.Where(x => usuarioLogin.Email.Equals(x.Email)).FirstOrDefaultAsync();

        if (usuario is null)
        {
            AdicionarErroProcessamento("Usuário ou Senha incorreto.");
            return CustomResponse();
        }

        if (!_funcoesSenha.VerificarSenha(usuarioLogin.Senha, usuario.Senha))
        {
            AdicionarErroProcessamento("Usuário ou Senha incorreto.");
            return CustomResponse();
        }
        
        return CustomResponse(await GerarJwt(usuarioLogin.Email));
    }

    private async Task<UsuarioRespostaLogin> GerarJwt(string email)
    {
        var user = await _context.Usuarios.Where(x => email.Equals(x.Email)).FirstOrDefaultAsync();

        var ret = ObterClaimsUsuario(user);
        var encodedToken = CodificarToken(ret.Item1);

        return ObterRepostaToken(encodedToken, user, ret.Item2);
    }

    private (ClaimsIdentity, ICollection<Claim>) ObterClaimsUsuario(Usuario user)
    {
        var claims = new List<Claim>();
        
        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return (identityClaims, claims);
    }

    private string CodificarToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
        {
            Issuer = _appSettings.Emissor,
            Audience = _appSettings.ValidoEm,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }

    private UsuarioRespostaLogin ObterRepostaToken(string encodedToken, Usuario user, ICollection<Claim> claims)
    {
        var paciente = _context.Pacientes.Where(x => x.UsuarioId == user.Id).FirstOrDefault();
        var psicologo = _context.Psicologos.Where(x => x.UsuarioId == user.Id).FirstOrDefault();
        
        var response = new UsuarioRespostaLogin
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = user.Id.ToString(),
                PacienteId = paciente?.Id,
                PsicologoId = psicologo?.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };

        return response;
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);
}