using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UFCApi.CSVObjects
{
    [Table("RoundsCsv")]
    public class RoundCsv
    {
        [Key, Column(Order = 0)]
        [StringLength(50)]
        public string FightId { get; set; } = string.Empty;
        public FightCsv? Fight { get; set; }

        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string FighterId { get; set; } = string.Empty;
        public FighterCsv? Fighter { get; set; }

        [Key, Column(Order = 2)]
        public int Round { get; set; }

        public int Knockdowns { get; set; }
        public int StrikesAtt { get; set; }
        public int StrikesSucc { get; set; }
        public int HeadStrikesAtt { get; set; }
        public int HeadStrikesSucc { get; set; }
        public int BodyStrikesAtt { get; set; }
        public int BodyStrikesSucc { get; set; }
        public int LegStrikesAtt { get; set; }
        public int LegStrikesSucc { get; set; }
        public int DistanceStrikesAtt { get; set; }
        public int DistanceStrikesSucc { get; set; }
        public int GroundStrikesAtt { get; set; }
        public int GroundStrikesSucc { get; set; }
        public int ClinchStrikesAtt { get; set; }
        public int ClinchStrikesSucc { get; set; }
        public int TotalStrikesAtt { get; set; }
        public int TotalStrikesSucc { get; set; }
        public int TakedownAtt { get; set; }
        public int TakedownSucc { get; set; }
        public int SubmissionAtt { get; set; }
        public int Reversals { get; set; }
        [StringLength(10)]
        public string CtrlTime { get; set; } = string.Empty;
    }
}
