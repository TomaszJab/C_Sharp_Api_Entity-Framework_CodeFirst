using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.DTOs.Responses;

namespace WebApplication1.Services
{
    public interface IDbService
    {
        public Task<IEnumerable<Doctor>> GetDoctorsAsync();

        public Task PostDoctorsAsync(Doctor doctor);
        
        public Task<bool> DeleteDoctorsAsync(int idDoctor);

        public Task<bool> PutDoctorsAsync(Doctor doctor);

        public Task<GetPrescriptionsResponseDto> GetPrescriptionsAsync(int idPrescription);

        public Task<bool> CheckPrescriptionsAsync(int idPrescription);

        public Task<bool> CheckUserExistsAsync(LoginRequest loginRequest);
        public Task<AppUser> LoginUsersAsync(LoginRequest loginRequest);
        public Task<Tocken> GetTokensAsync(AppUser appUser);
        public Task<bool> AddUsersAsync(LoginRequest loginRequest);

        public Task<AppUser> GetAppUserAsync(RefreshTokenRequest refreshToken);

        public Task<Tocken> GetNewTockenAsync(AppUser appUser);
    }
}
