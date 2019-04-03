using AstmaAPI.Models;
using AstmaAPI.Models.DBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstmaAPI.Helpers
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(User user, List<ChartValue> chartValues)
        {
            if (chartValues == null)
                return string.Empty;

            var sb = new StringBuilder();

            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>");

            sb.AppendFormat(@"<div class='header'><h1>{0}</h1></div>
                              <div class='header'><h2>{1}</h2></div>", 
                              user.Name + " " + user.Surname + " " + user.BirthDate.ToShortDateString(), 
                              user.Height + " см " + user.Weight + " кг ");

            sb.Append(@"<table align='center'>
                                    <tr>
                                        <th>Date</th>
                                        <th>Is morning</th>
                                        <th>Value</th>
                                    </tr>");

            foreach (var cv in chartValues)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                  </tr>", cv.Date.ToShortDateString(), cv.IsMorning ? "yes" : "no", cv.Value);
            }

            sb.Append(@"</table>
                        </body>
                        </html>");

            return sb.ToString();
        }
    }
}
