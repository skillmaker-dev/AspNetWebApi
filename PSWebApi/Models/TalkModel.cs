using CoreCodeCamp.Data;
using System.ComponentModel.DataAnnotations;

namespace PSWebApi.Models
{
    public class TalkModel
    {
        public int TalkId { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [Required,StringLength(5000,MinimumLength = 5)]
        public string Abstract { get; set; }

        [Range(100,300)]
        public int Level { get; set; }
        public Speaker? Speaker { get; set; }
    }
}
