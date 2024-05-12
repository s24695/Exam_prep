using probnykol1.DTOs;
using probnykol1.Models;

namespace probnykol1.Repositories;

public interface IPrescriptionsRepository
{
    Task<IEnumerable<PrescriptionDTO>> GetPrescriptions(string lastName);
    Task<PrescriptionDTO> CreatePrescription(PrescriptionDTO prescriptionDto);
}