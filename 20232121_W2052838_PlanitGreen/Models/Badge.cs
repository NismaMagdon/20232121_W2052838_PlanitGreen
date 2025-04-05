using System.ComponentModel.DataAnnotations;

namespace _20232121_W2052838_PlanitGreen.Models
{
    public class Badge
    {
        [Key]
        public int BadgeID { get; set; }
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeCategory { get; set; }
        public string CriteriaType { get; set; }
        public int ThresholdValue { get; set; }
        public string BadgeImage { get; set; }
        public int BonusEcoPoints { get; set; }
    }
}
