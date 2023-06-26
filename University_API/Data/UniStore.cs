
using University_API.Models;

namespace University_API.Data
{
    public class UniStore
    {
         public static List<University> uniList = new List<University>
            {
                new University{Id=1,Name="Big U",Country="SG",Webpages="http://bigu.edu.sg",IsBookmark=false},
                    new University{Id=2,Name="UAS",Country="SG",Webpages="http://uas.edu.sg",IsBookmark=false},
                        new University { Id = 3, Name = "NTUC", Country = "SG", Webpages = "http://ntuc.edu.sg", IsBookmark = false }
            };
    }
}
