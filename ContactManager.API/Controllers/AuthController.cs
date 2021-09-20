using AutoMapper;
using ContactManager.Model.DTO;
using ContactManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ContactManager.API.Controllers
{
    [Route("[Controller]/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
     
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;
        public LoginDTO LoginDTO;

        public AuthController(
            ILogger<AuthController> logger,
            IMapper mapper,
            IAuthManager  authManager
            )
        {
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("login", Name ="Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            

            if (!await _authManager.ValidateUser(loginDTO))
            {
                return Unauthorized();
            }
            return Accepted(new { Token = await _authManager.CreateToken()});  
            }
           
        }
    
}