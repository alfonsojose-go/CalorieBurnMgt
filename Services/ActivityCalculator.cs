using System;

namespace CalorieBurnMgt.Services
{
    /// Handles all calculations related to calories, distance, and activities.
    public class ActivityCalculator
    {
        private const double CaloriesPerKm = 80.0;

        /// Convert calories eaten into required walking distance (km).
        public double CaloriesToWalkingKm(int calories)
        {
            return calories / CaloriesPerKm;
        }

        /// Convert an activity distance (walking/jogging/cycling)
        public double ConvertToWalkingEquivalent(double km, string activity)
        {
            if (string.IsNullOrWhiteSpace(activity))
                return km;

            activity = activity.Trim().ToLower();

            return activity switch
            {
                "jogging" => km * 2,   // jogging twice as effective
                "cycling" => km * 4,   // cycling four times as effective
                _ => km                // default: walking
            };
        }

        /// Convert walking-equivalent km into calories burned,
        public int WalkingKmToCalories(double walkingKm)
        {
            return (int)(walkingKm * CaloriesPerKm);
        }
    }
}
