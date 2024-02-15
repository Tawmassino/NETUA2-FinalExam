using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Repositories;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.API_Services;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs.LocationDTOs;
using NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs;
using System.ComponentModel.DataAnnotations;
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
        private readonly ILocationMapper _locationMapper;

        public LivingLocationController(
            ILogger<LivingLocationController> logger,
            ILivingLocationRepository livingLocationRepository,
            IUserService userService, ILocationMapper locationMapper, IPersonRepository personRepository
            )
        {
            _logger = logger;
            _livingLocationRepository = livingLocationRepository;
            _userService = userService;
            _locationMapper = locationMapper;
            _personRepository = personRepository;
        }

        // ======================= METHODS =======================



        /// <summary>
        /// creates a location.
        /// </summary>
        /// <param name="request">The LocationCreateDTO object containing location information.</param>
        /// <returns>IActionResult</returns>
        [HttpPost("CreateNewLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateNewLocation([FromBody] LocationCreateDTO request)
        {
            if (_userService == null)
            {
                _logger.LogError("UserService is null");
                return StatusCode(500, "Internal Server Error");
            }

            if (_locationMapper == null)
            {
                _logger.LogError("LocationMapper is null");
                return StatusCode(500, "Internal Server Error");
            }

            _logger.LogInformation($"Creating location information for person (person id:{request.PersonId})");

            //GetPersonInfoFromUser
            var userId = _userService.GetCurrentUserId();
            Person person = _personRepository.GetPersonByUserId(userId);

            if (person == null)
            {
                return NotFound();
            }

            if (person.Id != request.PersonId)
            {
                return Unauthorized();
            }

            //mapper dto -> location
            var newLocation = _locationMapper.Map(request, person.Id);

            //create location

            _userService.CreateNewLocation(newLocation);//send to service and then to DataBase

            return Ok();
        }

        /// <summary>
        /// Retrieves location information associated with the current user's person.
        /// </summary>
        /// <returns>IActionResult</returns>
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [HttpGet("GetLocationInfoFromPerson")]
        public IActionResult GetLocationInfoFromPerson()
        {
            //var userId = _userService.GetCurrentUserId();
            //Person person = _personRepository.GetPersonByUserId(userId);
            //LivingLocation location = _livingLocationRepository.GetLivingLocationByPersonId(person.Id);

            LivingLocation location = _userService.GetCurrentLivingLocation();

            return Ok(location);
        }


        /// <summary>
        /// removes a location.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns>IActionResult </returns>
        [HttpDelete("{locationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteLocation(int locationId)
        {
            int personToDeleteId = _userService.GetCurrentUserId();
            if (personToDeleteId == null)
            {
                _logger.LogInformation($"Account ID {personToDeleteId} not found");
                return NotFound();
            }
            _personRepository.DeletePersonById(personToDeleteId);
            return NoContent();

        }

        // ======================= UPDATE METHODS =======================

        /// <summary>
        /// Updates the city of a location.
        /// </summary>
        /// <param name="locationId">The ID of the location to be updated.</param>
        /// <param name="locationCity">The new city value for the location.</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{locationId}/updateLocationCity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        public IActionResult UpdateLocationCity
            ([FromRoute] int locationId,
            [FromForm][Required] string locationCity
            )
        {
            _logger.LogInformation($"Getting location of current user");
            var locationToUpdate = _userService.GetCurrentLivingLocation();

            _logger.LogInformation($"Updating {nameof(locationCity)} details to {locationCity})");

            locationToUpdate.City = locationCity;
            _livingLocationRepository.UpdateLivingLocation(locationToUpdate);
            return NoContent();
        }


        /// <summary>
        /// Updates the street name of a location.
        /// </summary>
        /// <param name="locationId">The ID of the location to be updated.</param>
        /// <param name="locationStreetName">The new street name value for the location.</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{locationId}/updateLocationStreetName")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        public IActionResult UpdateLocationStreetName
            ([FromRoute] int locationId,
            [FromForm][Required] string locationStreetName
            )
        {
            _logger.LogInformation($"Getting location of current user");
            var locationToUpdate = _userService.GetCurrentLivingLocation();

            _logger.LogInformation($"Updating {nameof(locationStreetName)} details to {locationStreetName})");

            locationToUpdate.StreetName = locationStreetName;
            _livingLocationRepository.UpdateLivingLocation(locationToUpdate);
            return NoContent();
        }


        /// <summary>
        /// Updates the house number of a location.
        /// </summary>
        /// <param name="locationId">The ID of the location to be updated.</param>
        /// <param name="locationHouseNumber">The new house number value for the location.</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{locationId}/updateLocationHouseNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        public IActionResult UpdateLocationHouseNumber
            ([FromRoute] int locationId,
            [FromForm][Required] int locationHouseNumber
            )
        {
            _logger.LogInformation($"Getting location of current user");
            var locationToUpdate = _userService.GetCurrentLivingLocation();

            _logger.LogInformation($"Updating {nameof(locationHouseNumber)} details to {locationHouseNumber})");

            locationToUpdate.HouseNumber = locationHouseNumber;
            _livingLocationRepository.UpdateLivingLocation(locationToUpdate);
            return NoContent();
        }


        /// <summary>
        /// Updates the country of a location.
        /// </summary>
        /// <param name="locationId">The ID of the location to be updated.</param>
        /// <param name="locationCountry">The new country value for the location.</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{locationId}/updateLocationCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        public IActionResult UpdateLocationCountry
           ([FromRoute] int locationId,
           [FromForm][Required] string locationCountry
           )
        {
            _logger.LogInformation($"Getting location of current user");
            var locationToUpdate = _userService.GetCurrentLivingLocation();

            _logger.LogInformation($"Updating {nameof(locationCountry)} details to {locationCountry})");

            locationToUpdate.Country = locationCountry;
            _livingLocationRepository.UpdateLivingLocation(locationToUpdate);
            return NoContent();
        }
    }
}
