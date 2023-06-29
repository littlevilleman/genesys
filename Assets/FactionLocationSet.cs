using Sirenix.OdinInspector;
using System;

[Serializable]
public class LocationSet
{
    [InlineProperty(LabelWidth = 15)]
    [PropertyOrder(1)]
    public FormationLocation formationLocation;

    public bool[] Locations => new bool[4] { formationLocation.one, formationLocation.two, formationLocation.three, formationLocation.four };
}

[Serializable]
public class FactionLocationSet : LocationSet
{
    [EnumToggleButtons()]
    [ReadOnly]
    public EFaction faction;

    public static FactionLocationSet[] AllyTeam => GetTeamLocations(EFaction.Ally);
    public static FactionLocationSet[] EnemyTeam => GetTeamLocations(EFaction.Enemy);
    public static FactionLocationSet[] Global => GetGlobalLocations();

    private static FactionLocationSet[] GetGlobalLocations()
    {
        return new FactionLocationSet[] { new FactionLocationSet() { faction = EFaction.Ally, formationLocation = FormationLocation.All },
                                            new FactionLocationSet() { faction = EFaction.Enemy, formationLocation = FormationLocation.All }
                                        };
    }

    private static FactionLocationSet[] GetTeamLocations(EFaction faction)
    {
        return new FactionLocationSet[] { new FactionLocationSet() { faction = faction, formationLocation = FormationLocation.All } };
    }

    public static FactionLocationSet[] GetLocationByIndex(EFaction faction, int locationIndex)
    {
        return new FactionLocationSet[] { new FactionLocationSet() { faction = faction, formationLocation = FormationLocation.GetSelf(locationIndex) } };
    }
}

[Serializable]
public struct FormationLocation
{

    [LabelText("")]
    [HorizontalGroup("", Width = 20)]
    public bool one;

    [LabelText("")]
    [HorizontalGroup("", Width = 20)]
    public bool two;

    [LabelText("")]
    [HorizontalGroup("", Width = 20)]
    public bool three;

    [LabelText("")]
    [HorizontalGroup("", Width = 20)]
    public bool four;

    public FormationLocation(bool one, bool two, bool three, bool four)
    {
        this.one = one;
        this.two = two;
        this.three = three;
        this.four = four;
    }

    public static FormationLocation GetSelf(int locationIndex)
    {
        return new FormationLocation(locationIndex == 0, locationIndex == 1, locationIndex == 2, locationIndex == 3);
    }

    public static FormationLocation All => new FormationLocation (true, true, true, true );
}