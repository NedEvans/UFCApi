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

        [HttpPost("fights")]
        public async Task<IActionResult> ImportFights()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "fight_data.csv");
            var lines = await System.IO.File.ReadAllLinesAsync(path);

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var values = line.Split(',');
                var fight = new FightCsv
                {
                    FightId = GetValue(values, 0),
                    EventId = GetValue(values, 1),
                    Referee = GetValue(values, 2),
                    Fighter1Id = GetValue(values, 3),
                    Fighter2Id = GetValue(values, 4),
                    WinnerId = GetValue(values, 5),
                    NumRounds = ParseInt(values, 6),
                    TitleFight = ParseBool(values, 7),
                    WeightClass = GetValue(values, 8),
                    Gender = GetValue(values, 9),
                    Result = GetValue(values, 10),
                    ResultDetails = GetValue(values, 11),
                    FinishRound = ParseInt(values, 12),
                    FinishTime = GetValue(values, 13),
                    TimeFormat = GetValue(values, 14),
                    Scores1 = GetValue(values, 15),
                    Scores2 = GetValue(values, 16)
                };

                if (string.IsNullOrEmpty(fight.FightId))
                {
                    continue;
                }

                _context.FightsCsv.Add(fight);
            }

            await _context.SaveChangesAsync();

            return Ok("Fights imported successfully.");
        }

        [HttpPost("rounds")]
        public async Task<IActionResult> ImportRounds()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "round_data.csv");
            var lines = await System.IO.File.ReadAllLinesAsync(path);

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var values = line.Split(',');
                var round = new RoundCsv
                {
                    FightId = GetValue(values, 0),
                    FighterId = GetValue(values, 1),
                    Round = ParseInt(values, 2),
                    Knockdowns = ParseInt(values, 3),
                    StrikesAtt = ParseInt(values, 4),
                    StrikesSucc = ParseInt(values, 5),
                    HeadStrikesAtt = ParseInt(values, 6),
                    HeadStrikesSucc = ParseInt(values, 7),
                    BodyStrikesAtt = ParseInt(values, 8),
                    BodyStrikesSucc = ParseInt(values, 9),
                    LegStrikesAtt = ParseInt(values, 10),
                    LegStrikesSucc = ParseInt(values, 11),
                    DistanceStrikesAtt = ParseInt(values, 12),
                    DistanceStrikesSucc = ParseInt(values, 13),
                    GroundStrikesAtt = ParseInt(values, 14),
                    GroundStrikesSucc = ParseInt(values, 15),
                    ClinchStrikesAtt = ParseInt(values, 16),
                    ClinchStrikesSucc = ParseInt(values, 17),
                    TotalStrikesAtt = ParseInt(values, 18),
                    TotalStrikesSucc = ParseInt(values, 19),
                    TakedownAtt = ParseInt(values, 20),
                    TakedownSucc = ParseInt(values, 21),
                    SubmissionAtt = ParseInt(values, 22),
                    Reversals = ParseInt(values, 23),
                    CtrlTime = GetValue(values, 24)
                };

                if (string.IsNullOrEmpty(round.FightId) || string.IsNullOrEmpty(round.FighterId) || round.Round == 0)
                {
                    continue;
                }

                _context.RoundsCsv.Add(round);
            }

            await _context.SaveChangesAsync();

            return Ok("Rounds imported successfully.");
        }

        private static string GetValue(string[] values, int index)
        {
            if (index < 0 || index >= values.Length)
            {
                return string.Empty;
            }

            return values[index].Trim();
        }

        private static int ParseInt(string[] values, int index)
        {
            var value = GetValue(values, index);
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : 0;
        }

        private static bool? ParseBool(string[] values, int index)
        {
            var value = GetValue(values, index);

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.Equals("t", StringComparison.OrdinalIgnoreCase) || value.Equals("true", StringComparison.OrdinalIgnoreCase) || value.Equals("1", StringComparison.OrdinalIgnoreCase) || value.Equals("y", StringComparison.OrdinalIgnoreCase) || value.Equals("yes", StringComparison.OrdinalIgnoreCase)
                ? true
                : value.Equals("f", StringComparison.OrdinalIgnoreCase) || value.Equals("false", StringComparison.OrdinalIgnoreCase) || value.Equals("0", StringComparison.OrdinalIgnoreCase) || value.Equals("n", StringComparison.OrdinalIgnoreCase) || value.Equals("no", StringComparison.OrdinalIgnoreCase)
                    ? false
                    : (bool?)null;
        }
    }
}
