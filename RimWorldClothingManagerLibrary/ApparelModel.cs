using System.Collections.Generic;
using System.Xml.Serialization;

namespace RimWorldClothingManagerLibrary
{
    [XmlRoot("Defs")]
    public class Defs
    {
        [XmlElement("ThingDef")] public List<ApparelModel> ApparelModels { get; set; }
    }

    [XmlRoot("thingDef")]
    public class ApparelModel
    {
        [XmlAttribute("Name")] public string Name { get; set; }
        [XmlAttribute("ParentName")] public string ParentName { get; set; }
        [XmlAttribute("Abstract")] public string Abstract { get; set; }
        [XmlElement("label")] public string Label { get; set; }
        [XmlElement("description")] public string Description { get; set; }
        [XmlElement("thingCategories")] public List<ThingCategories> ThingCategories { get; set; } = new List<ThingCategories>();
        [XmlElement("apparel")] public Apparel Apparel { get; set; }
        [XmlElement("statBases")] public StatBases StatBases { get; set; }
        [XmlElement("equippedStatOffsets")] public EquippedStatOffsets EquippedStatOffsets { get; set; }
    }

    [XmlRoot("thingCategories")]
    public class ThingCategories
    {
        [XmlElement("li")] public string Li { get; set; }
    }

    public class Apparel
    {
        [XmlElement("bodyPartGroups")] public List<BodyPartGroups> BodyPartGroups { get; set; } = new List<BodyPartGroups>();
        [XmlElement("layers")] public List<Layers> Layers { get; set; } = new List<Layers>();
    }

    public class BodyPartGroups
    {
        [XmlElement("li")] public string Li { get; set; }
    }

    public class Layers
    {
        [XmlElement("li")] public string Li { get; set; }
    }

    [XmlRoot("statBases")]
    public class StatBases
    {
        [XmlElement("MaxHitPoints")] public double MaxHitPoints { get; set; }
        [XmlElement("Flammability")] public double Flammability { get; set; }
        [XmlElement("DeteriorationRate")] public double DeteriorationRate { get; set; }
        [XmlElement("Beauty")] public double Beauty { get; set; }
        [XmlElement("WorkToMake")] public double WorkToMake { get; set; }
        [XmlElement("Mass")] public double Mass { get; set; }
        [XmlElement("StuffEffectMultiplierArmor")] public double StuffEffectMultiplierArmor { get; set; }
        [XmlElement("StuffEffectMultiplierInsulation_Cold")] public double StuffEffectMultiplierInsulationCold { get; set; }
        [XmlElement("StuffEffectMultiplierInsulation_Heat")] public double StuffEffectMultiplierInsulationHeat { get; set; }
        [XmlElement("ArmorRating_Sharp")] public double ArmorRatingSharp { get; set; }
        [XmlElement("ArmorRating_Blunt")] public double ArmorRatingBlunt { get; set; }
        [XmlElement("ArmorRating_Heat")] public double ArmorRatingHeat { get; set; }
        [XmlElement("Insulation_Cold")] public double InsulationCold { get; set; }
        [XmlElement("Insulation_Heat")] public double InsulationHeat { get; set; }
        [XmlElement("EnergyShieldRechargeRate")] public double EnergyShieldRechargeRate { get; set; }
        [XmlElement("EnergyShieldEnergyMax")] public double EnergyShieldEnergyMax { get; set; }
        [XmlElement("SmokepopBeltRadius")] public double SmokepopBeltRadius { get; set; }
        [XmlElement("EquipDelay")] public double EquipDelay { get; set; }
    }

    public class EquippedStatOffsets
    {
        [XmlElement("MoveSpeed")] public double MoveSpeed { get; set; }
        [XmlElement("PsychicSensitivity")] public double PsychicSensitivity { get; set; }
        [XmlElement("PsychicEntropyRecoveryRate")] public double PsychicEntropyRecoveryRate { get; set; }
        [XmlElement("PainShockThreshold")] public double PainShockThreshold { get; set; }
        [XmlElement("SocialImpact")] public double SocialImpact { get; set; }
        [XmlElement("PawnBeauty")] public double PawnBeauty { get; set; }
        [XmlElement("TradePriceImprovement")] public double TradePriceImprovement { get; set; }
    }
}