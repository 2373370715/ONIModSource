using Klei.AI;

namespace Database {
    public class Sicknesses : ResourceSet<Sickness> {
        public Sickness Allergies;
        public Sickness FoodSickness;
        public Sickness RadiationSickness;
        public Sickness SlimeSickness;
        public Sickness Sunburn;
        public Sickness ZombieSickness;

        public Sicknesses(ResourceSet parent) : base("Sicknesses", parent) {
            FoodSickness   = Add(new FoodSickness());
            SlimeSickness  = Add(new SlimeSickness());
            ZombieSickness = Add(new ZombieSickness());
            if (DlcManager.FeatureRadiationEnabled()) RadiationSickness = Add(new RadiationSickness());
            Allergies = Add(new Allergies());
            Sunburn   = Add(new Sunburn());
        }

        public static bool IsValidID(string id) {
            var result = false;
            using (var enumerator = Db.Get().Sicknesses.resources.GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current.Id == id)
                        result = true;
            }

            return result;
        }
    }
}