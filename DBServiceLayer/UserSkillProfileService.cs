using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSkillProfiles.Models;

namespace UserSkillProfiles.DBServiceLayer
{
    public class UserSkillProfileService
    {
        private readonly IMongoCollection<UserSkillProfile> _userSkillProfileData;
       
        public UserSkillProfileService(IUserSkillProfileDBDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _userSkillProfileData = database.GetCollection<UserSkillProfile>(settings.UserSkillProfileCollectionName);
           
        }

        public List<UserSkillProfile> GetAllUserSkillProfile() =>
            _userSkillProfileData.Find(b => true).ToList();

        public UserSkillProfile GetUserbyUserId( int userID) =>
           _userSkillProfileData.Find<UserSkillProfile>(b =>  b.UserID == userID).FirstOrDefault();


        public bool CreateUserSkilProfile(UserSkillProfile userData)
        {
            bool isSuccess = false;
            try
            {
                _userSkillProfileData.InsertOne(userData);
                isSuccess = true;
            }
            catch (Exception ex)
            {

            }

            return isSuccess;
        }

        public bool UpdateUserSkilProfile(int userID, UserSkillProfile userData)
        {
            bool isSuccess = false;
            try
            {
                var filter = Builders<UserSkillProfile>.Filter.Eq("UserID", userID);
                   // & Builders<BuyerBidDetails>.Filter.Eq("ProductID", productID);
                var update = Builders<UserSkillProfile>.Update.Set("HTMLCSSJAVASCRIPT", userData.HTMLCSSJAVASCRIPT);
                _userSkillProfileData.UpdateOne(filter, update);
                isSuccess = true;
            }
            catch (Exception ex)
            {

            }

            return isSuccess;
        }

    }
}
