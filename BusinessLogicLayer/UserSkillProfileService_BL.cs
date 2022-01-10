using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UserSkillProfiles.DBServiceLayer;
using UserSkillProfiles.Models;

namespace UserSkillProfiles.BusinessLogicLayer
{
    public class UserSkillProfileService_BL:IUserSkillProfileService_BL
    {
        private readonly UserSkillProfileService _userService;

        public UserSkillProfileService_BL(UserSkillProfileService userService)
        {
            _userService = userService;
        }
        public bool CreateUserSkilProfile(UserSkillProfile user)
        {
            bool isSuccess = false;
           

            try
            {
                if (ValidateRequestBody(user))
                {
                        user.CreationDate = DateTime.Now.ToString();
                        _userService.CreateUserSkilProfile(user);
                        isSuccess = true;
                   
                }
            }
            catch (Exception ex)
            {

            }

            return isSuccess;
        }


        public List<UserSkillProfile> GetUserSkillProfileDetails()
        {
            return _userService.GetAllUserSkillProfile();
        }

        public UserSkillProfile GetUserbyUserId(string userID)
        {
            return _userService.GetUserbyUserId(userID);
        }

        public bool UpdateUserSkilProfile(string userID, UserSkillProfile userData)
        {
            bool isValidRequest = true;
            bool isUpdateSuccessful = false;

            try
            {
                UserSkillProfile prevData = _userService.GetUserbyUserId(userID);

                if (Convert.ToDateTime(prevData.CreationDate) > DateTime.Now.AddDays(-10))
                {
                    isValidRequest = false;

                    throw new Exception("Update of profile must be allowed only after 10 days of adding profile or last change");
                    
                }

                if (isValidRequest)
                {
                    userData.CreationDate = DateTime.Now.ToString();
                    userData.UserID = userID;
                    isUpdateSuccessful = _userService.UpdateUserSkilProfile(userID, userData);
                }

                
            }
            catch (Exception ex)
            {

            }
            return isUpdateSuccessful;
        }

        private bool ValidateRequestBody(UserSkillProfile user)
        {
            bool isNAMEValid = false,
                isAssociateIdValid = false,
                isExpertiseValid = false,
                isEmailValid = false,
                isPhoneValid = false;
            long phone;
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Name) && user.Name.Length >= 5
                    && user.Name.Length <= 30)
                {
                    isNAMEValid = true;
                }
                if (!string.IsNullOrWhiteSpace(user.AssociateId) && user.AssociateId.Length >= 5
                    && user.AssociateId.Length <= 30 && user.AssociateId.StartsWith("CTS"))
                {
                    isAssociateIdValid = true;
                }
                if (
                    (user.ANGULAR >= 0
                    && user.ANGULAR <= 20) 
                    && (user.HTMLCSSJAVASCRIPT >= 0
                    && user.HTMLCSSJAVASCRIPT <= 20)
                    )
                {
                    isExpertiseValid = true;
                }
                if (!string.IsNullOrWhiteSpace(user.Email) && Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                {
                    isEmailValid = true;
                }
                if (!string.IsNullOrWhiteSpace(user.Mobile) &&
                    long.TryParse(user.Mobile, out phone) && user.Mobile.Length == 10)
                {
                    isPhoneValid = true;
                }

            }

             return (isNAMEValid && isAssociateIdValid && isExpertiseValid && isEmailValid && isPhoneValid);
            //return true;
        }
    }
}
