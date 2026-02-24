
using Microsoft.AspNetCore.Identity;

namespace VirasureYouAI.Entities
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ImageURL { get; set; }
        public string? Education { get; set; }
        public string? City { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Article> Articles { get; set; }
        public List<Policy> Policies { get; set; }
    }
}
