using FE_BE._DATA.DB_Repositories;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using System.Net.Mime;

namespace NETUA2_FinalExam_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class LivingLocationController : ControllerBase
    {
        private readonly ILogger<LivingLocationController> _logger;
        private readonly ILivingLocationRepository _livingLocationRepository;
        private readonly IUserService _userService;
        private readonly IPersonRepository _personRepository;

        public LivingLocationController(
            ILogger<LivingLocationController> logger,
            ILivingLocationRepository livingLocationRepository,
            IUserService userService
            )
        {
            _logger = logger;
            _livingLocationRepository = livingLocationRepository;
            _userService = userService;
        }

        // ======================= METHODS =======================


        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [HttpGet("GetLocationInfoFromPerson")]
        public IActionResult GetLocationInfoFromPerson()
        {
            var userId = _userService.GetCurrentUserId();
            Person person = _personRepository.GetPersonByUserId(userId);
            LivingLocation location = _livingLocationRepository.GetLivingLocationByPersonId(person.Id);

            return Ok(location);
        }


        /// <summary>
        /// removes a location.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpDelete("{locationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteLocation(int locationId)
        {
            int personToDeleteId = _userService.GetCurrentUserId();// USER SERVICE in PERSON CONTROLLER??? 
            if (personToDeleteId == null)
            {
                _logger.LogInformation($"Account ID {personToDeleteId} not found");
                return NotFound();
            }
            _personRepository.DeletePersonById(personToDeleteId);// PERSON SERVICE
            return NoContent();

        }
    }
}
