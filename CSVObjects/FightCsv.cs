using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UFCApi.CSVObjects
{
    [Table("FightsCsv")]
    [Index(nameof(EventId))]
    [Index(nameof(Fighter1Id))]
    [Index(nameof(Fighter2Id))]
    [Index(nameof(WinnerId))]
    public class FightCsv
    {
        [Key]
        [StringLength(50)]
        public string FightId { get; set; } = string.Empty;

        [StringLength(50)]
        public string EventId { get; set; } = string.Empty;
        public EventCsv? Event { get; set; }

        [StringLength(100)]
        public string Referee { get; set; } = string.Empty;

        [StringLength(50)]
        public string Fighter1Id { get; set; } = string.Empty;
        public FighterCsv? Fighter1 { get; set; }

        [StringLength(50)]
        public string Fighter2Id { get; set; } = string.Empty;
        public FighterCsv? Fighter2 { get; set; }

        [StringLength(50)]
        public string WinnerId { get; set; } = string.Empty;
        public FighterCsv? Winner { get; set; }

        public int NumRounds { get; set; }

        public bool? TitleFight { get; set; }

        [StringLength(100)]
        public string WeightClass { get; set; } = string.Empty;

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(50)]
        public string Result { get; set; } = string.Empty;

        [StringLength(255)]
        public string ResultDetails { get; set; } = string.Empty;

        public int FinishRound { get; set; }

        [StringLength(10)]
        public string FinishTime { get; set; } = string.Empty;

        [StringLength(50)]
        public string TimeFormat { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Scores1 { get; set; } = string.Empty;

        [StringLength(50)]
        public string Scores2 { get; set; } = string.Empty;
    }
}
