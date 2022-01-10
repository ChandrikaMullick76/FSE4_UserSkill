using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSkillProfiles.Models;

namespace UserSkillProfiles.BusinessLogicLayer
{
    public interface IUserSkillProfileService_BL
    {
        public bool CreateUserSkilProfile(UserSkillProfile user);

        public List<UserSkillProfile> GetUserSkillProfileDetails();

        public UserSkillProfile GetUserbyUserId(string userID);

        public bool UpdateUserSkilProfile(string userID, UserSkillProfile userData);
    }
}
