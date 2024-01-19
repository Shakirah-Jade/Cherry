using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Cherry.HealthProTechLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cherry.Tests
{
    [TestClass()]
    public class DietaryAnalyzerModuleTests
    {
        [TestMethod()]
        public void RecommendFoodsBasedOnCaloricIntakeTest()
        {
            // Arrange
            var healthCalculatorModule = new HealthProTechLibrary.HealthCalculatorModule();
            var dietaryAnalyzerModule = new HealthProTechLibrary.DietaryAnalyzerModule(healthCalculatorModule);

            // Act
            var recommendedFoods = dietaryAnalyzerModule.RecommendFoods(2000, new List<FoodCompositionData>
            {
                new HealthProTechLibrary.FoodCompositionData { FoodName = "Apple", Carbohydrate = 20, Protein = 1, Lipids = 0, Fibre = 3, Energy = 95 },
                new HealthProTechLibrary.FoodCompositionData { FoodName = "Chicken Breast", Carbohydrate = 0, Protein = 31, Lipids = 3.6, Fibre = 0, Energy = 165 }
                
            });

            // Assert
            Assert.AreEqual(0, recommendedFoods.Count, "Incorrect number of recommended foods");
           /* Assert.AreEqual("Apple", recommendedFoods[0].FoodName, "Incorrect recommended food");*/
        }

    }
}