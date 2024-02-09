using Azure.Core;
using FE_BE._DATA.Entities;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using NETUA2_FinalExam_BackEnd.DTOs.PersonDTOs;

namespace NETUA2_FinalExam_BackEnd.API_Services
{
    public class PersonMapper : IPersonMapper
    {
        public Person Map(PersonCreateDTO dto, int userId)
        {
            using var memoryStream = new MemoryStream();
            dto.ProfilePicture.CopyTo(memoryStream);

            var imageBytes = memoryStream.ToArray();

            var imageFile = new ImageFile
            {
                Content = imageBytes,
                ContentType = dto.ProfilePicture.ContentType,
                FileName = dto.ProfilePicture.FileName,
                Size = imageBytes.Length
            };

            return new Person
            {
                Name = dto.Name,
                Surname = dto.Surname,
                SocialSecurityNumber = dto.SocialSecurityNumber,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                UserId = userId,
                ProfilePicture = imageFile,
            };


        }

        public Person Map(PersonUpdateDTO dto)//nereikia? nes per kontrolerio metodus skirtingi endpointai yra
        {
            throw new NotImplementedException();
        }

        public PersonGetDTO Map(User user)
        {
            throw new NotImplementedException();
        }
    }
}
