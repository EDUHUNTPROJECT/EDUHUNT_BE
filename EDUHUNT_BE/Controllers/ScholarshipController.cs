using EDUHUNT_BE.Data;
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

        public ScholarshipController(IScholarship scholarshipRepository)
        {
            _scholarshipRepository = scholarshipRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetScholarships()
        {
            try
            {
                var scholarships = await _scholarshipRepository.GetScholarships();




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
