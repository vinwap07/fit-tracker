using Domain.ValueObjects;

namespace Domain.Services;

public static class CalorieCalculationService
{
    private const double TimeCoefficient = 0.0025;
    private const double ExerciseCoefficient = 0.003;
        
    public static double CalculateExerciseCalories(double met, ExerciseVolume exerciseVolume,
        UserProfile userProfile)
    {
        return exerciseVolume.Repetitions * 
               (met * userProfile.Weight.Kilograms * TimeCoefficient + 
                exerciseVolume.Weight.Kilograms * userProfile.Height.Centimeters / 100 * ExerciseCoefficient);
    }
}