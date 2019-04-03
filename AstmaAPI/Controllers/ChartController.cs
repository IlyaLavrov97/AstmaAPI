using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AstmaAPI.EF;
using AstmaAPI.Helpers;
using AstmaAPI.Models;
using AstmaAPI.Models.API.Request;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstmaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/chart")]
    public class ChartController : BaseController
    {
        private readonly MainContext _context;
        private IConverter _converter;

        public ChartController(MainContext context, IHostingEnvironment hostingEnvironment, IConverter converter)
            :base (context, hostingEnvironment)
        {
            _context = context;
            _converter = converter;
        }

        [HttpPost("report")]
        public async Task<IActionResult> GetValuesReport([FromBody] GetReportByDateRequest request)
        {
            return await MethodWrapper(async (token) =>
            {
                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = $"PDF Report {token.User.Name} {DateTime.Now.ToLongDateString()}",
                    //Out = @"C:\Users\Lavrov\pdf\mypdf.pdf"
                };

                List<ChartValue> values = null;

                if (request.StartDate != default(DateTime) && request.EndDate != default(DateTime))
                    values = await Context.ChartValues?.Where(v => v.Date <= request.EndDate && v.Date >= request.StartDate && v.UserId == token.UserId)?.ToListAsync();
                else
                    values = await Context.ChartValues?.Where(v => v.UserId == token.UserId).ToListAsync();

                if (values == null)
                    return BadRequest();

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = TemplateGenerator.GetHTMLString(token.User, values),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var file = _converter.Convert(pdf);
                return File(file, "application/pdf", $"PDF Report {token.User.Name} {DateTime.Now.ToLongDateString()}.pdf");
            }, request);
        }


        [HttpPost]
        public async Task<IActionResult> GetValuesByDate([FromBody] GetValuesByDateRequest request)
        {
            return await MethodWrapper(async (token) =>
            {
                List<ChartValue> values;

                if (request.StartDate != default(DateTime) && request.EndDate != default(DateTime))
                    values = await Context.ChartValues?.Where(v => v.Date <= request.EndDate && v.Date >= request.StartDate && v.UserId == token.UserId)?.ToListAsync();
                else
                    values = await Context.ChartValues?.Where(v => v.UserId == token.UserId).ToListAsync();

                if (values == null)
                    return NotFound();

                return Ok(values);
            }, request);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddChartValue([FromBody] AddValueRequest request)
        {
            return await MethodWrapper(async (token) =>
            {
                var newValue = new ChartValue
                {
                    Value = request.Value,
                    Date = request.Date,
                    IsMorning = request.IsMorning,
                    UserId = token.UserId
                };

                await Context.ChartValues.AddAsync(newValue);

                await Context.SaveChangesAsync();

                return Ok(newValue);
            }, request);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditValue([FromBody] EditValueRequest request)
        {
            return await MethodWrapper(async (token) =>
            {
                ChartValue chartValue = await Context.ChartValues.FirstOrDefaultAsync(art => art.ID == request.ID);

                if (request.Value != 0)
                    chartValue.Value = request.Value;

                Context.ChartValues.Update(chartValue);

                await Context.SaveChangesAsync();

                return Ok(chartValue);
            }, request);
        }
    }
}