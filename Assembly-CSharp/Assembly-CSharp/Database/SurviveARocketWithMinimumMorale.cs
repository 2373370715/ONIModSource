using STRINGS;

namespace Database {
    public class SurviveARocketWithMinimumMorale : ColonyAchievementRequirement {
        public float minimumMorale;
        public int   numberOfCycles;

        public SurviveARocketWithMinimumMorale(float minimumMorale, int numberOfCycles) {
            this.minimumMorale  = minimumMorale;
            this.numberOfCycles = numberOfCycles;
        }

        public override string GetProgress(bool complete) {
            if (complete)
                return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE_COMPLETE,
                                     minimumMorale,
                                     numberOfCycles);

            return base.GetProgress(complete);
        }

        public override bool Success() {
            foreach (var keyValuePair in SaveGame.Instance.ColonyAchievementTracker
                                                 .cyclesRocketDupeMoraleAboveRequirement)
                if (keyValuePair.Value >= numberOfCycles)
                    return true;

            return false;
        }
    }
}