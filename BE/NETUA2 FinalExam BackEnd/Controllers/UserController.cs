﻿using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs;
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

        public UserController(ILogger<UserController> logger, IUserService userService, IJwtService jwtService, IUserDBRepository userDBRepository)
        {
            _logger = logger;
            _userService = userService;
            _jwtService = jwtService;
            _userDBRepository = userDBRepository;
        }

        /// <summary>
        ///  user login
        /// </summary>
        /// <response code="400">Model validation error</response>
        /// <response code="500">System error</response>
        [HttpPost("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(UserGetDTO request)
        {
            _logger.LogInformation($"Login attempt for {request.Username}");
            var response = _userService.Login(request.Username, request.Password, out string role);

            if (!response.IsSuccess)
            {
                _logger.LogWarning($"User {request.Username} not found");
                return BadRequest(response);
            }

            _logger.LogInformation($"User {request.Username} successfully logged in");

            var jwtToken = _jwtService.GetJwtToken(request.Username, role);
            return Ok(jwtToken);
        }

        /// <summary>
        ///  create a user account
        /// </summary>
        /// <param name="request"></param>
        /// <response code="500">System error</response>
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
        /// remove a user account. for admin only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
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
            return NoContent();
        }

    }
}