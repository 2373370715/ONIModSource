﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Database {
    [DebuggerDisplay("{Id}")]
    public class SpaceDestinationType : Resource {
        public const float                                  MASS_TO_RECOVER = 1000f;
        public       ArtifactDropRate                       artifactDropTable;
        public       int                                    cyclesToRecover;
        public       string                                 description;
        public       Dictionary<SimHashes, MathUtil.MinMax> elementTable;
        public       int                                    iconSize = 128;
        public       Dictionary<string, int>                recoverableEntities;
        public       string                                 spriteName;
        public       string                                 typeName;
        public       bool                                   visitable;

        public SpaceDestinationType(string                                 id,
                                    ResourceSet                            parent,
                                    string                                 name,
                                    string                                 description,
                                    int                                    iconSize,
                                    string                                 spriteName,
                                    Dictionary<SimHashes, MathUtil.MinMax> elementTable,
                                    Dictionary<string, int>                recoverableEntities = null,
                                    ArtifactDropRate                       artifactDropRate = null,
                                    int                                    max = 64000000,
                                    int                                    min = 63994000,
                                    int                                    cycles = 6,
                                    bool                                   visitable = true) : base(id, parent, name) {
            typeName                 = name;
            this.description         = description;
            this.iconSize            = iconSize;
            this.spriteName          = spriteName;
            this.elementTable        = elementTable;
            this.recoverableEntities = recoverableEntities;
            artifactDropTable        = artifactDropRate;
            maxiumMass               = max;
            minimumMass              = min;
            cyclesToRecover          = cycles;
            this.visitable           = visitable;
        }

        public int   maxiumMass              { get; private set; }
        public int   minimumMass             { get; private set; }
        public float replishmentPerCycle     => 1000f / cyclesToRecover;
        public float replishmentPerSim1000ms => 1000f / (cyclesToRecover * 600f);
    }
}