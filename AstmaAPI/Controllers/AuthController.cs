using System;
using System.Threading.Tasks;
using AstmaAPI.EF;
using AstmaAPI.Models.API.Common;
using AstmaAPI.Models.API.Request;
using AstmaAPI.Models.DBO;
using AstmaAPI.ViewModels.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstmaAPI.Controllers
{

    [EnableCors("MyPolicy")]
    [Produces("application/json")]
    [Route("api/user")]
    public class AuthController : Controller
    {
        private readonly MainContext _context;

        public AuthController(MainContext context)
        {
            _context = context;
        }

        // POST: api/user
        [HttpPost()]
        public async Task<IActionResult> GetUser([FromBody]BaseRequest auth)
        {
            if (auth == null)
                return BadRequest();

            User user = await _context.Users
                .Include(u => u.UserToken)
                .FirstOrDefaultAsync(u => u.UserToken.Value == auth.Token);

            if (user != null)
            {
                AuthResponse authResponse = await Authenticate(user);
                return Ok(authResponse);
            }
            else
            {
                ModelState.AddModelError("error", "Токен недействителен");
                return BadRequest(ModelState);
            }
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]AuthRequest auth)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == auth.Login && u.Password == auth.Password);

            if (user != null)
            {
                AuthResponse authResponse = await Authenticate(user);
                return Ok(authResponse);
            }
            else
            {
                ModelState.AddModelError("error", "Некорректные логин и(или) пароль");
                return BadRequest(ModelState);
            }
        }

        // POST: api/user/edit
        [HttpPost("edit")]
        public async Task<IActionResult> EditUser([FromBody]EditUserRequest request)
        {
            User user = await _context.Users
                .Include(u => u.UserToken)
                .FirstOrDefaultAsync(u => u.UserToken.Value == request.Token);

            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name;

            if (request.Height != 0)
                user.Height = request.Height;

            if (request.Weight != 0)
                user.Weight = request.Weight;

            _context.Update(user);

            await _context.SaveChangesAsync();

            if (user != null)
            {
                AuthResponse authResponse = await Authenticate(user);
                return Ok(authResponse);
            }
            else
            {
                ModelState.AddModelError("error", "Токен недействителен");
                return BadRequest(ModelState);
            }
        }

        // POST: api/user/signup
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody]SignupRequest auth)
        {
            User newUser = new User
            {
                BirthDate = auth.BirthDate,
                Height = auth.Height,
                Login = auth.Login,
                Name = auth.Name,
                Password = auth.Password,
                Sex = auth.Sex,
                Surname = auth.Surname,
                Weight = auth.Weight
            };

            try
            {
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            AuthResponse authResponse = await Authenticate(newUser);
           
            return Ok(authResponse);
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

            var bounds = await CalculatePeakFlowmetryBounds(user);

            return new AuthResponse
            {
                Token = token.Value,
                User = user,
                PeakFlowmetryBounds = bounds
            };
        }

        private async Task<UserPeakFlowmetryBounds> CalculatePeakFlowmetryBounds(User user)
        {
            UserPeakFlowmetryBounds result = null;

            int tableAge = UserPeakFlowmetryBounds.GetTableAgeValue(user);
            int tableHeight = UserPeakFlowmetryBounds.GetTableHeightValue(user);

            PeakFlowmetryBound userFlowmetryRate = null;

            if (UserPeakFlowmetryBounds.IsChild(user.BirthDate, out int userAge))
            {
                userFlowmetryRate = await _context.PeakFlowmetryBounds
                    .FirstOrDefaultAsync(fb => fb.Age == tableAge && fb.Height == tableHeight);
            }
            else
            {
                userFlowmetryRate = await _context.PeakFlowmetryBounds
                    .FirstOrDefaultAsync(fb => fb.Age == tableAge && fb.Height == tableHeight && fb.Sex == user.Sex);
            }

            if (userFlowmetryRate != null)
                result = new UserPeakFlowmetryBounds(userFlowmetryRate.Value);

            return result;
        }
    }
}