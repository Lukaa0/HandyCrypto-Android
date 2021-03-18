using System;
namespace HandyCrypto.Model
{
    internal class MessageContent 
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public string Avatar { get; set; }



        public MessageContent() { }
        public MessageContent(string username, string message,string avatar)
        {
            this.Username = username;
            this.Message = message;
            this.Avatar = avatar;
            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}