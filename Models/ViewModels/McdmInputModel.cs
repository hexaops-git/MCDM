namespace MCDM.Models.ViewModels
{
    public class McdmInputModel
    {
        public List<CriterionModel> Criterion { get; set; }

        public class CriterionModel
        {
            public string Name { get; set; }
            public bool Type { get; set; }
            public double PercentageValue { get; set; }
            public List<double> Values { get; set; }
        }
    }
}
