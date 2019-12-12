namespace SmartPower.Models
{
    public class Loadparameter
    {
        public int Id { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal Power { get; set; }
        public decimal RatingCurrent { get; set; }
        public decimal RatingVoltage { get; set; }
        public decimal RatingTemp { get; set; }
        public string Type { get; set; }
    }
}