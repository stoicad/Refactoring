namespace Statements.Models
{
    public class PerformanceDetails
    {
        public PlayTypeEnum PlayTypeEnum { get; set; }
        public string PlayName { get; set; }
        public double Amount { get; set; }
        public double Credits { get; set; }
        public int Audience { get; set; }
    }
}