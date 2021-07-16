namespace CryptoClock.Data.Models
{
    public record LightningModel(decimal ConfirmedBalance, decimal UnconfirmedBalance, decimal LocalBalance, decimal RemoteBalance)
    {
        public decimal TotalBalance => ConfirmedBalance + UnconfirmedBalance;
    }
}
