using ECAR.Infrastructure.Data;
using ECAR.Infrastructure.Entities;
using ECAR.Shared.DTOs;
using ECAR.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECAR.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly ECARDbContext _context;

    public RolesController(ECARDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RolDto>>>> GetRoles()
    {
        var roles = await _context.Roles
            .Select(r => new RolDto
            {
                IdRol = r.IdRol,
                Nombre = r.Nombre
            })
            .ToListAsync();

        return Ok(ApiResponse<IEnumerable<RolDto>>. SuccessResponse(roles));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RolDto>>> GetRol(long id)
    {
        var rol = await _context.Roles.FindAsync(id);

        if (rol == null)
        {
            return NotFound(ApiResponse<RolDto>.ErrorResponse("Rol no encontrado"));
        }

        var rolDto = new RolDto
        {
            IdRol = rol.IdRol,
            Nombre = rol.Nombre
        };

        return Ok(ApiResponse<RolDto>.SuccessResponse(rolDto));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<RolDto>>> CreateRol(CreateRolDto createDto)
    {
        var rol = new Rol
        {
            Nombre = createDto.Nombre
        };

        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();

        var rolDto = new RolDto
        {
            IdRol = rol.IdRol,
            Nombre = rol.Nombre
        };

        return CreatedAtAction(nameof(GetRol), new { id = rol.IdRol }, 
            ApiResponse<RolDto>.SuccessResponse(rolDto, "Rol creado exitosamente"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteRol(long id)
    {
        var rol = await _context.Roles.FindAsync(id);

        if (rol == null)
        {
            return NotFound(ApiResponse<bool>.ErrorResponse("Rol no encontrado"));
        }

        _context.Roles.Remove(rol);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Rol eliminado exitosamente"));
    }
}