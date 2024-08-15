// IScholarship interface
using SharedClassLibrary.DTOs;

namespace SharedClassLibrary.Contracts
{
    public interface IScholarship
    {
        Task<List<ScholarshipDTO>> GetScholarships();
        Task AddScholarship(ScholarshipDTO scholarship);
    }
}
