using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UFCApi.CSVObjects
{
    [Table("FightersCsv")]
    [Index(nameof(FighterFName))]
    [Index(nameof(FighterLName))]
    public class FighterCsv
    {
        [Key]
        [StringLength(50)]
        public string FighterId { get; set; } = string.Empty;

        [StringLength(100)]
        public string FighterFName { get; set; } = string.Empty;

        [StringLength(100)]
        public string FighterLName { get; set; } = string.Empty;

        [StringLength(100)]
        public string FighterNickname { get; set; } = string.Empty;

        public double FighterHeightCm { get; set; }

        public double FighterWeightLbs { get; set; }

        public double FighterReachCm { get; set; }

        [StringLength(50)]
        public string FighterStance { get; set; } = string.Empty;

        public DateTime? FighterDob { get; set; }

        public int FighterW { get; set; }

        public int FighterL { get; set; }

        public int FighterD { get; set; }

        public int FighterNcDq { get; set; }
    }
}
