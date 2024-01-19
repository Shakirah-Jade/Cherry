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
    public class HealthCalculatorModuleTests
    {
        [TestMethod()]
        public void CalculateBMRTest()
        {
            // Arrange
            var healthCalculatorModule = new HealthProTechLibrary.HealthCalculatorModule();

            // Act
            double bmrMale = healthCalculatorModule.CalculateBMR(70, 170, 25, HealthProTechLibrary.Gender.Male);
            double bmrFemale = healthCalculatorModule.CalculateBMR(60, 160, 30, HealthProTechLibrary.Gender.Female);

            // Assert
            Assert.AreEqual(1699.557, bmrMale, 0.7, "Incorrect BMR for Male");
            Assert.AreEqual(1367.693, bmrFemale, 0.7, "Incorrect BMR for Female");
        }

        [TestMethod()]
        public void CalculateTDEETest()
        {
            // Arrange
            var healthCalculatorModule = new HealthProTechLibrary.HealthCalculatorModule();
            healthCalculatorModule.CalculateBMR(70, 170, 25, HealthProTechLibrary.Gender.Male);

            // Act
            double tdeeSedentary = healthCalculatorModule.CalculateTDEE(HealthProTechLibrary.ActivityLevel.Sedentary);
            double tdeeActive = healthCalculatorModule.CalculateTDEE(HealthProTechLibrary.ActivityLevel.ExtremelyActive);

            // Assert
            Assert.AreEqual(2039.5683, tdeeSedentary, 0.7, "Incorrect TDEE for Sedentary activity level");
            Assert.AreEqual(3229.6083, tdeeActive, 0.7, "Incorrect TDEE for ExtremelyActive activity level");
        }

        [TestMethod()]
        public void CalculateDailyCaloricIntakeTest()
        {
            // Arrange
            var healthCalculatorModule = new HealthProTechLibrary.HealthCalculatorModule();
            healthCalculatorModule.CalculateBMR(70, 170, 25, HealthProTechLibrary.Gender.Male);
            healthCalculatorModule.CalculateTDEE(HealthProTechLibrary.ActivityLevel.ModeratelyActive);

            // Act
            double dailyCaloricIntakeLoss = healthCalculatorModule.CalculateDailyCaloricIntake(HealthProTechLibrary.UserGoal.WeightLoss);
            double dailyCaloricIntakeGain = healthCalculatorModule.CalculateDailyCaloricIntake(HealthProTechLibrary.UserGoal.WeightGain);

            // Assert
            Assert.AreEqual(634.58835, dailyCaloricIntakeLoss, 0.7, "Incorrect Daily Caloric Intake for Weight Loss goal");
            Assert.AreEqual(3634.58835, dailyCaloricIntakeGain, 0.7, "Incorrect Daily Caloric Intake for Weight Gain goal");
        }

    }
}