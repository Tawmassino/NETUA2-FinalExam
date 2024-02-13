using FE_BE._BUSINESS.BL_Services.BL_Interfaces;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;
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
        private readonly IPersonMapper _personMapper;
        private readonly IImageFileService _imageFileService;

        public PersonController(
            ILogger<PersonController> logger, IHttpContextAccessor httpContextAccessor,
            IUserService userService, IPersonRepository personRepository, IPersonMapper personMapper, IImageFileService imageFileService
            )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _personRepository = personRepository;
            _personMapper = personMapper;
            _imageFileService = imageFileService;
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
            if (person == null)
            {
                return NotFound();

            }
            var imageToGet = _imageFileService.GetImage(person.ProfilePictureId);

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

        // POST CreateOrUpdate new person??

        /// <summary>
        /// creates a section for personal information (person class)
        /// </summary>
        /// <param name="request"></param>
        /// <response code="500">System error</response>
        [HttpPost("CreateNewPerson")]
        public IActionResult CreateNewPerson([FromBody] PersonCreateDTO request)
        {
            //mapper dto -> person
            var newPerson = _personMapper.Map(request, request.UserId);

            //FOR LATER --- IMPLEMENT UPLOAD PIC
            //var personImageId = _imageFileService.AddImage(newPerson.ProfilePicture);// Uploading picture, getting the picture ID
            //newPerson.ProfilePictureId = personImageId;//setting the picture id to Person Foreign Key of picture id

            //newPerson.UserId = request.UserId;// already in the mapper

            //create person
            _userService.CreateNewPerson(newPerson);//send to service and then to DataBase

            return Ok();
        }



        // ===================================== UPDATING PERSON =====================================

        [HttpPut("{personId}/updateName")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]        
        ////[Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        ////[Consumes(MediaTypeNames.Application.Json)] - error for some reason. Changed to plain text. Remeber - pass plain string, not JSON anymore        
        public IActionResult UpdatePersonName
            ([FromRoute] int personId,
            [FromBody] [Required(ErrorMessage = "Name is required.")]//required uztenka
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]

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
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]
        ////[Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        ////[Consumes(MediaTypeNames.Application.Json)] - error for some reason. Changed to plain text. Remeber - pass plain string, not JSON anymore 
        public IActionResult UpdatePersonSurname
            ([FromRoute] int personId,
            [FromBody][Required(ErrorMessage = "Surname is required.")]
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
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]
        ////[Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        ////[Consumes(MediaTypeNames.Application.Json)] - error for some reason. Changed to plain text. Remeber - pass plain string, not JSON anymore 
        public IActionResult UpdatePersonSocSecNumber
            ([FromRoute] int personId,
            [FromBody]
            [RegularExpression(@"^\d{11}$", ErrorMessage = "SocialSecurityNumber must be exactly 11 numeric characters.")]
            [StringLength(11, ErrorMessage = "SocialSecurityNumber must be exactly 11 characters.")]
        string socSecNumber
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
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]
        ////[Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        ////[Consumes(MediaTypeNames.Application.Json)] - error for some reason. Changed to plain text. Remeber - pass plain string, not JSON anymore 
        public IActionResult UpdatePersonPhoneNumber(
            [FromRoute] int personId,
            [FromBody]
            [RegularExpression(@"^\d{11}$", ErrorMessage = "Phonenumber must be exactly 12 numeric characters.")]
            [StringLength(12, ErrorMessage = "Phonenumber must be exactly 12 characters.")]
        string phoneNumber
            )
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
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Text.Plain)]
        ////[Consumes(MediaTypeNames.Multipart.FormData)]//data from form
        ////[Consumes(MediaTypeNames.Application.Json)] - error for some reason. Changed to plain text. Remeber - pass plain string, not JSON anymore 
        public IActionResult UpdatePersonEmail
            ([FromRoute] int personId,
            [FromBody] // - siusti ne json o string
            //[FromForm] - error
            [EmailAddress(ErrorMessage = "Invalid email address.")] string email

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


        //this is bad, it should work with picture, not with id
        [HttpPut("{personId}/updatePersonImage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(MediaTypeNames.Application.Json)]
        //[Consumes(MediaTypeNames.Application.Json)]        
        public IActionResult UpdatePersonImage([FromRoute] int personId, [FromForm] int profilePictureId)
        {
            _logger.LogInformation($"Updating profile picture for user (id: {personId})");
            // kviesti image servise - GetImage - gaunu int
            var personWhosePicIdToChange = _personRepository.GetPersonByPersonId(personId);
            var imageToChange = _imageFileService.GetImage(profilePictureId);

            if (personWhosePicIdToChange == null)
            {
                _logger.LogInformation($"Person with id {personId} was not found");
                return NotFound();
            }

            personWhosePicIdToChange.ProfilePictureId = profilePictureId;
            _personRepository.UpdatePerson(personWhosePicIdToChange);

            return NoContent();
        }
    }
}
