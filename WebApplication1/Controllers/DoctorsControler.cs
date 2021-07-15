using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsControler : ControllerBase
    {
        private readonly IDbService _dbService;
        public DoctorsControler(IDbService dbService)
        {
            this._dbService = dbService;
        }
       // [AllowAnonymous] wyłączanie z autoryzacji
        [HttpGet]
        public async Task<IActionResult> GetDoctor()//ok
        {
            List<Doctor> listDoctor = (List<Doctor>)await _dbService.GetDoctorsAsync();
            if (listDoctor.Count() == 0)
            {
                return NotFound("There are no doctors at the base");
            }
            return Ok(await _dbService.GetDoctorsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostDoctor([FromBody] Doctor doctor)
        {
            await _dbService.PostDoctorsAsync(doctor);
            return Ok("The doctor has been added"); 
        }

        [HttpDelete("{idDoctor}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute]int idDoctor)//ok
        {
            if (await _dbService.DeleteDoctorsAsync(idDoctor))
            {
                return Ok("The doctor has been deleted");
            }
            else
            {
                return BadRequest("The doctor is not on the database");
            }
        }

        [HttpPut("{idDoctor}")]
        public async Task<IActionResult> PutDoctor([FromRoute] int idDoctor, [FromBody]Doctor doctor)//ok
        {
            if (idDoctor != doctor.IdDoctor)
            {
                return BadRequest("idDoctor from route is not the same as idDoctor from body");
            }

            if (await _dbService.PutDoctorsAsync(doctor))
            {
                return Ok("The doctor has been puted");
            }
            else
            {
                return BadRequest("The doctor is not on the database");
            }
        }

    }
}
