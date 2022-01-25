using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSkillProfiles.Models;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

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

        public List<UserSkillProfile> GetAllUserSkillProfile() 
        {
            List<UserSkillProfile> user;
            user = _userSkillProfileData.Find(emp => true).ToList();
            return user;
        }

        public UserSkillProfile GetUserbyUserId( string userID) =>
           _userSkillProfileData.Find<UserSkillProfile>(b =>  b.UserID == userID).FirstOrDefault();


        public bool CreateUserSkilProfile(UserSkillProfile userData)
        {
            bool isSuccess = false;
            try
            {
                var obj = userData;
                _userSkillProfileData.InsertOne(userData);
                AddToServiceBusQueue(obj);
                isSuccess = true;
            }
            catch (Exception ex)
            {

            }

            return isSuccess;
        }

        public bool UpdateUserSkilProfile(string userID, UserSkillProfile userData)
        {
            bool isSuccess = false;
            
            try
            {
                // var filter = Builders<UserSkillProfile>.Filter.Eq("UserID", userID);
                // & Builders<BuyerBidDetails>.Filter.Eq("ProductID", productID);
                //var update = Builders<UserSkillProfile>.Update.Set("HTMLCSSJAVASCRIPT", userData.HTMLCSSJAVASCRIPT);
                // _userSkillProfileData.UpdateOne(filter, update);
                _userSkillProfileData.ReplaceOne(x => x.UserID == userID, userData);
                
                isSuccess = true;
            }
            catch (Exception ex)
            {

            }

            return isSuccess;
        }

        async Task AddToServiceBusQueue(UserSkillProfile userData)
        {
            //bool isSuccess = false;
            try
            {
                // connection string to your Service Bus namespace
                string connectionString = "Endpoint=sb://userskillprofileservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yoviBaucaDjh143g8rJpTlgywKKGDYl+I8KAwIx73lA=";

                // name of your Service Bus queue
                 string queueName = "userskillprofilequeue";

                // the client that owns the connection and can be used to create senders and receivers
                 ServiceBusClient client;

                // the sender used to publish messages to the queue
                 ServiceBusSender sender;

                // number of messages to be sent to the queue
                int numOfMessages = 1;

                // The Service Bus client types are safe to cache and use as a singleton for the lifetime
                // of the application, which is best practice when messages are being published or read
                // regularly.
                //
                // Create the clients that we'll use for sending and processing messages.
                client = new ServiceBusClient(connectionString);
                sender = client.CreateSender(queueName);

                // create a batch 
                using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

                string body = JsonConvert.SerializeObject(userData);
                for (int i = 1; i <= numOfMessages; i++)
                {
                    // try adding a message to the batch
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage(body)))
                    {
                        // if it is too large for the batch
                        throw new Exception($"The message {i} is too large to fit in the batch.");
                    }
                }

                try
                {
                    // Use the producer client to send the batch of messages to the Service Bus queue
                    await sender.SendMessagesAsync(messageBatch);
                    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
                }
                finally
                {
                    // Calling DisposeAsync on client types is required to ensure that network
                    // resources and other unmanaged objects are properly cleaned up.
                    await sender.DisposeAsync();
                    await client.DisposeAsync();
                }

               // isSuccess = true;
            }
            catch (Exception ex)
            {

            }

            //return isSuccess;


        }

    
    }
}
