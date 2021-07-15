using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {

        private readonly IDbService _dbService;
        public PrescriptionsController(IDbService dbService)
        {
            this._dbService = dbService;
        }


        [HttpGet("{idPrescription}")]
        public async Task<IActionResult> GetPrescription([FromRoute] int idPrescription)
        {
            if (await _dbService.CheckPrescriptionsAsync(idPrescription))
            {
                return Ok(await _dbService.GetPrescriptionsAsync(idPrescription));
            }
            else
            {
                return NotFound("The prescription with the given id does not exist");
            }
    
        }
    }
}
