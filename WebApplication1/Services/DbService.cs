using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.DTOs.Responses;
using System.Web;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Services
{
    public class DbService : IDbService
    {
        private readonly MainDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public DbService(MainDbContext mainDbContext, IConfiguration configuration)
        {
            _dbContext = mainDbContext;
            _configuration = configuration;
        }

        

        public async Task<bool> DeleteDoctorsAsync(int idDoctor)//ok
        {
            if(!await _dbContext.Doctors.AnyAsync(e => e.IdDoctor == idDoctor))
            {
                return false;
            }

            _dbContext.Doctors.Remove(await _dbContext.Doctors.Where(e => e.IdDoctor == idDoctor).FirstAsync());
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
             return await _dbContext.Doctors.Select(e => e).ToListAsync();
        }

        public async Task<GetPrescriptionsResponseDto> GetPrescriptionsAsync(int idPrescription)
        {
            return await _dbContext.Prescriptions
                .Select(el => new GetPrescriptionsResponseDto
                {
                    IdPrescription = el.IdPrescription,
                    Date = el.Date,
                    DueDate = el.DueDate,
                    Patient = el.Patient,
                    Doctor = el.Doctor,
                    Medicaments = el.Prescription_Medicaments.Select(e => e).ToList()
                }).FirstAsync();
        }

        public async Task<bool> CheckPrescriptionsAsync(int idPrescription)
        {
            return await _dbContext.Prescriptions.AnyAsync(e => e.IdPrescription == idPrescription);
        }

        public async Task PostDoctorsAsync(Doctor doctor)
        {
            var doctor1 = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email
            };

            await _dbContext.Doctors.AddAsync(doctor1);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> PutDoctorsAsync(Doctor doctor)
        {
           if( await _dbContext.Doctors.AnyAsync(e => e.IdDoctor == doctor.IdDoctor))
            {
                Doctor doctor1 = await _dbContext.Doctors.FirstAsync(e => e.IdDoctor == doctor.IdDoctor);
                doctor1.FirstName = doctor.FirstName;
                doctor1.LastName = doctor.LastName;
                doctor1.Email = doctor.Email;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> AddUsersAsync(LoginRequest loginRequest)
        {
            
            if (await _dbContext.AppUsers.AnyAsync(e => e.Login == loginRequest.Login))
            {
                return false;
            }
            String pasword = loginRequest.Password;
           
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginRequest.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));


            var user = new AppUser()
            {
                Login = loginRequest.Login,
                Email = loginRequest.Login + "@gmail.com",
                Password = hashed,
                Salt = Convert.ToBase64String(salt),
                RefreshTocken = null,
                RefreshTokenExp = null
            };

           await _dbContext.AppUsers.AddAsync(user);
           await _dbContext.SaveChangesAsync();
            return true;
        }
        //check if the user exists
        public async Task<bool> CheckUserExistsAsync(LoginRequest loginRequest)
        {
            if (!(await _dbContext.AppUsers.AnyAsync(e => e.Login == loginRequest.Login)))
            {
                //nie ma uzytkownika o podanym loginie
                return false;
            }
            return true;
        }
        public async Task<AppUser> LoginUsersAsync(LoginRequest loginRequest)
        {
            AppUser appUser = await _dbContext.AppUsers.Where(u => u.Login == loginRequest.Login).FirstOrDefaultAsync();

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginRequest.Password,
                salt: Convert.FromBase64String(appUser.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (!hashed.Equals(appUser.Password))
            {
                return null;
            }
            return appUser;
        }

        public async Task<Tocken> GetTokensAsync(AppUser appUser)
        {
            Claim[] userclaim = new[] {
                    new Claim(ClaimTypes.Name, appUser.Login),
                    new Claim(ClaimTypes.Role, "admin")
                };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                appUser.RefreshTocken = Convert.ToBase64String(randomNumber);
            }

            appUser.RefreshTokenExp = DateTime.Now.AddDays(1);
            await _dbContext.SaveChangesAsync();

            var aa = new Tocken
            {
                accessToken=new JwtSecurityTokenHandler().WriteToken(jwtToken),
                refreshToken=appUser.RefreshTocken
            };
            return aa;
        }

        public async Task<AppUser> GetAppUserAsync(RefreshTokenRequest refreshToken)
        {
            return await _dbContext.AppUsers.Where(u => u.RefreshTocken == refreshToken.RefreshToken).FirstOrDefaultAsync();
        }

        public async Task<Tocken> GetNewTockenAsync(AppUser appUser)
        {
            Claim[] userclaim = new[] {
                    new Claim(ClaimTypes.Name, appUser.Login),
                    new Claim(ClaimTypes.Role, "admin")
                };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: _configuration["Issuer"],//"https://localhost:5000",
                audience: _configuration["Audience"], //"https://localhost:5000",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                appUser.RefreshTocken= Convert.ToBase64String(randomNumber);
            }
            appUser.RefreshTokenExp = DateTime.Now.AddDays(1);
            await _dbContext.SaveChangesAsync();
            return new Tocken
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                refreshToken = appUser.RefreshTocken
            };
        }
    }
}
