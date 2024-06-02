using System.Collections;

namespace MCDM.Models
{
    public class Mcdm
    {
        private int Id;
        private bool type;
        private double percentageValue;
        private string name;
        private List<double> value = new List<double>();
        private List<double> negative = new List<double>();
        private List<double> positive = new List<double>();

        public Mcdm() { }

        public int GetId()
        {
            return this.Id;
        }

        public void SetId(int Id)
        {
            this.Id = Id;
        }

        public bool IsType()
        {
            return this.type;
        }

        public bool GetType()
        {
            return this.type;
        }

        public void SetType(bool type)
        {
            this.type = type;
        }

        public double GetPercentageValue()
        {
            return this.percentageValue;
        }

        public void SetPercentageValue(double percentageValue)
        {
            this.percentageValue = percentageValue;
        }

        public string GetName()
        {
            return this.name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public List<double> GetValue()
        {
            return this.value;
        }

        public void SetValue(List<double> value)
        {
            this.value = value;
        }

        public List<double> GetPowValue()
        {
            return this.value.Select(x => x * x).ToList();
        }

        public double CalculateSquareRootOfSum()
        {
            return Math.Sqrt(this.GetPowValue().Sum());
        }

        public List<double> CalculatePercentage()
        {
            return this.value.Select(x =>
                (x / this.CalculateSquareRootOfSum()) * this.percentageValue
            ).ToList();
        }

        public double? GetPositiveIdealSolutionTypeValue()
        {
            var percentages = this.CalculatePercentage();
            if (this.type)
            {
                return percentages.Max();
            }
            else
            {
                return percentages.Min();
            }
        }

        public double? GetNegativeIdealSolutionTypeValue()
        {
            var percentages = this.CalculatePercentage();
            if (!this.type)
            {
                return percentages.Max();
            }
            else
            {
                return percentages.Min();
            }
        }

        public void GetPositiveIdealSolution()
        {
            var positiveValue = this.GetPositiveIdealSolutionTypeValue();
            if (positiveValue == null)
            {
                throw new InvalidOperationException("Positive ideal solution type value is not present");
            }

            this.positive.AddRange(this.CalculatePercentage().Select(x =>
                Math.Pow(x - positiveValue.Value, 2)
            ).ToList());
        }

        public void GetNegativeIdealSolution()
        {
            var negativeValue = this.GetNegativeIdealSolutionTypeValue();
            if (negativeValue == null)
            {
                throw new InvalidOperationException("Negative ideal solution type value is not present");
            }

            this.negative.AddRange(this.CalculatePercentage().Select(x =>
                Math.Pow(x - negativeValue.Value, 2)
            ).ToList());
        }

        public List<double> GetNegative()
        {
            if (!this.negative.Any())
            {
                this.GetNegativeIdealSolution();
            }
            return this.negative;
        }

        public void SetNegative(List<double> negative)
        {
            this.negative = negative;
        }

        public List<double> GetPositive()
        {
            if (!this.positive.Any())
            {
                this.GetPositiveIdealSolution();
            }
            return this.positive;
        }

        public void SetPositive(List<double> positive)
        {
            this.positive = positive;
        }
    }
}
