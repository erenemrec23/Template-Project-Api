using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LocalizationController : ControllerBase
{
    private readonly ILocalizationService _localizationService;
     
    public LocalizationController(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }
    [AllowAnonymous]
    [HttpGet("{lang}")]
    public IActionResult GetTranslations(string lang)
    {
        var translations = _localizationService.GetAll(lang);
        return Ok(translations);
    }
}