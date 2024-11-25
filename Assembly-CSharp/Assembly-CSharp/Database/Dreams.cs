namespace Database {
    public class Dreams : ResourceSet<Dream> {
        public Dream CommonDream;

        public Dreams(ResourceSet parent) : base("Dreams", parent) {
            CommonDream = new Dream("CommonDream", this, "dream_tear_swirly_kanim", new[] { "dreamIcon_journal" });
        }
    }
}