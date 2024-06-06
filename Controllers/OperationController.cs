using MCDM.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCDM.Controllers
{
    
    public class OperationController : Controller
    {
        private List<Group> groups = new List<Group>();
        private List<Criterion> criterions = new List<Criterion>();

        public void AddGroup(Group group){
            //  yeni bir grup eklendiğinde mevcuttaki tüm kriterler o gruba set edilmeli Double değeri default da 0 
            foreach (var item in criterions)
            {
                group.CriterionValue.Add(item, 0);
            }
            this.groups.Add(group);
        }

        //Bir kriter eklendiğinde mevcuttaki tüm gruplara bu kriter set edilmeli Double değeri 0 olacak
        public void AddCriterion(Criterion criterion){
            foreach(var item in groups)
            {
                item.CriterionValue.Add(criterion, 0);
            }
            this.criterions.Add(criterion);
        }

        public Group GetIdealSolution()
        {
            double maxValue = 0;
            Group ideaGroup = this.groups[0];
            foreach (var item in this.groups)
            {
                double PositiveNorm = this.GroupPositiveNormalization(item);
                double NegativeNorm = this.GroupNegativeNormalization(item);
                double CiValue = NegativeNorm / (PositiveNorm + NegativeNorm);
                if(CiValue >= maxValue){
                    maxValue = CiValue;
                    ideaGroup = item;
                }
            }

            return ideaGroup;
        }

        public double GroupNormalization(Group group, Func<Criterion, double> extremeFunc)
        {
            double total = 0;
            foreach (var item in group.CriterionValue)
            {
                double DT = GetDTWithCriterion(item.Key);
                double extremeI = extremeFunc(item.Key);
                double value = ((item.Value / DT) * item.Key.PercentageValue) - extremeI;
                total += value * value;
            }
            return Math.Sqrt(total);
        }

        public double GroupPositiveNormalization(Group group)
        {
            return GroupNormalization(group, GetPositiveI);
        }

        public double GroupNegativeNormalization(Group group)
        {
            return GroupNormalization(group, GetNegativeI);
        }
        
        public double GetExtremeI(Criterion criterion, Func<IEnumerable<double>, double> extremeFunc)
        {
            double DT = this.GetDTWithCriterion(criterion);
            List<double> list = new List<double>();

            foreach (var item in groups)
            {
                list.Add((item.CriterionValue[criterion] / DT) * criterion.PercentageValue);
            }

            return extremeFunc(list);
        }

        public double GetPositiveI(Criterion criterion)
        {
            return GetExtremeI(criterion, Enumerable.Max);
        }

        public double GetNegativeI(Criterion criterion)
        {
            return GetExtremeI(criterion, Enumerable.Min);
        }

        public double GetDTWithCriterion(Criterion criterion){
            double total = 0;
            foreach(var item in groups)
            {
                total += item.CriterionValue[criterion];
            }
            return Math.Sqrt(total);
        }
    }
}