using System.Data.SqlClient;
using probnykol1.DTOs;
using probnykol1.Models;

namespace probnykol1.Repositories;

public class PrescriptionsRepository : IPrescriptionsRepository
{
    private IConfiguration _configuration;

    public PrescriptionsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<PrescriptionDTO>> GetPrescriptions(string? lastName = null)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var com = new SqlCommand();
        com.Connection = con;

        var prescriptions = new List<PrescriptionDTO>();

        if (lastName != null)
        {
            com.CommandText = "SELECT Date, DueDate, IdPatient, IdDoctor FROM Prescription p JOIN Doctor d ON " +
                              "p.IdDoctor = d.IdDoctor " +
                              "WHERE d.LastName = @lastName " +
                              "ORDER BY p.Date DESC";
            com.Parameters.AddWithValue("@lastName", lastName);
            
            await using var reader = await com.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var prescription = new PrescriptionDTO()
                {
                    Date = DateTime.Parse(reader["Date"].ToString()),
                    DueDate = DateTime.Parse(reader["DueDate"].ToString()),
                    IdPatient = (int)reader["IdPatient"],
                    IdDoctor = (int)reader["IdDoctor"]
                };
            
                prescriptions.Add(prescription);
            }
            await reader.CloseAsync();
            com.Parameters.Clear();
            
        }
        else
        {
            com.CommandText = "SELECT Date, DueDate, IdPatient, IdDoctor FROM Prescription ORDER BY Prescription.Date DESC";
            await using var reader = await com.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var prescription = new PrescriptionDTO()
                {
                    Date = DateTime.Parse(reader["Date"].ToString()),
                    DueDate = DateTime.Parse(reader["DueDate"].ToString()),
                    IdPatient = (int)reader["IdPatient"],
                    IdDoctor = (int)reader["IdDoctor"]
                };
            
                prescriptions.Add(prescription);
            }
            await reader.CloseAsync();
        }

        await con.CloseAsync();
        
        return prescriptions;
    }

    public async Task<PrescriptionDTO> CreatePrescription(PrescriptionDTO prescriptionDto)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var com = new SqlCommand();
        com.Connection = con;

        com.CommandText = "INSERT INTO Prescription(Date, DueDate, IdPatient, IdDoctor)" +
                          "VALUES(@Date, @DueDate, @IdPatient, @IdDoctor)";
        
        com.Parameters.AddWithValue("@Date", prescriptionDto.Date);
        com.Parameters.AddWithValue("@DueDate", prescriptionDto.DueDate);
        com.Parameters.AddWithValue("@IdPatient", prescriptionDto.IdPatient);
        com.Parameters.AddWithValue("@IdDoctor", prescriptionDto.IdDoctor);
        
        com.Parameters.Clear();
        
        
        com.CommandText = "SELECT TOP 1 Date, DueDate, IdPatient, IdDoctor FROM Prescription " +
                          "ORDER BY IdPrescription DESC";
        
        await using var reader = await com.ExecuteReaderAsync();

        var newPrescription = new PrescriptionDTO();
        
        while (await reader.ReadAsync())
        {
            newPrescription = new PrescriptionDTO()
            {
                Date = DateTime.Parse(reader["Date"].ToString()),
                DueDate = DateTime.Parse(reader["DueDate"].ToString()),
                IdPatient = (int)reader["IdPatient"],
                IdDoctor = (int)reader["IdDoctor"]
            };
        }

        await reader.CloseAsync();
        com.Parameters.Clear();

        await con.CloseAsync();
        return newPrescription;
    }
}