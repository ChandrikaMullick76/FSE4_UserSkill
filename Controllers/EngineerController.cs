using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSkillProfiles.BusinessLogicLayer;
using UserSkillProfiles.Models;

namespace UserSkillProfiles.Controllers
{
    [Route("/skill-tracker/api/v1/[controller]")]
    [ApiController]
    public class EngineerController : ControllerBase
    {
        private readonly IUserSkillProfileService_BL _userBL;

        public EngineerController(IUserSkillProfileService_BL userBL)
        {
            _userBL = userBL;
        }
              

        [HttpGet]
        [Route("GetAllUserSkillProfiles")]
        public ActionResult<List<UserSkillProfile>> GetAllUserSkillProfiles()
        {
            var result = _userBL.GetUserSkillProfileDetails();

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        [Route("add-profile")]
        public IActionResult CreateUserSkillProfile(UserSkillProfile user)
        {
            bool isSuccess = _userBL.CreateUserSkilProfile(user);

            if (isSuccess)
            {
                // return CreatedAtRoute("GetSingleBuyerProductPair", new { buyerID = buyer.Email, productID = buyer.ProductID }, buyer);
                return StatusCode(200);
            }
            else
            {
                return StatusCode(500);
            }

        }

        [HttpPut]
        [Route("update-profile/{userid}")]
        public IActionResult UpdateUserSkillProfile(string userid, UserSkillProfile user)
        {
            var result = _userBL.GetUserbyUserId(userid);

                      

            if (result == null)
            {
                return NotFound();
            }
            if (_userBL.UpdateUserSkilProfile(userid, user))
            {
                return StatusCode(200);
            }
           else
            {
                return StatusCode(500,"gjgjg");
            }

        }

    }
}
