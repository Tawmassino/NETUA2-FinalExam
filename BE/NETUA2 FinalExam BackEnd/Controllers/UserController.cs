using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;
using System.Net.Mime;



namespace NETUA2_FinalExam_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        //private readonly IUserMapper _userMapper;
        private readonly IUserDBRepository _userDBRepository;
        private readonly IPersonRepository _personRepository;

        public UserController(
            ILogger<UserController> logger,
            IUserService userService,
            IJwtService jwtService,
            IUserDBRepository userDBRepository,
            IPersonRepository personRepository)
        {
            _logger = logger;
            _userService = userService;
            _jwtService = jwtService;
            _userDBRepository = userDBRepository;
            _personRepository = personRepository;
        }

        /// <summary>
        ///  Authenticate user and generate JWT token for login.
        /// </summary>
        /// <response code="400">Model validation error</response>
        /// <response code="500">System error</response>
        [HttpPost("Login")]
        //[Route("/prisijungimas")]//cia keicia linka, paziuret spargalkes imagecontroller
        [Consumes(MediaTypeNames.Application.Json)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(UserGetDTO request)
        {// Only username and password are needed

            _logger.LogInformation($"Login attempt for {request.Username}");

            var (loginResult, User) = _userService.Login(request.Username, request.Password, out string role);//trint role?

            if (loginResult == false)
            {
                _logger.LogWarning($"User {request.Username} not found");
                return BadRequest("Invalid username or password");
            }

            //Generating JWT in the controller
            var jwtToken = _jwtService.GetJwtToken(request.Username, role, User.Id);

            _logger.LogInformation($"User {request.Username} successfully logged in");

            return Ok(new UserResponse(true, "User logged in", jwtToken, User.Id));
        }


        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="request"></param>        
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserCreateDTO request)
        {
            _logger.LogInformation($"Creating account for {request.Username}");

            var response = _userService.Register(request.Username, request.Password, request.Email);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }

            return Ok();
        }


        /// <summary>
        /// Remove a user account. For admin only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting account {id}");
            var userToDelete = _userDBRepository.GetUserById(id);
            if (userToDelete is null)
            {
                _logger.LogInformation($"Account {id} not found");
                return NotFound();
            }
            _userDBRepository.DeleteUser(id);

            //sql cascade delete

            return NoContent();
        }

    }
}
