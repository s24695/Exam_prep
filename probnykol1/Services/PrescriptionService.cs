using probnykol1.DTOs;
using probnykol1.Models;
using probnykol1.Repositories;

namespace probnykol1.Services;

public class PrescriptionService : IPrescriptionService
{
    private IPrescriptionsRepository _prescriptionsRepository;

    public PrescriptionService(IPrescriptionsRepository prescriptionsRepository)
    {
        _prescriptionsRepository = prescriptionsRepository;
    }

    public Task<IEnumerable<PrescriptionDTO>> GetPrescriptions(string lastName)
    {
        return _prescriptionsRepository.GetPrescriptions(lastName);
    }

    public Task<PrescriptionDTO> CreatePrescription(PrescriptionDTO prescriptionDto)
    {
        return _prescriptionsRepository.CreatePrescription(prescriptionDto);
    }
}