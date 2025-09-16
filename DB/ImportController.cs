using Microsoft.AspNetCore.Mvc;
using AzureAPI.DB;
using UFCApi.CSVObjects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Globalization;

namespace AzureAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ImportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("events")]
        public async Task<IActionResult> ImportEvents()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "event_data.csv");
            var lines = await System.IO.File.ReadAllLinesAsync(path);

            // Skip header
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');

                var anEvent = new EventCsv
                {
                    EventId = values[0],
                    EventName = values[1],
                    EventCity = values[3],
                    EventState = values[4],
                    EventCountry = values[5]
                };

                if (DateTime.TryParse(values[2], out var eventDate))
                {
                    anEvent.EventDate = eventDate;
                }

                _context.EventsCsv.Add(anEvent);
            }

            await _context.SaveChangesAsync();

            return Ok("Events imported successfully.");
        }

        [HttpPost("fighters")]
        public async Task<IActionResult> ImportFighters()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "fighter_data.csv");
            var lines = await System.IO.File.ReadAllLinesAsync(path);

            // Skip header
            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');

                var fighter = new FighterCsv
                {
                    FighterId = values[0],
                    FighterFName = values[1],
                    FighterLName = values[2],
                    FighterNickname = values[3],
                    FighterStance = values[7],
                };

                if (double.TryParse(values[4], out var height))
                    fighter.FighterHeightCm = height;
                else
                    fighter.FighterHeightCm = 0;

                if (double.TryParse(values[5], out var weight))
                    fighter.FighterWeightLbs = weight;
                else
                    fighter.FighterWeightLbs = 0;

                if (double.TryParse(values[6], out var reach))
                    fighter.FighterReachCm = reach;
                else
                    fighter.FighterReachCm = 0;

                if (DateTime.TryParse(values[8], out var dob))
                    fighter.FighterDob = dob;

                if (int.TryParse(values[9], out var w))
                    fighter.FighterW = w;
                else
                    fighter.FighterW = 0;

                if (int.TryParse(values[10], out var l))
                    fighter.FighterL = l;
                else
                    fighter.FighterL = 0;

                if (int.TryParse(values[11], out var d))
                    fighter.FighterD = d;
                else
                    fighter.FighterD = 0;

                if (int.TryParse(values[12], out var ncdq))
                    fighter.FighterNcDq = ncdq;
                else
                    fighter.FighterNcDq = 0;

                _context.FightersCsv.Add(fighter);
            }

            await _context.SaveChangesAsync();

            return Ok("Fighters imported successfully.");
        }
    }
}
