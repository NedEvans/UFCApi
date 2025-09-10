using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzureAPI.Objects;

public class Fight
{
    [Key]
    public int Id { get; set; }// Unique identifier for the fight

    [ForeignKey("RedCornerFighter")]
    public int RedCornerId { get; set; }// Id of the fighter in the red corner

    [ForeignKey("BlueCornerFighter")]
    public int BlueCornerId { get; set; }// Id of the fighter in the blue corner

    [Column(TypeName = "tinyint")]
    public WeightClass WeightClass { get; set; }// Weight class of the fight

    [ForeignKey("Event")]
    public int EventId { get; set; }// Id of the event where the fight took place
    public int WinnerId { get; set; } // Id of the winning fighter

    [Column(TypeName = "tinyint")]
    public MethodOfVictory MethodOfVictory { get; set; } = MethodOfVictory.Decision;// Method of victory

    [Column(TypeName = "tinyint")]
    public byte Round { get; set; }// Round in which the fight ended
    public TimeSpan Time { get; set; } // Time in the round when the fight ended

    public bool IsTitleFight { get; set; } // Indicates if the fight was a title fight

    //red corner stats
    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedHeadStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant head strikes by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedBodyStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant body strikes by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedLegStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant leg strikes by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedDistanceStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant distance strikes by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedClinchStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant clinch strikes by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedGroundStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant ground strikes by the red corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort RedTotalStrikes { get; set; } = 0; // Total significant strikes by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedTakedowns { get; set; } = [0, 0, 0, 0, 0,];// Number of takedowns by the red corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort RedTotalTakedowns { get; set; } = 0; // Total takedowns by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedSubAttempts { get; set; } = [0, 0, 0, 0, 0]; // Number of submission attempts by the red corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort RedTotalSubAttempts { get; set; } = 0; // Total submission attempts by the red corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] RedKnockdowns { get; set; } = [0, 0, 0, 0, 0]; // red knockdowns by round

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort RedTotalKnockdowns { get; set; } = 0; // Total knockdowns by the red corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public short RedOdds { get; set; } = 0; // Moneyline odds for the red corner

    //blue corner stats
    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueHeadStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant head strikes by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueBodyStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant body strikes by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueLegStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant leg strikes by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueDistanceStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant distance strikes by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueClinchStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant clinch strikes by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueGroundStrikes { get; set; } = [0, 0, 0, 0, 0,];// Number of significant ground strikes by the blue corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort BlueTotalStrikes { get; set; } = 0; // Total significant strikes by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueTakedowns { get; set; } = [0, 0, 0, 0, 0,];// Number of takedowns by the blue corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort BlueTotalTakedowns { get; set; } = 0; // Total takedowns by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueSubAttempts { get; set; } = [0, 0, 0, 0, 0]; // Number of submission attempts by the red corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort BlueTotalSubAttempts { get; set; } = 0; // Total submission attempts by the blue corner

    [Column(TypeName = "BINARY(5)")]  // fixed-length 5-byte row
    public byte[] BlueKnockdowns { get; set; } = [0, 0, 0, 0, 0]; // blue knockdowns by round

    [Column(TypeName = "SMALLINT")]  // DB type
    public ushort BlueTotalKnockdowns { get; set; } = 0; // Total knockdowns by the blue corner

    [Column(TypeName = "SMALLINT")]  // DB type
    public short BlueOdds { get; set; } = 0; // Moneyline odds for the blue corner
}

public enum MethodOfVictory : byte
{
    Decision = 0,
    Knockout = 1,
    Submission = 2,
    Disqualification = 3,
    NoContest = 4
}