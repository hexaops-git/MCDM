namespace MCDM.Models
{
    public class Group{
        private int id;
        private string name;
        private Dictionary<Criterion, Double> criterionValue = new Dictionary<Criterion, double>();

        public Group(){ }

        public string Name { get => name; set => name = value; }
        public Dictionary<Criterion, double> CriterionValue { get => criterionValue; set => criterionValue = value; }
        public int Id { get => id; set => id = value; }
    }

}