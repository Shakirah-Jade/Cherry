using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.Statistics;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Diagnostics;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
/*using OpenCvSharp;
using OpenCvSharp.Dnn;*/
using System.Globalization;



namespace Cherry
{
    /*public class BMRCalculator
    {
        // method for the BMR
        public static double CalculateBMR(string gender, int age, double weightKg, double heightCm)
        {
            double bmr = 0;

            if (gender.ToLower() == "male")
            {
                // Harris-Benedict equation for men
                bmr = 88.362 + (13.397 * weightKg) + (4.799 * heightCm) - (5.677 * age);
                Console.WriteLine($"BMR for a {gender} with age {age}, weight {weightKg} kg," +
               $" and height {heightCm} cm is: {bmr} calories per day");
                Console.WriteLine("==================================================");
                Console.WriteLine("If your goal is weight loss, you would need to create a ");
                Console.WriteLine("calorie deficit by consuming fewer");
                Console.WriteLine("calories or increasing physical activity...");
                Console.WriteLine("If your goal is muscle gain, you may need to consume more calories..");

                Console.WriteLine("==================================================");

            }
            else if (gender.ToLower() == "female")
            {
                // Harris-Benedict equation for women
                bmr = 447.593 + (9.247 * weightKg) + (3.098 * heightCm) - (4.330 * age);
                Console.WriteLine($"BMR for a {gender} with age {age}, weight {weightKg} kg," +
                                        $" and height {heightCm} cm is: {bmr} calories per day");
                Console.WriteLine("==================================================");
                Console.WriteLine("If your goal is weight loss, you would need to create a ");
                Console.WriteLine("calorie deficit by consuming fewer");
                Console.WriteLine("calories or increasing physical activity...");
                Console.WriteLine("If your goal is muscle gain, you may need to consume more calories..");

                Console.WriteLine("==================================================");


            }
            else
            {
                throw new ArgumentException("Invalid gender. Please use 'male' or 'female'.", nameof(gender));
            }

            return bmr;
        }
        // Daily caloric intake
        public static double CalculateDailyCaloricIntake(double bmr, string activityLevel)
        {
            double dailycaloricintake = 0;
            Console.WriteLine("Daily Caloric Intake based on activity level:");

            switch (activityLevel.ToLower())
            {
                case "sedentary":
                    dailycaloricintake = bmr * 1.2;
                    Console.WriteLine("Sedentary: " + dailycaloricintake);
                    break;
                case "moderately active":
                    dailycaloricintake = bmr * 1.55;
                    Console.WriteLine("Moderately Active: " + dailycaloricintake);
                    break;
                case "active":
                    dailycaloricintake = bmr * 1.9;
                    Console.WriteLine("Active: " + dailycaloricintake);
                    break;
                default:
                    Console.WriteLine("Invalid activity level. Please use 'sedentary', 'moderately active', or 'active'.");
                    break;
            }
            return dailycaloricintake;
        }

        // Total daily expenditure
        public static double CalculateTotalDailyExpenditure(double bmr, double dailycaloricintake)
        {

            double totalDailyExpenditure = bmr + dailycaloricintake;

            Console.WriteLine("Total Daily Expenditure: " + totalDailyExpenditure);

            return totalDailyExpenditure;
        }
        // Glucosedepletion. Glucosedepletion= glucoseUtilizationPercentage * Total daily expenditure
        public static double CalculateGlucoseDepletion(double totalDailyExpenditure)
        {
            double glucoseUtilizationPercentage = 0.5;
            double glucoseDepletion = totalDailyExpenditure * glucoseUtilizationPercentage;

            Console.WriteLine("Estimated Glucose Depletion: " + glucoseDepletion);

            return glucoseDepletion;
        }
        // BodyIntakeRate
        public static double CalculateBodyIntakeRate(double glucoseDepletion)
        {
            double glucoseUtilizationPercentage = 0.5;
            double bodyIntakeRate = (glucoseDepletion - glucoseUtilizationPercentage) / glucoseUtilizationPercentage;

            Console.WriteLine("Body Intake Rate: " + bodyIntakeRate);

            return bodyIntakeRate;
        }
        // FoodGlucoseComposition
        public static double CalculateFoodGlucoseComposition(double currentGlucose, double bodyIntakeRate)
        {
            double normalGlucoseLevel = 70;

            double glucoseComposition = (normalGlucoseLevel - currentGlucose) / bodyIntakeRate;

            Console.WriteLine("Food Glucose Composition: " + glucoseComposition);

            return glucoseComposition;
        }
    }*/

    public class HealthProTechLibrary
    {

        public class HealthCalculatorModule
        {
            // The directory where individual user glucose data files will be stored
            private string glucoseDataDirectory = "GlucoseData";

            // Dictionary to store user-specific glucose data
            private Dictionary<string, List<double>> userGlucoseData = new Dictionary<string, List<double>>();

            // Properties for user-specific parameters
            private double BMR { get; set; }
            private double TDEE { get; set; }

            // Constructor
            public HealthCalculatorModule()
            {
                // Initialize or retrieve user-specific parameters here
                // For simplicity, we'll use default values
                BMR = 0;
                TDEE = 0;
            }

            // Method to calculate BMR (Basal Metabolic Rate)
            public double CalculateBMR(double weight, double height, int age, Gender gender)
            {
                // Use the Harris-Benedict equation for BMR calculation
                if (gender == Gender.Male)
                {
                    BMR = 88.362 + (13.397 * weight) + (4.799 * height) - (5.677 * age);
                }
                else if (gender == Gender.Female)
                {
                    BMR = 447.593 + (9.247 * weight) + (3.098 * height) - (4.330 * age);
                }

                return BMR;
            }

            // Method to calculate TDEE (Total Daily Energy Expenditure)
            public double CalculateTDEE(ActivityLevel activityLevel)
            {
                // Use the Mifflin-St Jeor equation to calculate TDEE based on activity level
                switch (activityLevel)
                {
                    case ActivityLevel.Sedentary:
                        TDEE = BMR * 1.2;
                        break;
                    case ActivityLevel.LightlyActive:
                        TDEE = BMR * 1.375;
                        break;
                    case ActivityLevel.ModeratelyActive:
                        TDEE = BMR * 1.55;
                        break;
                    case ActivityLevel.VeryActive:
                        TDEE = BMR * 1.725;
                        break;
                    case ActivityLevel.ExtremelyActive:
                        TDEE = BMR * 1.9;
                        break;
                }

                return TDEE;
            }

            // Method to calculate daily caloric intake based on user goals
            public double CalculateDailyCaloricIntake(UserGoal userGoal)
            {
                // Adjust TDEE based on user's goal
                switch (userGoal)
                {
                    case UserGoal.WeightLoss:
                        return TDEE - 2000; // Create a calorie deficit for weight loss
                    case UserGoal.Maintenance:
                        return TDEE; // Maintain current weight
                    case UserGoal.WeightGain:
                        return TDEE + 1000; // Create a calorie surplus for weight gain
                    default:
                        return TDEE;
                }
            }

            // Method to record user's glucose level
            public void RecordGlucoseLevel(string userId, double glucoseLevel)
            {
                // Validate user ID
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
                }

                // Initialize user's glucose data if not already present
                if (!userGlucoseData.ContainsKey(userId))
                {
                    userGlucoseData[userId] = new List<double>();
                }

                // Add the provided glucose level to the user's data
                userGlucoseData[userId].Add(glucoseLevel);

                try
                {
                    // Save user's glucose data to a file
                    SaveGlucoseDataToFile(userId, userGlucoseData[userId]);
                }
                catch (IOException ex)
                {
                    // Handle and display error if saving fails
                    Console.WriteLine($"Error saving glucose data for user {userId}: {ex.Message}");
                }
            }

            // Method to get stored glucose levels for a user
            public List<double> GetStoredGlucoseLevels(string userId)
            {
                // Validate user ID
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
                }

                // Construct the file path for the user's glucose data
                string filePath = GetFilePathForUser(userId);

                try
                {
                    // Check if the file exists
                    if (File.Exists(filePath))
                    {
                        // Read and return stored glucose levels from the file
                        return File.ReadAllLines(filePath).Select(ParseGlucoseLevel).ToList();
                    }
                }
                catch (IOException ex)
                {
                    // Handle and display error if reading fails
                    Console.WriteLine($"Error reading glucose data for user {userId}: {ex.Message}");
                }

                // Return an empty list if no data available
                return new List<double>();
            }

            // Helper method to parse glucose level from string
            private double ParseGlucoseLevel(string glucoseString)
            {
                if (double.TryParse(glucoseString, out var glucoseLevel))
                {
                    return glucoseLevel;
                }

                throw new FormatException($"Invalid glucose level format: {glucoseString}");
            }

            // Helper method to save user's glucose data to a file
            private void SaveGlucoseDataToFile(string userId, List<double> glucoseData)
            {
                // Construct the file path for the user's glucose data
                string filePath = GetFilePathForUser(userId);

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Write glucose data to the file
                File.WriteAllLines(filePath, glucoseData.Select(glucose => glucose.ToString()));
            }

            // Helper method to construct the file path for user's glucose data
            private string GetFilePathForUser(string userId)
            {
                return Path.Combine(glucoseDataDirectory, $"{userId}_GlucoseData.txt");
            }
        }

        // Enum for gender
        public enum Gender
        {
            Male,
            Female
        }

        // Enum for activity level
        public enum ActivityLevel
        {
            Sedentary,
            LightlyActive,
            ModeratelyActive,
            VeryActive,
            ExtremelyActive
        }

        // Enum for user goal
        public enum UserGoal
        {
            WeightLoss,
            Maintenance,
            WeightGain
        }

        public class ImageProcessingModule
        {
            private VideoCaptureDevice videoSource;

            //creating a folder to keep the images taken
            static string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            public string imageFilePath = $"HADIE/captured_image_{timestamp}.jpg";
            public void StartCamera()
            {
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (videoDevices.Count == 0)
                {
                    Console.WriteLine("No video devices found.");
                    return;
                }

                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();

                Console.WriteLine("Press Enter to capture an image...");
                Console.ReadLine();
                Console.WriteLine("Image captured and saved to: " + imageFilePath);

                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }

            private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
            {
                // Handle new frame from the camera
                Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

                // Save the bitmap to a file
                bitmap.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            public void OpenCapturedImage()
            {
                // Open the captured image using the default image viewer
                Process.Start(imageFilePath);
            }
        }


        // DietaryAnalyzerModule
        public class FoodCompositionData
        {
            public string FoodName { get; set; }
            public string Country { get; set; }
            public double Carbohydrate { get; set; }
            public double Protein { get; set; }
            public double Lipids { get; set; }
            public double Fibre { get; set; }
            public double Energy { get; set; }

            // Method to calculate calories based on macronutrients
            public double CalculateCalories()
            {
                // Calculate calories using the following macronutrient energy values
                // 1 gram of carbohydrate = 4 calories
                // 1 gram of protein = 4 calories
                // 1 gram of lipid (fat) = 9 calories
                // 1 gram of fiber = 2 calories (a common approximation)
                // one calorie (kcal) equaling 4.18 kJ.
                double carbohydrateCalories = Carbohydrate * 4;
                double proteinCalories = Protein * 4;
                double lipidCalories = Lipids * 9;

                // Include a fraction of fiber calories (commonly 2 calories per gram)
                double fiberCalories = Fibre * 2;

                // Calculate total calories
                double totalCalories = carbohydrateCalories + proteinCalories + lipidCalories + fiberCalories;

                return totalCalories;
            }

        }
            public class FoodCompositionDataReader
        {
            
                 public List<FoodCompositionData> ReadFoodCompositionData(string csvFilePath)
             {
                    using (var reader = new StreamReader(csvFilePath))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                var records = csv.GetRecords<FoodCompositionData>().ToList();
                return records;
             }
            }
         }
        public class DietaryAnalyzerModule
        {
            private readonly HealthCalculatorModule healthCalculatorModule;

            public DietaryAnalyzerModule(HealthCalculatorModule healthCalculatorModule)
            {
                this.healthCalculatorModule = healthCalculatorModule;
            }

            /*            public void AnalyzeDietaryComposition(string csvFilePath, string userId, UserGoal userGoal)
            {
                double weight = 70.0; // Example weight in kilograms
                double height = 170.0; // Example height in centimeters
                int age = 25; // Example age
                Gender gender = Gender.Male; // Example gender
                ActivityLevel activityLevel = ActivityLevel.ModeratelyActive; // Example activity level
                // Load food composition data from the CSV file
                var foodCompositionData = LoadFoodCompositionData(csvFilePath);

                // Get user health metrics from the Health Calculator Module
                double bmr = healthCalculatorModule.CalculateBMR(weight, height, age, gender);
                double tdee = healthCalculatorModule.CalculateTDEE(activityLevel);
                double dailyCaloricIntake = healthCalculatorModule.CalculateDailyCaloricIntake(userGoal);

                // Perform dietary analysis using food composition data and user health metrics
                // (Replace the following line with your actual dietary analysis logic)
                Console.WriteLine("Dietary analysis result: ");
                Console.WriteLine($"User BMR: {bmr}");
                Console.WriteLine($"User TDEE: {tdee}");
                Console.WriteLine($"User Daily Caloric Intake: {dailyCaloricIntake}");
            }*/

            /*            public void AnalyzeFoodImage(string imagePath)
            {
                // Retrieve user health metrics from the Health Calculator Module
                double dailyCaloricIntake = healthCalculatorModule.CalculateDailyCaloricIntake(UserGoal.WeightLoss);

                // Perform object detection using YOLOv3
                var detectedObjects = PerformObjectDetection(imagePath);

                // Refine dietary recommendations based on detected objects and user health metrics
                RefineDiet(detectedObjects, dailyCaloricIntake);

            }

            public List<string> PerformObjectDetection(string imagePath)
            {
                // Load YOLOv3 model and class names
                var net = CvDnn.ReadNetFromDarknet("C:\\Users\\User\\Desktop\\sem1\\MiniProject\\Fud\\yolov3.cfg", "C:\\Users\\User\\Desktop\\sem1\\MiniProject\\Fud\\yolov3.weights");
                var classNames = System.IO.File.ReadAllLines("C:\\Users\\User\\Desktop\\sem1\\MiniProject\\Fud\\coco.names");

                // Load the image
                var image = Cv2.ImRead(imagePath);

                // Prepare the image for object detection
                var blob = CvDnn.BlobFromImage(image, 1.0 / 255.0, new OpenCvSharp.Size(416, 416), new Scalar(0), true, false);
                net.SetInput(blob);

                // Run forward pass to get detections
                var detections = net.Forward();

                // Process detections
                var detectedObjects = new List<string>();
                for (int i = 0; i < detections.Total(); i += 7)
                {
                    // Ensure that there are enough elements in the detections array
                    if (i + 6 < detections.Total())
                    {
                        float confidence = detections.At<float>(i + 2);

                        if (confidence > 0.5)
                        {
                            int classId = (int)detections.At<float>(i + 1);

                            // Ensure that classId is within the bounds of classNames array
                            if (classId > 0 && classId <= classNames.Length)
                            {
                                string className = classNames[classId - 1];
                                detectedObjects.Add(className);
                            }
                        }
                    }
                }

                return detectedObjects;
            }


            public void RefineDiet(List<string> detectedObjects, double dailyCaloricIntake)
            {
                // Print detected objects
                Console.WriteLine("Detected Objects:");
                foreach (var detectedObject in detectedObjects)
                {
                    Console.WriteLine($"- {detectedObject}");
                }

                // Print daily caloric intake
                Console.WriteLine($"\nDaily Caloric Intake: {dailyCaloricIntake} calories\n");
            }*/
            public void RecommendFoodsBasedOnCaloricIntake(string csvFilePath, string userId, UserGoal userGoal)
            {
                // Get user health metrics from the Health Calculator Module
                double dailyCaloricIntake = healthCalculatorModule.CalculateDailyCaloricIntake(userGoal);

                // Load food composition data from the CSV file
                Console.WriteLine("Loading food composition data...");
                var foodCompositionDataList = LoadFoodCompositionData(csvFilePath);
                Console.WriteLine($"Loaded {foodCompositionDataList.Count()} food items."); ;

                // Recommend foods based on user's caloric intake goal
                Console.WriteLine("Recommending foods...");
                var recommendedFoods = RecommendFoods(dailyCaloricIntake, foodCompositionDataList);
                Console.WriteLine($"Recommended {recommendedFoods.Count()} foods.");
                // Display the recommended foods
                Console.WriteLine($"Recommended foods for user {userId} with a {userGoal} goal and a daily caloric intake of {dailyCaloricIntake} calories:");

                foreach (var food in recommendedFoods)
                {
                    Console.WriteLine($"Food: {food.FoodName}");
                    Console.WriteLine($"Calories per 100g: {food.CalculateCalories()}");
                    Console.WriteLine("------------------------");
                }
            }

            public List<FoodCompositionData> LoadFoodCompositionData(string csvFilePath)
            {
                var dataReader = new FoodCompositionDataReader();
                return dataReader.ReadFoodCompositionData(csvFilePath);
            }
            public List<FoodCompositionData> RecommendFoods(double dailyCaloricIntake, List<FoodCompositionData> foodList)
            {
                // Set a larger threshold temporarily to include a wider range of foods
                double thresholdPercentage = 20; // 20% threshold
                double threshold = dailyCaloricIntake * (thresholdPercentage / 100);

                // Filter foods based on caloric intake goal
                var recommendedFoods = foodList
                    .Where(food => Math.Abs(food.CalculateCalories() - dailyCaloricIntake) <= threshold)
                    .ToList();

                return recommendedFoods;
            }
        }




    }
}
