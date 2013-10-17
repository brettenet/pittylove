using System.ComponentModel.DataAnnotations;

namespace _04ShareADog.TokenAuth.Models
{
    public class Pitbull
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
    }
}