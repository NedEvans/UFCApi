using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;using System;

namespace AzureAPI.Objects;

public class Event
{

    [Key]
    public int Id { get; set; }// Unique identifier for the event

    [Required]
    public DateTime Date { get; set; }// Date of the event

    [Required]
    [MaxLength(100)]
    public string Location { get; set; } = null!;// Location of the event

    [Required]
    [MaxLength(100)]
    public string Description { get; set; } = null!;// Description of the event
}
