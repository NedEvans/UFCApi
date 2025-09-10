using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AzureAPI.Objects;

public class Fighter
{
    [Key]
    public int Id { get; set; }// Unique identifier for the fighter

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;// Fighter's full name

    [Required]
    [MaxLength(50)]
    public string Country { get; set; } = null!;// Fighter's country of origin

    public DateTime BirthDate { get; set; }// Fighter's date of birth

    [Column(TypeName = "tinyint")]
    public WeightClass WeightClass { get; set; }// Fighter's weight class

    [Column(TypeName = "tinyint")]
    public byte Reach { get; set; }// Fighter's reach in inches

    [Column(TypeName = "tinyint")]
    public byte Height { get; set; }// Fighter's height in inches

    [Column(TypeName = "tinyint")]
    public byte LegReach { get; set; }// Fighter's leg reach in inches
    public bool Status { get; set; }// True if active, false if retired

    [Required]
    public byte[] Image { get; set; } = [];// Fighter's image stored as a byte array

    [Column(TypeName = "tinyint")]
    public Gender Gender { get; set; } = Gender.Male;// Fighter's gender

 

}

public enum Gender : byte
{
    Male = 0,
    Female = 1
}

public enum WeightClass : byte
{
    Flyweight = 0,
    Bantamweight = 1,
    Featherweight = 2,
    Lightweight = 3,
    Welterweight = 4,
    Middleweight = 5,
    LightHeavyweight = 6,
    Heavyweight = 7
}
