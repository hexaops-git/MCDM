
namespace MCDM.Models
{
    public class Criterion{
        private int id;
        private bool type;
        private double percentageValue;
        private string name;

        public Criterion() { }

        public int Id { get => id; set => id = value; }
        public bool Type { get => type; set => type = value; }
        public double PercentageValue { get => percentageValue; set => percentageValue = value; }
        public string Name { get => name; set => name = value; }
    }
}