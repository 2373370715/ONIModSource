using System.Collections.Generic;

namespace Database {
    public class ArtifactDropRate : Resource {
        public List<Tuple<ArtifactTier, float>> rates = new List<Tuple<ArtifactTier, float>>();
        public float                            totalWeight;

        public void AddItem(ArtifactTier tier, float weight) {
            rates.Add(new Tuple<ArtifactTier, float>(tier, weight));
            totalWeight += weight;
        }

        public float GetTierWeight(ArtifactTier tier) {
            var result = 0f;
            foreach (var tuple in rates)
                if (tuple.first == tier)
                    result = tuple.second;

            return result;
        }
    }
}