using Klei.AI;

namespace Database {
    public class BuildingAttributes : ResourceSet<Attribute> {
        public Attribute Comfort;
        public Attribute Decor;
        public Attribute DecorRadius;
        public Attribute FatalTemperature;
        public Attribute Hygiene;
        public Attribute NoisePollution;
        public Attribute NoisePollutionRadius;
        public Attribute OverheatTemperature;

        public BuildingAttributes(ResourceSet parent) : base("BuildingAttributes", parent) {
            Decor                = Add(new Attribute("Decor",                true, Attribute.Display.General, false));
            DecorRadius          = Add(new Attribute("DecorRadius",          true, Attribute.Display.General, false));
            NoisePollution       = Add(new Attribute("NoisePollution",       true, Attribute.Display.General, false));
            NoisePollutionRadius = Add(new Attribute("NoisePollutionRadius", true, Attribute.Display.General, false));
            Hygiene              = Add(new Attribute("Hygiene",              true, Attribute.Display.General, false));
            Comfort              = Add(new Attribute("Comfort",              true, Attribute.Display.General, false));
            OverheatTemperature  = Add(new Attribute("OverheatTemperature",  true, Attribute.Display.General, false));
            OverheatTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature,
                                                                            GameUtil.TimeSlice.ModifyOnly));

            FatalTemperature = Add(new Attribute("FatalTemperature", true, Attribute.Display.General, false));
            FatalTemperature.SetFormatter(new StandardAttributeFormatter(GameUtil.UnitClass.Temperature,
                                                                         GameUtil.TimeSlice.ModifyOnly));
        }
    }
}