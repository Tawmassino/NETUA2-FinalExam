using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace NETUA2_FinalExam_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;//ar sita tik user controller?
        private readonly IUserService _userService;
        private readonly IPersonRepository _personRepository;

        public PersonController(
            ILogger<PersonController> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService, IPersonRepository personRepository
            )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _personRepository = personRepository;
        }


        // ======================= METHODS =======================

        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [HttpGet("GetPersonInfoFromUser")]
        public IActionResult GetPersonInfoFromUser()
        {
            var userId = _userService.GetCurrentUserId();
            Person person = _personRepository.GetPersonByUserId(userId);

            return Ok(person);
        }

        /// <summary>
        /// remove a person account.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePerson(int personId)
        {//kontroleris ksirsto darbus, todel gali but serviso mixas
            int personToDeleteId = _userService.GetCurrentUserId();// USER SERVICE in PERSON CONTROLLER??? 
            if (personToDeleteId == null)
            {
                _logger.LogInformation($"Account ID {personToDeleteId} not found");
                return NotFound();
            }
            _personRepository.DeletePersonById(personToDeleteId);// PERSON SERVICE
            return NoContent();

        }

        // ===================================== UPDATING USER =====================================
        //CANNOT UPDATE TO EMPTY OR WHITEPSACE     
        //implement validation logic!! 

        [HttpPut("{personId}/updateName")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Multipart.FormData)]//duomenys is formos
        public IActionResult UpdatePersonSurname([FromRoute] int personId, [FromForm] string personName)
        {
            _logger.LogInformation($"Updating {nameof(personName)} details to {personName} for user (id: {personId})");
            var entity = _personRepository.GetPersonByPersonId(personId);
            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.Name = personName;
            _personRepository.UpdatePerson(entity);
            return NoContent();
        }


        [HttpPut("{personId}/updateSurname")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UpdatePersonSurname2([FromRoute] int personId, [FromForm][StringLength(30, MinimumLength = 3)] string surname)
        {
            _logger.LogInformation($"Updating {nameof(surname)} details to {surname} for user (id: {personId})");

            var entity = _personRepository.GetPersonByPersonId(personId);

            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.Surname = surname;
            _personRepository.UpdatePerson(entity);

            return NoContent();
        }

        [HttpPut("{personId}/updateSocSecNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UpdatePersonSocSecNumber([FromRoute] int personId, [FromForm] string socSecNumber)
        {
            _logger.LogInformation($"Updating {nameof(socSecNumber)} details to {socSecNumber} for user (id: {personId})");

            var entity = _personRepository.GetPersonByPersonId(personId);

            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.SocialSecurityNumber = socSecNumber;
            _personRepository.UpdatePerson(entity);

            return NoContent();
        }

        [HttpPut("{personId}/updatePhoneNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UpdatePersonPhoneNumber([FromRoute] int personId, [FromForm] string phoneNumber)
        {
            _logger.LogInformation($"Updating {nameof(phoneNumber)} details to {phoneNumber} for user (id: {personId})");

            var entity = _personRepository.GetPersonByPersonId(personId);

            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.PhoneNumber = phoneNumber;
            _personRepository.UpdatePerson(entity);

            return NoContent();
        }

        [HttpPut("{personId}/updateEmail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UpdatePersonEmail([FromRoute] int personId, [FromForm][EmailAddress] string email)
        {
            _logger.LogInformation($"Updating {nameof(email)} details to {email} for user (id: {personId})");

            var entity = _personRepository.GetPersonByPersonId(personId);

            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.Email = email;
            _personRepository.UpdatePerson(entity);

            return NoContent();
        }

        [HttpPut("{personId}/updateEmail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult UpdatePersonImage([FromRoute] int personId, [FromForm][EmailAddress] string email)
        {
            _logger.LogInformation($"Updating {nameof(email)} details to {email} for user (id: {personId})");

            var entity = _personRepository.GetPersonByPersonId(personId);

            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.Email = email;
            _personRepository.UpdatePerson(entity);

            return NoContent();
        }
    }
}
