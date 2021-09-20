using AutoMapper;
using ContactManager.Common;
using ContactManager.Model;
using ContactManager.Model.DTO;
using ContactManager.Repository;
using ContactManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactManager.API.Controllers
{
    [Route("[Controller]/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly ICloudinaryServices _cloudinaryServices;

        public UserController(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            ILogger<UserController> logger,
            IMapper mapper,
            ICloudinaryServices cloudinaryServices 
            )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _cloudinaryServices = cloudinaryServices;
        }
        /// <summary>
        /// Get All Users Paginated
        /// </summary>
        /// <param name="paginationParams"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("all-users", Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParams paginationParams)
        {
            
     
                var users = await _unitOfWork.Users.GetPagedList(paginationParams);
                var results = _mapper.Map<IList<ResponseDTO>>(users);
                return Ok(results);
            
            
           
        }
        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var results = _mapper.Map<ResponseDTO>(user);
                return Ok(results);
            }

            return BadRequest("No record found");
        }

        /// <summary>
        /// Get One User using Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("email", Name = "GetUserByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var users = await _unitOfWork.Users.Get(o => o.Email == email);
            var results = _mapper.Map<ResponseDTO>(users);
            return Ok(results);
        }
       
        
        
        /// <summary>
        /// Add New User
        /// </summary>
        /// <param name="ImageFile"></param>
        /// <param name="registerRequestDTO"></param>
        /// <returns></returns>
       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("add-new", Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterRequestDTO registerRequestDTO)
        {
           
            User user = _mapper.Map<User>(registerRequestDTO);
            user.UserName = registerRequestDTO.Email;
            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(CreatedAtRoute("CreateUser", new { id = user.Id }, registerRequestDTO));
                
            }
            else
            {
                return StatusCode(500);
            }
           


        }
        /// <summary>
        /// Updates User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="responseDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] ResponseDTO responseDTO)
        {
            if (!ModelState.IsValid || id == null)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateUser)}");
                return BadRequest(ModelState);
            }

            var user = await _unitOfWork.Users.Get(o => o.Id == id);
            if (user == null)
            {
                _logger.LogError($"Something Went Wrong {nameof(UpdateUser)}");
                return StatusCode(500, "Something Went Wrong");
            }
            _mapper.Map(responseDTO, user);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.Save();
            return NoContent();
        }
        /// <summary>
        /// Search Table using FirstName, LastName and Email
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("search", Name = "Search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var users = await _unitOfWork.Users.GetAll(
                o => o.Email.Contains(term)
                || o.FirstName.Contains(term)
                || o.LastName.Contains(term)
                || o.Email.Contains(term)
                );
            var results = _mapper.Map<IList<ResponseDTO>>(users);
            return Ok(results);
        }
        /// <summary>
        /// Deletes User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles ="Admin")]
        [Route("delete/{id}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteUser)}");
                return BadRequest();
            }

            await _unitOfWork.Users.Delete(id);
            await _unitOfWork.Save();
            return NoContent();
        }
        /// <summary>
        /// Updates User Record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Image"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("photo", Name = "PatchPhoto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchPhoto(string id, [FromForm] AddImageDTO imageDTO)
        {
            var image = imageDTO.Image;
            var user = await _userManager.FindByIdAsync(id);
            var uploadUrl = await _cloudinaryServices.ImageUploadAsync(image);
            var imageProperty = new ImageAddedDTO()
            {
                PublicId = uploadUrl.PublicId,
                URL = uploadUrl.Url.ToString()
            };

            user.ImageUrl = string.IsNullOrWhiteSpace(imageProperty.URL) ? "Default.jpg" : imageProperty.URL;

            var response = await _userManager.UpdateAsync(user);
            if (response.Succeeded)
                return NoContent();

            return BadRequest(response);
        }

    }
}

