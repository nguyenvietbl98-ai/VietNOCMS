namespace VietNOCMS.Models
{
   
        public class WalletViewModel
        {
            public decimal CurrentBalance { get; set; }
            public List<Wallet> Transactions { get; set; } = new();
            public decimal TotalDepositThisMonth { get; set; }
            public decimal TotalWithdrawThisMonth { get; set; }
        }
    }

