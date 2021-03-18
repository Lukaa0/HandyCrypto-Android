using SQLite.Net.Attributes;

namespace HandyCrypto.Model
{
    public class User
    {
        private int id;

        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => id = value; }
        public string Username { get; set; }

        public string AvatarId { get; set; } = "man";


        public User(string username, string avatarID = "man")
        {
            Username = username;
            AvatarId = avatarID;
        }
        public User() { }
    }
}