using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/Accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IDbService _dbService;
        public AccountsController(IDbService dbService)
        {
            this._dbService = dbService;
        }
      
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
           if(! await _dbService.CheckUserExistsAsync(loginRequest))
            {
                return BadRequest("The user with the given login does not exist");
            }
            AppUser appUser = await _dbService.LoginUsersAsync(loginRequest);
            if (appUser == null)
            {
                return Unauthorized("Provide wrond login");
            }

            return Ok(await _dbService.GetTokensAsync(appUser));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Loggin(LoginRequest loginRequest)
        {
           if(await _dbService.AddUsersAsync(loginRequest))
            {
                return Ok("User has been registered");
            }
            else
            {

                return BadRequest("Give another login");
            }
           
            
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromHeader(Name = "Authorization")] string token, RefreshTokenRequest refreshToken)
        {

            AppUser user = await _dbService.GetAppUserAsync(refreshToken);
            if (user == null)
            {

                throw new SecurityTokenException("Invalid refresh token");
            }

            if (user.RefreshTokenExp < DateTime.Now)
            {

                throw new SecurityTokenException("Refresh token expired");
            }


            return Ok(await _dbService.GetNewTockenAsync(user));
        }



    }
}
