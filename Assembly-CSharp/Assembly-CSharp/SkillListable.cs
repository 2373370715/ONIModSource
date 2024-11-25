public class SkillListable : IListableOption {
    public LocString name;

    public SkillListable(string name) {
        skillName = name;
        var skill = Db.Get().Skills.TryGet(skillName);
        if (skill != null) {
            this.name = skill.Name;
            skillHat  = skill.hat;
        }
    }

    public string skillName       { get; }
    public string skillHat        { get; private set; }
    public string GetProperName() { return name; }
}