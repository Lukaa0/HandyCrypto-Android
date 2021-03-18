using SQLite;
using TableMapping = SQLite.Net.TableMapping;

namespace HandyCrypto.Model
{
    public class Wallet
    {

        [PrimaryKey, NotNull,AutoIncrement,Unique]
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Investment { get; set; }
        public string InvestDate { get; set; }
        
        public decimal CoinPrice { get; set; }




        public Wallet(string symbol, decimal investment,string invDate)
        {
            Symbol= symbol;
            Investment = investment;
            InvestDate = invDate;
        }
        public Wallet()
        {

        }

       
    }

}