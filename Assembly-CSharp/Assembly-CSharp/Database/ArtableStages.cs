using System.Collections.Generic;

namespace Database {
    public class ArtableStages : ResourceSet<ArtableStage> {
        public ArtableStages(ResourceSet parent) : base("ArtableStages", parent) {
            foreach (var artableInfo in Blueprints.Get().all.artables)
                Add(artableInfo.id,
                    artableInfo.name,
                    artableInfo.desc,
                    artableInfo.rarity,
                    artableInfo.animFile,
                    artableInfo.anim,
                    artableInfo.decor_value,
                    artableInfo.cheer_on_complete,
                    artableInfo.status_id,
                    artableInfo.prefabId,
                    artableInfo.symbolname,
                    artableInfo.dlcIds);
        }

        public ArtableStage Add(string       id,
                                string       name,
                                string       desc,
                                PermitRarity rarity,
                                string       animFile,
                                string       anim,
                                int          decor_value,
                                bool         cheer_on_complete,
                                string       status_id,
                                string       prefabId,
                                string       symbolname,
                                string[]     dlcIds) {
            var status_item = Db.Get().ArtableStatuses.Get(status_id);
            var artableStage = new ArtableStage(id,
                                                name,
                                                desc,
                                                rarity,
                                                animFile,
                                                anim,
                                                decor_value,
                                                cheer_on_complete,
                                                status_item,
                                                prefabId,
                                                symbolname,
                                                dlcIds);

            resources.Add(artableStage);
            return artableStage;
        }

        public List<ArtableStage> GetPrefabStages(Tag prefab_id) {
            return resources.FindAll(stage => stage.prefabId == prefab_id);
        }

        public ArtableStage DefaultPrefabStage(Tag prefab_id) {
            return GetPrefabStages(prefab_id)
                .Find(stage => stage.statusItem == Db.Get().ArtableStatuses.AwaitingArting);
        }
    }
}