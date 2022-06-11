using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace pos_covid_api.Controllers;

[Authorize]
[Route("api/pacientes")]
public class PacienteController : MainController
{
    
}