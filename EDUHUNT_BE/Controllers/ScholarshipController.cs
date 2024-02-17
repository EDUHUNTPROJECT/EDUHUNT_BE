using EDUHUNT_BE.Data;
using EDUHUNT_BE.Model;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;

namespace EDUHUNT_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScholarshipController : ControllerBase

    {
        private readonly AppDbContext _context;
        private readonly IScholarship _scholarshipRepository;

        public ScholarshipController(AppDbContext context, IScholarship scholarshipRepository)
        {
            _context = context;
            _scholarshipRepository = scholarshipRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetScholarships()
        {
            try
            {
                var scholarships = await _scholarshipRepository.GetScholarships();

                foreach (ScholarshipDTO scholarshipDTO in scholarships)
                {
                    var newScholarshipInfo = new ScholarshipInfo
                    {
                        // Assuming Id in DTO is not needed as it's a new Guid generated in ScholarshipInfo
                        Title = scholarshipDTO.Title,
                        Location = scholarshipDTO.Location,
                        SchoolName = scholarshipDTO.School_name,
                        Url = scholarshipDTO.Url,
                        // Convert Budget from string to decimal. Assuming the string is always a valid decimal format.
                        Budget = scholarshipDTO.Budget,
                        // Default values for fields not present in DTO
                        CategoryId = 0, // Assuming a default value, adjust as necessary
                        AuthorId = 0, // Assuming a default value, adjust as necessary
                        IsInSite = false, // Assuming a default value, adjust as necessary
                                          // CreatedAt is set by default in the model
                    };
                    _context.ScholarshipInfos.Add(newScholarshipInfo);
                }

                await _context.SaveChangesAsync();

                return Ok(scholarships);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddScholarship([FromBody] ScholarshipDTO scholarshipDTO)
        {
            try
            {


                // Add scholarship to the database
                await _scholarshipRepository.AddScholarship(scholarshipDTO);

                // You can add additional logic if needed, such as returning the added scholarship
                // var addedScholarship = await _scholarshipRepository.GetScholarshipById(scholarshipDTO.Id);

                return Ok("Scholarship added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
