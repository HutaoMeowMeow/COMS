using COMS.DTOs;
using COMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementsController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var announcements = await _announcementService.GetAllAsync();
        return Ok(announcements);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var announcement = await _announcementService.GetByIdAsync(id);
        if (announcement == null)
            return NotFound();
        return Ok(announcement);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        
        if (userId == null || userRole == null)
            return Unauthorized();

        var announcement = await _announcementService.CreateAsync(dto, Guid.Parse(userId), userRole);
        return CreatedAtAction(nameof(GetById), new { id = announcement.Id }, announcement);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAnnouncementDto dto)
    {
        var announcement = await _announcementService.UpdateAsync(id, dto);
        if (announcement == null)
            return NotFound();
        return Ok(announcement);
    }

    [Authorize(Roles = "Admin,LGU,Barangay,Maintenance")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _announcementService.DeleteAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
