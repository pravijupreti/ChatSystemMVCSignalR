using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ChatSystemMVC
{
    public class ChatServices : IChatServices
    {
        public ChatServices()
        {
            if (FirebaseApp.DefaultInstance == default)
            {
                var credential = GoogleCredential.FromFile("E:\\ChatSystemMVC\\ChatSystemMVC\\Firebase_Admin_sdk.json");
                var firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }
        }
        public async Task SendMessage(MessageDto messageDto)
        {
            // Store the message in Firebase Realtime Database
            // You can use the FirebaseAdmin SDK to interact with the database
            // Example code:
            //messageDto.SenderId = "string1";
            //messageDto.Reply = "hello";
            messageDto.DateTime = DateTime.UtcNow.ToString();

            var firebaseClient = new FirebaseClient("https://fir-auth-5f97e-default-rtdb.asia-southeast1.firebasedatabase.app");
            await firebaseClient.Child("messages").PostAsync(messageDto);

            //Notify the client applications about the new message using Firebase Cloud Messaging(optional)
            //Example code:
            var message = new Message
            {
                Topic = "new-message",
                Data = new Dictionary<string, string>
                 {
                     { "messageId", messageDto.Id },
                     { "senderId", messageDto.SenderId },
                     { "content", messageDto.Content }
                 }
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task<IEnumerable<MessageDto>> GetChatMessage(List<string> userIds)
        {
            // Retrieve chat messages from Firebase Realtime Database
            // Example code:
            var firebaseClient = new FirebaseClient("https://fir-auth-5f97e-default-rtdb.asia-southeast1.firebasedatabase.app");
            var MessagesList = new List<MessageDto>();
            foreach (var user in userIds)
            {
                var senderMessages = await firebaseClient
                .Child("messages")
                .OrderBy("SenderId")
                .EqualTo(user)
                .OnceAsync<MessageDto>();

                var recipientMessages = await firebaseClient
                    .Child("messages")
                    .OrderBy("To")
                    .EqualTo(user)
                    .OnceAsync<MessageDto>();
                if (senderMessages.Count != default)
                {
                    MessagesList.AddRange(senderMessages.Select(m => m.Object));
                }
                if (recipientMessages.Count != default)
                {
                    MessagesList.AddRange(recipientMessages.Select(m => m.Object));
                }
            }

            var specificUserMessages = MessagesList.Where(x =>
            (userIds.Contains(x.SenderId) && userIds.Contains(x.To)) ||
            (userIds.Contains(x.To) && userIds.Contains(x.SenderId)))
            .ToList().DistinctBy(x=>x.DateTime);

            return specificUserMessages;
        }

        public async Task<IEnumerable<MessageDto>> GetLatestChatMessage(List<string> userIds)
        {
            // Retrieve chat messages from Firebase Realtime Database
            // Example code:
            var firebaseClient = new FirebaseClient("https://fir-auth-5f97e-default-rtdb.asia-southeast1.firebasedatabase.app");
            var MessagesList = new List<MessageDto>();
            foreach (var user in userIds)
            {
                var snapshot = await firebaseClient.Child("messages")
                .OrderByKey()
                .LimitToLast(1)
                .OnceAsync<MessageDto>();
                MessagesList.AddRange(snapshot.Select(x => x.Object));
            }

            var specificUserMessages = MessagesList.Where(x =>
            (userIds.Contains(x.SenderId) && userIds.Contains(x.To)) ||
            (userIds.Contains(x.To) && userIds.Contains(x.SenderId)))
            .ToList().DistinctBy(x => x.Content);

            return specificUserMessages;
        }


        public string SecretGroupName(string user1, string user2)
        {
            // Generate a unique salt for each hash
            string salt = GenerateRandomString();

            // Combine the inputs and salt
            string combinedInput = user1 + user2 + salt;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(combinedInput);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Generate a random string of a specified length for the salt
        private static string GenerateRandomString()
        {
            var length = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
