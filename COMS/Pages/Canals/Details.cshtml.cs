using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace COMS.Pages.Canals;

public class DetailsModel : PageModel
{
    private readonly ICanalService _canalService;
    private readonly ISensorService _sensorService;

    public DetailsModel(ICanalService canalService, ISensorService sensorService)
    {
        _canalService = canalService;
        _sensorService = sensorService;
    }

    public CanalResponseDto? Canal { get; set; }
    public IEnumerable<SensorResponseDto> Sensors { get; set; } = new List<SensorResponseDto>();

    public async Task<IActionResult> OnGet(Guid id)
    {
        Canal = await _canalService.GetByIdAsync(id);
        if (Canal == null) return NotFound();
        Sensors = await _sensorService.GetByCanalAsync(id);
        return Page();
    }
}
