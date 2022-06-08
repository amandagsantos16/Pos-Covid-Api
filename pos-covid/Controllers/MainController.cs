using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace pos_covid_api.Controllers;

[ApiController]
public class MainController : Controller
{
    protected ICollection<string> Erros = new List<string>();
    protected bool IsValid => Erros.Any() == false;

    protected ActionResult CustomResponse(object result = null)
    {
        if (IsValid) return Ok(result);

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            {"Mensagens", Erros.ToArray()}
        }));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        var erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (var erro in erros)
        {
            AdicionarErroProcessamento(erro.ErrorMessage);
        }

        return CustomResponse();
    }

    protected bool OperacaoValida()
    {
        return Erros.Any() == false;
    }

    protected void AdicionarErroProcessamento(string erro)
    {
        Erros.Add(erro);
    }

    protected void LimparErrosProcessamento()
    {
        Erros.Clear();
    }
}