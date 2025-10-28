using BakeryBI.Data;


namespace BakeryBI.Utils
{
    internal class DataPoint
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public bool IsForecast { get; set; } = false;
    }
}
