using STRINGS;
using EQUIPMENT = TUNING.EQUIPMENT;

namespace Database {
    public class AssignableSlots : ResourceSet<AssignableSlot> {
        public AssignableSlot Bed;
        public AssignableSlot BionicUpgrade;
        public AssignableSlot Clinic;
        public AssignableSlot GeneShuffler;
        public AssignableSlot HabitatModule;
        public AssignableSlot MassageTable;
        public AssignableSlot MedicalBed;
        public AssignableSlot MessStation;
        public AssignableSlot Outfit;
        public AssignableSlot ResetSkillsStation;
        public AssignableSlot RocketCommandModule;
        public AssignableSlot Suit;
        public AssignableSlot Toilet;
        public AssignableSlot Tool;
        public AssignableSlot Toy;
        public AssignableSlot WarpPortal;

        public AssignableSlots() {
            Bed = Add(new OwnableSlot("Bed", MISC.TAGS.BED));
            MessStation = Add(new OwnableSlot("MessStation", MISC.TAGS.MESSSTATION));
            Clinic = Add(new OwnableSlot("Clinic", MISC.TAGS.CLINIC));
            MedicalBed = Add(new OwnableSlot("MedicalBed", MISC.TAGS.CLINIC));
            MedicalBed.showInUI = false;
            GeneShuffler = Add(new OwnableSlot("GeneShuffler", MISC.TAGS.GENE_SHUFFLER));
            GeneShuffler.showInUI = false;
            Toilet = Add(new OwnableSlot("Toilet", MISC.TAGS.TOILET));
            MassageTable = Add(new OwnableSlot("MassageTable", MISC.TAGS.MASSAGE_TABLE));
            RocketCommandModule = Add(new OwnableSlot("RocketCommandModule", MISC.TAGS.COMMAND_MODULE));
            HabitatModule = Add(new OwnableSlot("HabitatModule", MISC.TAGS.HABITAT_MODULE));
            ResetSkillsStation = Add(new OwnableSlot("ResetSkillsStation", "ResetSkillsStation"));
            WarpPortal = Add(new OwnableSlot("WarpPortal", MISC.TAGS.WARP_PORTAL));
            WarpPortal.showInUI = false;
            BionicUpgrade = Add(new OwnableSlot("BionicUpgrade", MISC.TAGS.BIONIC_UPGRADE));
            Toy = Add(new EquipmentSlot(EQUIPMENT.TOYS.SLOT, MISC.TAGS.TOY, false));
            Suit = Add(new EquipmentSlot(EQUIPMENT.SUITS.SLOT, MISC.TAGS.SUIT));
            Tool = Add(new EquipmentSlot(EQUIPMENT.TOOLS.TOOLSLOT, MISC.TAGS.MULTITOOL, false));
            Outfit = Add(new EquipmentSlot(EQUIPMENT.CLOTHING.SLOT, UI.StripLinkFormatting(MISC.TAGS.CLOTHES)));
        }
    }
}