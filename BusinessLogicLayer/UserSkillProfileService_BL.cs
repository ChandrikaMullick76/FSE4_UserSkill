using System;
using System.Collections.Generic;
using System.Linq;
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
            bool validationCheckPassed = false;

            try
            {
                if (ValidateRequestBody(user))
                {
                    /*Product product = _buyerService.GetProductDetails(buyer.ProductID);

                    if (ValidateProductForExistence(buyer.ProductID))
                    {
                        validationCheckPassed = true;
                    }

                    if (validationCheckPassed && !ValidateProductBidDate(buyer.ProductID))
                    {
                        validationCheckPassed = false;
                    }
                    if (validationCheckPassed && !ValidateDuplicateBid(buyer.ProductID, buyer.Email))
                    {
                        validationCheckPassed = false;
                    }*/

                    if (validationCheckPassed)
                    {
                        _userService.CreateUserSkilProfile(user);
                        isSuccess = true;
                    }
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

        public UserSkillProfile GetUserbyUserId(int userID)
        {
            return _userService.GetUserbyUserId(userID);
        }

        public bool UpdateUserSkilProfile(int userID, UserSkillProfile userData)
        {
            bool isValidRequest = true;
            bool isUpdateSuccessful = false;

            //Product product = _buyerService.GetProductDetails(productId);

            //if (DateTime.Now > Convert.ToDateTime(product.BidEndDate))
            //{
            //    isValidRequest = false;
            //    return isValidRequest;
            //}

            if (isValidRequest)
            {
                _userService.UpdateUserSkilProfile(userID, userData);
            }

            return isUpdateSuccessful;

        }

        private bool ValidateRequestBody(UserSkillProfile user)
        {
            bool isFNValid = false,
                isLNValid = false,
                isEmailValid = false,
                isPhoneValid = false;
            /*long phone;
            if (buyer != null)
            {
                if (!string.IsNullOrWhiteSpace(buyer.FirstName) && buyer.FirstName.Length >= 5
                    && buyer.FirstName.Length <= 30)
                {
                    isFNValid = true;
                }
                if (!string.IsNullOrWhiteSpace(buyer.LastName) && buyer.LastName.Length >= 3
                    && buyer.LastName.Length <= 25)
                {
                    isLNValid = true;
                }
                if (!string.IsNullOrWhiteSpace(buyer.Email) && Regex.IsMatch(buyer.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                {
                    isEmailValid = true;
                }
                if (!string.IsNullOrWhiteSpace(buyer.Phone) &&
                    long.TryParse(buyer.Phone, out phone) && buyer.Phone.Length == 10)
                {
                    isPhoneValid = true;
                }

            }*/
            return (isFNValid && isLNValid && isEmailValid && isPhoneValid);
        }
    }
}
