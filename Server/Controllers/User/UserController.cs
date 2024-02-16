﻿using Microsoft.AspNetCore.Mvc;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.CommandR;

namespace Server.Controllers.User
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        private readonly ICommander commander;
        public UserController(IUserService userService, ICommander commander)
        {
            this.userService = userService;
            this.commander = commander;
        }
        [HttpPost("create")]
        public async Task<ActionResult<bool>> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.Entity.PhoneNumber) || string.IsNullOrEmpty(command.Entity.Password))
            {
                var errorMessages = new List<dynamic>();

                if (string.IsNullOrEmpty(command.Entity.PhoneNumber))
                {
                    errorMessages.Add(new { message = "phone number required", param = "phone_number" });
                }

                if (string.IsNullOrEmpty(command.Entity.Password))
                {
                    errorMessages.Add(new { message = "password required", param = "password" });
                }

                return StatusCode(402, new { success = false, messages = errorMessages });
            }

            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "User already exists")
            {
                return StatusCode(408, new { success = false, messages = "User already exists" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { error = ex.Message, success = false }); 
            }
        }





        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true }); 
            }
            catch (CustomException ex) when (ex.Message == "UserEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "User not found" }); 
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false }); 
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var result = await commander.Call(command, cancellationToken);
                return StatusCode(200, new { success = true });
            }
            catch (CustomException ex) when (ex.Message == "UserEntity Not Found")
            {
                return StatusCode(408, new { success = false, messages = "User not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginIncome login)
        {
            try
            {
                var result = await userService.Login(login.PhoneNumber, login.Password);
                return StatusCode(200, new { success = true, messages = result });
            }
            catch (CustomException ex) when (ex.Message == "User was not found")
            {
                return StatusCode(408, new { success = false, messages = "User not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }

        }

        [HttpPost("admin/login")]
        public async Task<ActionResult<string>> AdminLogin(LoginIncome login)
        {
            try
            {
                var result = await userService.AdminLogin(login.PhoneNumber, login.Password);
                return StatusCode(200, new { success = true, messages = result });
            }
            catch (CustomException ex) when (ex.Message == "Admin user not found")
            {
                return StatusCode(408, new { success = false, messages = "Admin user not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }

        }


        [HttpGet("return/user")]
        public async Task<ActionResult<UserView>> Return([FromHeader] string token, CancellationToken cancellationToken)
        {
            try
            {
                var result = await userService.GetByToken(token, cancellationToken);
                return StatusCode(200, new { success = true, message = result });
            }
            catch (CustomException ex) when (ex.Message == "User was not found")
            {
                return StatusCode(408, new { success = false, messages = "User not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }

        [HttpGet("get/all")]
        public async Task<TableResponse<UserView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await userService.GetAll(options, cancellationToken);
        }

        [HttpGet("get")]
        public async Task<ActionResult<UserResultView>> Get(long Id)
        {
            try
            {
                var user = await userService.Get(Id);
                return StatusCode(200, new { success = true, messages = user });
            }
            catch (CustomException ex) when (ex.Message == "User was not found")
            {
                return StatusCode(408, new { success = false, messages = "User not found" });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, success = false });
            }
        }
    }

    
}
