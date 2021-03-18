using PortableCryptoLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HandyCrypto.Model
{
    public class WalletModel : CryptoItemModel
    {
        public WalletModel(Wallet wallet,decimal percentAllTime,decimal currentBudget, ObservableCollection<HistoricalDataModel>historicalData = null)
        {
            Wallet = wallet;
            PercentAllTime = percentAllTime;
            CurrentBudget = currentBudget;
            HistoricalData = historicalData;
        }
        public WalletModel()
        {
            
        }
        
        public decimal CurrentBudget { get; set; }
        public decimal PercentAllTime { get; set; }
        public decimal PercentHour { get; set; }

        public Wallet Wallet { get; set; }
        public ObservableCollection<HistoricalDataModel> HistoricalData { get; set; }
    }
}