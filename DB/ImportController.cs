using Microsoft.AspNetCore.Mvc;
using AzureAPI.DB;
using UFCApi.CSVObjects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;

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

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                var anEvent = new EventCsv
                {
                    EventId = values[0].Trim(),
                    EventName = values[1].Trim(),
                    EventCity = values[3].Trim(),
                    EventState = values[4].Trim(),
                    EventCountry = values[5].Trim()
                };

                if (DateTime.TryParse(values[2], out var eventDate))
                    anEvent.EventDate = eventDate;

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

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');

                var fighter = new FighterCsv
                {
                    FighterId = values[0].Trim(),
                    FighterFName = values[1].Trim(),
                    FighterLName = values[2].Trim(),
                    FighterNickname = values[3].Trim(),
                    FighterStance = values[7].Trim(),
                };

                if (double.TryParse(values[4], out var height))
                    fighter.FighterHeightCm = height;

                if (double.TryParse(values[5], out var weight))
                    fighter.FighterWeightLbs = weight;

                if (double.TryParse(values[6], out var reach))
                    fighter.FighterReachCm = reach;

                if (DateTime.TryParse(values[8], out var dob))
                    fighter.FighterDob = dob;

                if (int.TryParse(values[9], out var w))
                    fighter.FighterW = w;

                if (int.TryParse(values[10], out var l))
                    fighter.FighterL = l;

                if (int.TryParse(values[11], out var d))
                    fighter.FighterD = d;

                if (int.TryParse(values[12], out var ncdq))
                    fighter.FighterNcDq = ncdq;

                _context.FightersCsv.Add(fighter);
            }

            await _context.SaveChangesAsync();
            return Ok("Fighters imported successfully.");
        }

        [HttpPost("fights")]
        public async Task<IActionResult> ImportFights()
        {
            var eventIds = new HashSet<string>(
                await _context.EventsCsv.Select(e => e.EventId.Trim()).ToListAsync(),
                StringComparer.OrdinalIgnoreCase);

            var fighterIds = new HashSet<string>(
                await _context.FightersCsv.Select(f => f.FighterId.Trim()).ToListAsync(),
                StringComparer.OrdinalIgnoreCase);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "fight_data.csv");
            var importedCount = 0;
            var skippedFights = new List<string>();

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                if (!parser.EndOfData) parser.ReadLine(); // skip header

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    if (fields == null || fields.Length < 17) continue;

                    var fightId = fields[0].Trim();
                    var eventId = fields[1].Trim();
                    var fighter1Id = fields[3].Trim();
                    var fighter2Id = fields[4].Trim();
                    var winnerIdRaw = fields[5].Trim();

                    if (!eventIds.Contains(eventId)) { skippedFights.Add($"FightId: {fightId} - Missing EventId: {eventId}"); continue; }
                    if (!fighterIds.Contains(fighter1Id)) { skippedFights.Add($"FightId: {fightId} - Missing Fighter1Id: {fighter1Id}"); continue; }
                    if (!fighterIds.Contains(fighter2Id)) { skippedFights.Add($"FightId: {fightId} - Missing Fighter2Id: {fighter2Id}"); continue; }

                    string finalWinnerId = null;
                    if (!(string.IsNullOrWhiteSpace(winnerIdRaw) ||
                          string.Equals(winnerIdRaw, "Draw", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(winnerIdRaw, "NC", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (fighterIds.Contains(winnerIdRaw))
                            finalWinnerId = winnerIdRaw;
                        else
                        {
                            skippedFights.Add($"FightId: {fightId} - Missing WinnerId: {winnerIdRaw}");
                            continue;
                        }
                    }

                    var fight = new FightCsv
                    {
                        FightId = fightId,
                        EventId = eventId,
                        Referee = fields[2].Trim(),
                        Fighter1Id = fighter1Id,
                        Fighter2Id = fighter2Id,
                        WinnerId = finalWinnerId, // NULL instead of ""
                        NumRounds = int.TryParse(fields[6], out var numRounds) ? numRounds : 0,
                        TitleFight = fields[7].Trim() == "T",
                        WeightClass = fields[8].Trim(),
                        Gender = fields[9].Trim(),
                        Result = fields[10].Trim(),
                        ResultDetails = fields[11].Trim(),
                        FinishRound = int.TryParse(fields[12], out var finishRound) ? finishRound : 0,
                        FinishTime = fields[13].Trim(),
                        TimeFormat = fields[14].Trim(),
                        Scores1 = fields[15].Trim(),
                        Scores2 = fields[16].Trim()
                    };

                    _context.FightsCsv.Add(fight);
                    importedCount++;
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Fights import completed.", SuccessfullyImported = importedCount, SkippedFights = skippedFights });
        }

        [HttpGet("diagnose-fighters-trimmed")]
        public async Task<IActionResult> DiagnoseFightersTrimmed()
        {
            var fighters = await _context.FightersCsv
                .Select(f => new { f.FighterId, f.FighterFName, f.FighterLName })
                .ToListAsync();

            var untrimmed = fighters
                .Where(f =>
                    f.FighterId != f.FighterId.Trim() ||
                    f.FighterFName != f.FighterFName.Trim() ||
                    f.FighterLName != f.FighterLName.Trim())
                .ToList();

            return Ok(new
            {
                TotalFighters = fighters.Count,
                UntrimmedCount = untrimmed.Count,
                Examples = untrimmed.Take(10)
            });
        }


       [HttpPost("rounds")]
public async Task<IActionResult> ImportRounds()
{
    var fightIds = new HashSet<string>(
        await _context.FightsCsv.Select(f => f.FightId.Trim()).ToListAsync(),
        StringComparer.OrdinalIgnoreCase);

    var fighterIds = new HashSet<string>(
        await _context.FightersCsv.Select(f => f.FighterId.Trim()).ToListAsync(),
        StringComparer.OrdinalIgnoreCase);

    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "round_data.csv");
    var importedCount = 0;
    var skippedRounds = new List<string>();

    using (TextFieldParser parser = new TextFieldParser(path))
    {
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;

        if (!parser.EndOfData) parser.ReadLine(); // skip header

        while (!parser.EndOfData)
        {
            string[] fields = parser.ReadFields();
            if (fields == null || fields.Length < 25) continue;

            var fightId = fields[0].Trim();
            var fighterId = fields[1].Trim();

            if (!fightIds.Contains(fightId))
            {
                skippedRounds.Add($"Round skipped - missing FightId: {fightId}");
                continue;
            }

            if (!fighterIds.Contains(fighterId))
            {
                skippedRounds.Add($"Round skipped - missing FighterId: {fighterId}");
                continue;
            }

            var round = new RoundCsv
            {
                FightId = fightId,
                FighterId = fighterId,
                Round = int.TryParse(fields[2], out var roundNum) ? roundNum : 0,
                Knockdowns = int.TryParse(fields[3], out var knockdowns) ? knockdowns : 0,
                StrikesAtt = int.TryParse(fields[4], out var strikesAtt) ? strikesAtt : 0,
                StrikesSucc = int.TryParse(fields[5], out var strikesSucc) ? strikesSucc : 0,
                HeadStrikesAtt = int.TryParse(fields[6], out var headAtt) ? headAtt : 0,
                HeadStrikesSucc = int.TryParse(fields[7], out var headSucc) ? headSucc : 0,
                BodyStrikesAtt = int.TryParse(fields[8], out var bodyAtt) ? bodyAtt : 0,
                BodyStrikesSucc = int.TryParse(fields[9], out var bodySucc) ? bodySucc : 0,
                LegStrikesAtt = int.TryParse(fields[10], out var legAtt) ? legAtt : 0,
                LegStrikesSucc = int.TryParse(fields[11], out var legSucc) ? legSucc : 0,
                DistanceStrikesAtt = int.TryParse(fields[12], out var distAtt) ? distAtt : 0,
                DistanceStrikesSucc = int.TryParse(fields[13], out var distSucc) ? distSucc : 0,
                GroundStrikesAtt = int.TryParse(fields[14], out var groundAtt) ? groundAtt : 0,
                GroundStrikesSucc = int.TryParse(fields[15], out var groundSucc) ? groundSucc : 0,
                ClinchStrikesAtt = int.TryParse(fields[16], out var clinchAtt) ? clinchAtt : 0,
                ClinchStrikesSucc = int.TryParse(fields[17], out var clinchSucc) ? clinchSucc : 0,
                TotalStrikesAtt = int.TryParse(fields[18], out var totalAtt) ? totalAtt : 0,
                TotalStrikesSucc = int.TryParse(fields[19], out var totalSucc) ? totalSucc : 0,
                TakedownAtt = int.TryParse(fields[20], out var tdAtt) ? tdAtt : 0,
                TakedownSucc = int.TryParse(fields[21], out var tdSucc) ? tdSucc : 0,
                SubmissionAtt = int.TryParse(fields[22], out var subAtt) ? subAtt : 0,
                Reversals = int.TryParse(fields[23], out var reversals) ? reversals : 0,
                CtrlTime = fields[24].Trim()
            };

            _context.RoundsCsv.Add(round);
            importedCount++;
        }
    }

    await _context.SaveChangesAsync();
    return Ok(new { Message = "Rounds import completed.", SuccessfullyImported = importedCount, SkippedRounds = skippedRounds });
}



        [HttpGet("diagnose-rounds")]
        public async Task<IActionResult> DiagnoseRounds()
        {
            var fightIds = new HashSet<string>(await _context.FightsCsv.Select(f => f.FightId.Trim()).ToListAsync(), StringComparer.OrdinalIgnoreCase);
            var fighterIds = new HashSet<string>(await _context.FightersCsv.Select(f => f.FighterId.Trim()).ToListAsync(), StringComparer.OrdinalIgnoreCase);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "round_data.csv");
            var lines = await System.IO.File.ReadAllLinesAsync(path);

            var missingFights = 0;
            var missingFighters = 0;
            var malformed = 0;

            foreach (var line in lines.Skip(1))
            {
                var fields = line.Split(',');
                if (fields.Length < 25)
                {
                    malformed++;
                    continue;
                }

                var fightId = fields[0].Trim();
                var fighterId = fields[1].Trim();

                if (!fightIds.Contains(fightId)) missingFights++;
                if (!fighterIds.Contains(fighterId)) missingFighters++;
            }

            return Ok(new
            {
                TotalRows = lines.Length - 1,
                Malformed = malformed,
                MissingFightIds = missingFights,
                MissingFighterIds = missingFighters
            });
        }

    }
}
