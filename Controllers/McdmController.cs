using MCDM.Models;
using MCDM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace MCDM.Controllers
{
    public class McdmController : Controller
    {

        private List<Mcdm> MCDMModels;
        private List<double> siNegative = new List<double>();
        private List<double> siPositive = new List<double>();
        public McdmController()
        {
        }

        [HttpGet]
        [Route("mcdm/positive-ideal-solution")]
        public IActionResult CalculatePositiveIdealSolution()
        {
            return View();
        }

        [HttpPost]
        [Route("mcdm/positive-ideal-solution")]
        public IActionResult CalculatePositiveIdealSolution(McdmInputModel model)
        {
            MCDMModels = new List<Mcdm>();

            // TODO: model null ise işlem yapma. Daha sonra hata yönetimi eklendiğinde düzenlenmeli.
            if(model.Criterion == null || model == null )
            {
                return View(model);
            }
            foreach (var kriter in model.Criterion)
            {
                Mcdm mcdmModel = new Mcdm();
                mcdmModel.SetType(kriter.Type);
                mcdmModel.SetPercentageValue(kriter.PercentageValue);
                mcdmModel.SetName(kriter.Name);
                mcdmModel.SetValue(kriter.Values);
                MCDMModels.Add(mcdmModel);
            }

            this.CalculateSiNegative();
            this.CalculateSiPositive();
            List<double> result = new List<double>();

            for (int i = 0; i < this.MCDMModels[0].GetValue().Count; i++)
            {
                result.Add(this.siNegative[i] / (this.siPositive[i] + this.siNegative[i]));
            }


            // TODO: Sonucu ön yüzde görmek için geçici olarak ViewBag ile taşıdık. Daha sonra veritabanı dahil oldugunda HttpGet Metodunda getirilmeli.
            ViewBag.Result = result.Select((value, index) => new { Value = value, Index = index })
                                   .Aggregate((a, b) => a.Value > b.Value ? a : b).Index;
            ViewBag.PositiveScore = string.Join(", ", result);
            //
            return View("CalculatePositiveIdealSolution");
        }

        public void CalculateSiPositive()
        {
            this.siPositive.AddRange(this.GetPositiveSumValues().Select(x => Math.Sqrt(x)).ToList());
        }

        public void CalculateSiNegative()
        {
            this.siNegative.AddRange(this.GetNegativeSumValues().Select(x => Math.Sqrt(x)).ToList());
        }

        public List<double> GetPositiveSumValues()
        {
            List<double> result = new List<double>(new double[this.MCDMModels[0].GetPositive().Count]);

            foreach (Mcdm m in this.MCDMModels)
            {
                for (int i = 0; i < m.GetPositive().Count; i++)
                {
                    result[i] += m.GetPositive()[i];
                }
            }

            return result;
        }

        public List<double> GetNegativeSumValues()
        {
            List<double> result = new List<double>(new double[this.MCDMModels[0].GetNegative().Count]);

            foreach (Mcdm m in this.MCDMModels)
            {
                for (int i = 0; i < m.GetNegative().Count; i++)
                {
                    result[i] += m.GetNegative()[i];
                }
            }

            return result;
        }

        public List<Mcdm> GetMCDMModels()
        {
            if (this.MCDMModels == null)
            {
                this.MCDMModels = new List<Mcdm>();
            }
            return this.MCDMModels;
        }

        public void SetMCDMModels(List<Mcdm> MCDMModels)
        {
            this.MCDMModels = MCDMModels;
        }
    }
}
