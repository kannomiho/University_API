using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using University_API.Models;
using University_API.Repository.IRepository;

namespace University_API.Controllers
{

    [Route("api/UniversityAPI")]
    [ApiController]
    public class UniversityAPIController : ControllerBase
    {
        private readonly IUniRepository _dbUni;

        private string Secret="";
        public UniversityAPIController(IUniRepository dbUni,IConfiguration configuration)
        {
            _dbUni = dbUni;
             Secret = configuration.GetValue<string>("ApiSettings:Secret");
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<University>>> GetUniversities([FromQuery] string? SearchName,int pageSize=0,int pageNumber=1 )
        {
            IEnumerable<University> uniList;
 
            
            if(string.IsNullOrEmpty(SearchName))
            {
                uniList = await _dbUni.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber);
            }
            else
            {
                uniList = await _dbUni.GetAllAsync(u=> u.Name.ToLower().Contains(SearchName), pageSize: pageSize, pageNumber: pageNumber);
            }

            return Ok(uniList);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<University>> GetUniversity(int id)
        {
            if (id == 0)
            {
                ModelState.AddModelError("", "invalid id");
                return BadRequest(ModelState);
            }

            var uni = await _dbUni.GetAsync(u => u.Id == id);

            if (uni == null)
            {
                ModelState.AddModelError("", "id not found");
                return NotFound(ModelState);
            }

            return Ok(uni);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<University>> CreateUni([FromBody] University uni)
        {

            if (await _dbUni.GetAsync(u => u.Name.ToLower() == uni.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "already exists");
                return BadRequest(ModelState);
            }

            if (uni == null)
            {
                return BadRequest(uni);
            }

            await _dbUni.CreateAsync(uni);

            return Ok(uni);

        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteUniversity(int id)
        {
            if(id == 0)
            {
                ModelState.AddModelError("", "invalid id");
                return BadRequest(ModelState);
            }

            var uni=await _dbUni.GetAsync(u=>u.Id==id);
            if (uni == null)
            {
                ModelState.AddModelError("", "id not found");
                return NotFound(ModelState);
            }

           await _dbUni.RemoveAsync(uni);

            return NoContent(); 
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateUniversity(int id, [FromBody]University uni)
        {
            if(uni == null || id!= uni.Id)
            {
               ModelState.AddModelError("", "invalid id");
                return BadRequest(ModelState);
            } 

            // for demo purpose i use direct assign here. 
            var singleUni =await _dbUni.GetAsync(u=> u.Id==id);

            if(singleUni == null)
            {
                ModelState.AddModelError("", "id not found");
                return NotFound(ModelState);
            }
            singleUni.Name=uni.Name;
            singleUni.Webpages=uni.Webpages;
            singleUni.Country=uni.Country;
            singleUni.IsBookmark=uni.IsBookmark;
            singleUni.LastModified = DateTime.Now;
            singleUni.IsDeleted=uni.IsDeleted;
            singleUni.DeletedAt=uni.DeletedAt;
           await _dbUni.UpdateAsync(singleUni);
    

            return NoContent();

        }


        [HttpPost("bookmark/{id:int}")]
        [Authorize]
        public async Task<ActionResult<University>> CreateBookmark(int id, [FromBody] bool bookmarkFlag)
        {

            var singleUni =await _dbUni.GetAsync(u => u.Id == id);

            if (singleUni == null)
            {
                ModelState.AddModelError("", "id not found");
                return NotFound(ModelState);
            }
            singleUni.IsBookmark= bookmarkFlag;
            singleUni.LastModified = DateTime.Now;
           await _dbUni.UpdateAsync(singleUni);



            return NoContent();

        }

        [HttpGet("/getToken")]
        public ActionResult<string> getToken()
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(tokenHandler.WriteToken(token));
        }


    }//end class
}
