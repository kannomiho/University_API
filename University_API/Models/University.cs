using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University_API.Models
{
    public class University
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Webpages { get; set; }
        public bool IsBookmark { get; set; } = false;
        public DateTime Created { get; set; }= DateTime.Now;
        public DateTime LastModified { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DeletedAt { get; set; } 

    }
    

}
