using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UFCApi.CSVObjects
{
    [Table("EventsCsv")]
    [Index(nameof(EventDate))]
    public class EventCsv
    {
        [Key]
        [StringLength(50)]
        public string EventId { get; set; } = string.Empty;

        [StringLength(255)]
        public string EventName { get; set; } = string.Empty;

        public DateTime? EventDate { get; set; }

        [StringLength(100)]
        public string EventCity { get; set; } = string.Empty;

        [StringLength(100)]
        public string EventState { get; set; } = string.Empty;

        [StringLength(100)]
        public string EventCountry { get; set; } = string.Empty;
    }
}
