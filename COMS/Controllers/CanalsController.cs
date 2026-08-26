using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
public class CanalsController : ControllerBase
{
    private readonly ICanalService _canalService;

    public CanalsController(ICanalService canalService)
    {
        _canalService = canalService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var canals = await _canalService.GetAllAsync();
        return Ok(canals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var canal = await _canalService.GetByIdAsync(id);
        if (canal == null)
            return NotFound();
        return Ok(canal);
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStatus()
    {
        var statuses = await _canalService.GetStatusAsync();
        return Ok(statuses);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCanalDto dto)
    {
        var canal = await _canalService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = canal.Id }, canal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCanalDto dto)
    {
        var canal = await _canalService.UpdateAsync(id, dto);
        if (canal == null)
            return NotFound();
        return Ok(canal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _canalService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
