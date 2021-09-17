using AutoMapper;
using ContactManager.Model;
using ContactManager.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.API.Controllers
{
    [Route("User/")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        public AccountController(UserManager<User> userManager, 
           // SignInManager<User> signInManager,
            ILogger<AccountController> logger,
            IMapper mapper)
        {
           // _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
           
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("add-new")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<User>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user);
                return Accepted();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something Went Wrong");
                return Problem("Something Went Wrong", statusCode: 500);
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }
                return Accepted();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Something Went Wrong");
                return Problem("Something Went Wrong", statusCode: 500);
            }
        }
    }
}


/*
 GET: http:localhost:[port]/User/all-users?page=[current number]

· GET: http:localhost:[port]/User/[id]

· GET: http:localhost:[port]/User/[email]

· GET: http:localhost:[port]/User/search?term=[search-term]

· POST: http:localhost:[port]/User/add-new

· PUT: http:localhost:[port]/User/update/[id]

· DELETE: http:localhost:[port]/User/delete/[id]

· PATCH: http:localhost:[port]/User/photo/[id
*/