// IScholarship interface
using SharedClassLibrary.DTOs;
using static SharedClassLibrary.DTOs.ServiceResponses;

namespace SharedClassLibrary.Contracts
{
    public interface IScholarship
    {
        Task<List<ScholarshipDTO>> GetScholarships();
        Task AddScholarship(ScholarshipDTO scholarship);
    }
}
