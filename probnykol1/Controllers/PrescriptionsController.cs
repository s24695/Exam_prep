using Microsoft.AspNetCore.Mvc;
using probnykol1.DTOs;
using probnykol1.Models;
using probnykol1.Services;

namespace probnykol1.Controllers;

[Route("/api/prescriptions")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrescriptionDTO>>> GetPrescriptions(string? lastName)
    {
        var prescriptions = await _prescriptionService.GetPrescriptions(lastName);
        return Ok(prescriptions);
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionDTO>> CreatePrescription(PrescriptionDTO prescriptionDto)
    {
        if (prescriptionDto.DueDate < DateTime.Now)
        {
            return BadRequest("Podano złą datę ważności recepty");
        }
        else
        {
            var result = await _prescriptionService.CreatePrescription(prescriptionDto);
            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}