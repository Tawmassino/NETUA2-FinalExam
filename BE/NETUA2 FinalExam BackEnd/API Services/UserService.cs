﻿using Azure.Core;
using FE_BE._DATA.DB_Interfaces;
using FE_BE._DATA.DB_Repositories;
using FE_BE._DATA.DB_Repositories.DB_Interfaces;
using FE_BE._DATA.Entities;
using Microsoft.EntityFrameworkCore;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs.UserDTOs;
using System.Security.Claims;
using System.Security.Cryptography;

namespace NETUA2_FinalExam_BackEnd.API_Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserDBRepository _userDBRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IPersonRepository _personRepository;//galima
        //location repo
        private readonly ILivingLocationRepository _locationRepository;


        public UserService(
            IHttpContextAccessor httpContextAccessor,
            IUserDBRepository userDBRepository,
            ILogger<UserService> logger,
            IJwtService jwtService,
            IPersonRepository personRepository,
            ILivingLocationRepository locationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDBRepository = userDBRepository;
            _logger = logger;
            _jwtService = jwtService;
            _personRepository = personRepository;
            _locationRepository = locationRepository;
        }

        // ============================================================ methods ============================================================

        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //add dependency in program cs: builder.Services.AddHttpContextAccessor();
            return int.Parse(userId);
        }

        public (bool, User) Login(string username, string password, out string role)//UserResponse graziname tik controller, cia to nereikia
        {
            //tuple - su bool ir useriu          
            var user = _userDBRepository.GetUserByUsername(username);
            var userId = user.Id;
            role = user?.Role;

            if (userId == null)
            {
                return (false, null);
            }

            if (user == null)
            {
                _logger.LogWarning($"Username or password does not match");
                return (false, null);
            };

            if (!VerifyPasswordHash(password, user.Password, user.PasswordSalt))
            {
                _logger.LogWarning($"Incorrect password for user: {username}");
                return (false, null);
            }
            _logger.LogInformation($"User logged in: {username}");
            //var jwt = _jwtService.GetJwtToken(user.Username, user.Role, user.Id);            
            return (true, user); ;
        }

        public UserResponse Register(string username, string password, string email)
        {
            var user = _userDBRepository.GetUserByUsername(username);
            if (user is not null)
            {
                return new UserResponse(false, "User already exists");
            }

            user = CreateUser(username, password, email);
            int userId = _userDBRepository.AddNewUser(user);
            string message = $"User {username} has been created successfully with ID: {userId}";
            _logger.LogInformation($"User {username} has been created successfully with ID: {userId}");
            return new UserResponse(true, message);
        }

        internal User CreateUser(string username, string password, string email)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Username = username,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Email = email,
                Role = "User",
            };

            return user;
        }
        // -------------------------------------------------------- person methods --------------------------------------------------------

        //create person
        public Person CreateNewPerson(Person newPersonData)
        {
            _logger.LogInformation($"Creating a person with ID: {newPersonData.Id} and userID: {newPersonData.UserId}");
            //newPersonData.UserId = userId;


            _logger.LogInformation($"Person with ID: {newPersonData.Id} and userID: {newPersonData.UserId} has been successfully created.");

            _personRepository.AddNewPerson(newPersonData);
            _logger.LogInformation($"Person with ID: {newPersonData.Id} and userID: {newPersonData.UserId} sent to DataBase.");

            return newPersonData;
        }


        // -------------------------------------------------------- location methods --------------------------------------------------------


        //create location
        public LivingLocation CreateNewLocation(LivingLocation location)
        {
            _locationRepository.CreateNewLivingLocation(location);
            _logger.LogInformation($"Location with ID: {location.Id} has been successfully created for person with id {location.PersonId}.");

            return location;
        }

        public LivingLocation GetCurrentLivingLocation()
        {
            var userId = GetCurrentUserId();

            Person? person = _personRepository.GetPersonByUserId(userId);
            if (person == null)
            {
                return null;
            }

            LivingLocation? location = _locationRepository.GetLocationByPersonId(person.Id);
            if (location == null)
            {
                return null;//galima padaryt new Car car = new Car;
            }

            return location;
        }

        // ---------------------------- user methods ----------------------------
        public void DeleteUser(int userId)
        {
            _userDBRepository.DeleteUser(userId);
            _personRepository.DeletePersonById(userId);
            //delete location
            //delete image
        }

        // ======================================== password hash verification ========================================
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
