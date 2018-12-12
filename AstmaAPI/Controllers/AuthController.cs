﻿using System;
using System.Threading.Tasks;
using AstmaAPI.EF;
using AstmaAPI.Models.DBO;
using AstmaAPI.ViewModels.Request;
using AstmaAPI.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstmaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/login")]
    public class AuthController : Controller
    {
        private readonly DataContext _context;

        public AuthController(DataContext context)
        {
            _context = context;
        }

        // POST: api/login
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AuthRequest auth)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == auth.Login && u.Password == auth.Password);

            if (user != null)
                return Ok(await Authenticate(user));
            else
            {
                ModelState.AddModelError("error", "Некорректные логин и(или) пароль");
                return BadRequest(ModelState);
            }
        }

        private async Task<AuthResponse> Authenticate(User user)
        {
            UserToken token = await _context.UserTokens.FirstOrDefaultAsync(tok => tok.UserId == user.ID);

            if (token == null)
            {
                token = new UserToken { Value = Guid.NewGuid().ToString(), DateOfExpire = DateTime.Now.AddDays(3), User = user, UserId = user.ID };
                await _context.UserTokens.AddAsync(token);

                await _context.SaveChangesAsync();
            }
            else if (token.DateOfExpire < DateTime.Now)
            {
                token.Value = Guid.NewGuid().ToString();
                token.DateOfExpire = DateTime.Now.AddDays(3);

                _context.UserTokens.Update(token);

                await _context.SaveChangesAsync();
            }

            return new AuthResponse { Token = token.Value };
        }
    }
}