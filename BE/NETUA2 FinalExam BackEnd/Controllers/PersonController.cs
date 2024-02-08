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
        /// <summary>
        /// Gets a person from current user.
        /// </summary>
        /// <param></param>
        /// <returns></returns>

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
        /// <param name="personId"></param>
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

        // POST create new person



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
        public IActionResult UpdatePersonName
            ([FromRoute] int personId,
            [FromForm] [Required(ErrorMessage = "Name is required.")]//required uztenka
            [RegularExpression(@"^[^\s]+$", ErrorMessage = "Name cannot be empty or whitespace.")]//gali nebuti
            string personName
            )
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
        public IActionResult UpdatePersonSurname
            ([FromRoute] int personId,
            [FromForm][Required(ErrorMessage = "Surname is required.")]
            [RegularExpression(@"^[^\s]+$", ErrorMessage = "Surname cannot be empty or whitespace.")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Surname must be between 3 and 30 characters.")]
            string surname
            )
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
        public IActionResult UpdatePersonSocSecNumber
            ([FromRoute] int personId,
            [FromForm] string socSecNumber
            )
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
        public IActionResult UpdatePersonEmail
            ([FromRoute] int personId,
            [FromForm][EmailAddress(ErrorMessage = "Invalid email address.")] string email
            )
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

        [HttpPut("{personId}/updatePersonImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]


        public IActionResult UpdatePersonImage([FromRoute] int personId, [FromForm][EmailAddress] int profilePictureId)
        {
            _logger.LogInformation($"Updating profile picture for user (id: {personId})");
            // kviesti image servise - GetImage - gaunu int
            var entity = _personRepository.GetPersonByPersonId(personId);

            if (entity == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            entity.ProfilePictureId = profilePictureId;
            _personRepository.UpdatePerson(entity);

            return NoContent();
        }
    }
}
