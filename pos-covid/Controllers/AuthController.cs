using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pos_covid_api.Data;
using pos_covid_api.Extensions;
using pos_covid_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pos_covid_api.Controllers;

[Route("api/identidades")]
public class AuthController : MainController
{
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;

    public AuthController(IOptions<AppSettings> appSettings, ApplicationDbContext context)
    {
        _appSettings = appSettings.Value;
        _context = context;
    }

    [HttpPost]
    [Route("nova-conta")]
    public async Task<IActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (ModelState.IsValid == false) return CustomResponse(ModelState);

        var user = new Usuario
        {
            Id = Guid.NewGuid(),
            Email = usuarioRegistro.Email,
            Senha = usuarioRegistro.Senha
        };

        await _context.Usuarios.AddAsync(user);

        await GerarJwt(user.Email);

        return CustomResponse();
    }

    [HttpPost]
    [Route("autenticar")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (ModelState.IsValid == false) 
            return CustomResponse(ModelState);

        //var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha,
        //    false, true);

        //if (result.Succeeded)
        //{
        //    return CustomResponse(await GerarJwt(usuarioLogin.Email));
        //}

        //if (result.IsLockedOut)
        //{
        //    AdicionarErroProcessamento("Usuário temporarimente bloqueado por tentativas inválidas");
        //    return CustomResponse();
        //}

        //AdicionarErroProcessamento("Usuário ou Senha incorretos");
        return CustomResponse();
    }

    private async Task<UsuarioRespostaLogin> GerarJwt(string email)
    {
        //var user = await _userManager.FindByEmailAsync(email);
        //var claims = await _userManager.GetClaimsAsync(user);

        //var identityClaims = await ObterClaimsUsuario(claims, user);
        //var encodedToken = CodificarToken(identityClaims);

        //return ObterRepostaToken(encodedToken, user, claims);

        throw new NotImplementedException();
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
    {
        //var userRoles = await _userManager.GetRolesAsync(user);

        //claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        //claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        //foreach (var userRole in userRoles)
        //{
        //    claims.Add(new Claim("role", userRole));
        //}

        //var identityClaims = new ClaimsIdentity();
        //identityClaims.AddClaims(claims);

        //return identityClaims;

        throw new NotImplementedException();
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

    private UsuarioRespostaLogin ObterRepostaToken(string encodedToken, IdentityUser user, ICollection<Claim> claims)
    {
        var response = new UsuarioRespostaLogin
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = user.Id,
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