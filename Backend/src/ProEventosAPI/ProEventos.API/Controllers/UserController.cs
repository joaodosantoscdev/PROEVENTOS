using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
using ProEventos.Application.DTO;
using ProEventos.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // Constructor &  Dependencies
        #region DI Injected
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUtility _utility;
        private readonly string _destiny = "Profile";

        public UserController(IUserService userService,
                              ITokenService tokenService,
                              IUtility utility)
        {
            _userService = userService;
            _tokenService = tokenService;
            _utility = utility;
        }
        #endregion

        // GET - User
        #region GET Methods - Controller
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _userService.GetUserByUserNameAsync(userName);

                return Ok(user);
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar encontrar Usuário. Erro {e.Message}"); ;
            }
        }
        #endregion

        // ADD - User
        #region POST Methods - Controller
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {
                if (await _userService.UserExists(userDTO.UserName)) 
                    return BadRequest("Usuário já existe");

                var user = await _userService.CreateAccountAsync(userDTO);

                if ( user != null) return Ok(new {
                    userName = user.UserName,
                    FirstName = user.FirstName,
                    token = _tokenService.CreateToken(user).Result
                });

                return BadRequest("Não foi possivel cadastrar o usuário, tente mais tarde!");
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar registrar Usuário. Erro {e.Message}"); ;
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(userLoginDTO.Username);
                if (user == null) 
                    return Unauthorized("Usuário ou senha inválido(s)");

                var result = await _userService.CheckUserPasswordAsync(user, userLoginDTO.Password);
                if (!result.Succeeded) return Unauthorized("Acesso negado. Verifique se o usuário ou/e a senha estão corretas");

                return Ok(new 
                {
                    userName = user.UserName,
                    FirstName = user.FirstName,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro no servidor, estamos cuidando disso. Tente novamente mais tarde. Erro {e.Message}"); ;
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImg()
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _utility.DeleteImg(user.ImageURL, _destiny);
                    user.ImageURL = await _utility.SaveImg(file, _destiny);
                }
                var userReturn = await _userService.UpdateAccount(user);

                return Ok(userReturn);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar realizar upload de foto do usuário. Erro {e.Message}");
            }
        }
        #endregion

        // UPDATE - User
        #region PUT Methods - Controller
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                if (userUpdateDTO.UserName != User.GetUserName())
                    return Unauthorized("Usuário inválido");

                var user = await _userService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null)
                    return Unauthorized("Usuário inválido");

                var userReturn = await _userService.UpdateAccount(userUpdateDTO);
                if (userReturn == null) return NoContent();

                return Ok(new {
                    userName = userReturn.UserName,
                    FirstName = userReturn.FirstName,
                    token = _tokenService.CreateToken(userReturn).Result
                });
            }
            catch (Exception e)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar o Usuário. Erro {e.Message}"); ;
            }
        }
        #endregion

    }
}
