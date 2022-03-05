using CoreCodeCamp.Data;
using System.ComponentModel.DataAnnotations;

namespace PSWebApi.Models
{
    public class CampModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        public Location? Location { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Range(1,100)]
        public int Length { get; set; } = 1;
        public ICollection<TalkModel>? Talks { get; set; }
    }
}
