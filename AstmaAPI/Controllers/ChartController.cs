using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AstmaAPI.EF;
using AstmaAPI.Models;
using AstmaAPI.ViewModels.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstmaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/chart")]
    public class ChartController : BaseController
    {
        private readonly DataContext _context;

        public ChartController(DataContext context, IHostingEnvironment hostingEnvironment)
            :base (context, hostingEnvironment)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GetValuesByDate([FromBody] GetValuesByDateRequest request)
        {
            return await MethodWrapper(async (token, param) =>
            {
                List<ChartValue> values;

                if (request.StartDate != default(DateTime) && request.EndDate != default(DateTime))
                    values = await Context.ChartValues?.Where(v => v.Date <= request.EndDate && v.Date >= request.StartDate)?.ToListAsync();
                else
                    values = await Context.ChartValues?.ToListAsync();

                if (values == null)
                    return NotFound();

                return Ok(values);
            }, request);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddChartValue([FromBody] AddValueRequest request)
        {
            return await MethodWrapper(async (token, param) =>
            {
                var newValue = new ChartValue
                {
                    Value = param.Value,
                    Date = param.Date,
                    IsMorning = param.IsMorning,
                };

                await Context.ChartValues.AddAsync(newValue);

                await Context.SaveChangesAsync();

                return Ok(newValue);
            }, request);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditArticle([FromBody] EditValueRequest request)
        {
            return await MethodWrapper(async (token, param) =>
            {
                ChartValue chartValue = await Context.ChartValues.FirstOrDefaultAsync(art => art.ID == param.ID);

                if (param.Value != 0)
                    chartValue.Value = param.Value;

                Context.ChartValues.Update(chartValue);

                await Context.SaveChangesAsync();

                return Ok(chartValue);
            }, request);
        }
    }
}