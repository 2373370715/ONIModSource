using System.Collections.Generic;

public class ExposureType {
    public int          base_resistance;
    public List<string> excluded_effects;
    public List<string> excluded_traits;
    public int          exposure_threshold;
    public string       germ_id;
    public bool         infect_immediately;
    public string       infection_effect;
    public List<string> required_traits;
    public string       sickness_id;
}