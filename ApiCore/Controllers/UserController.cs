using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Request;

namespace ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
       [Authorize]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(UserRequest user)
        {
            try
            {
                bool response = await userService.CreateUser(user);

                if (response) return Ok(new { status = true, message = "Usuario creado con éxito" });

                return StatusCode(StatusCodes.Status200OK, new { status = false, message = "El email ya se encuentra registrado" });
            }
            catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrio un error", data = ex }); }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest user)
        {
            try
            {
                string response = await userService.Login(user);

                if (string.IsNullOrEmpty(response)) return StatusCode(StatusCodes.Status200OK, new { status = false, message = "Credenciales incorrectas" });

                return Ok(new { status = true, data = response });
            }
            catch (Exception ex) { return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Ocurrio un error", data = ex }); }


        }
    }
}
