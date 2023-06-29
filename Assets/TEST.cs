using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TEST", menuName = "Config/Card/Test", order = 1)]
public class TEST : ScriptableObject
{
    public string cardName;
    public string description;
    public int manaCost;
    public Sprite picture;
    //public List<T> actions;

    [EnumToggleButtons]
    public ETargetType TargetType;

    public FactionLocationSet[] sourceLocations;
    public FactionLocationSet[] targetLocations;

    //public FormationLocation[] sourceLocations;
    //public FormationLocation[] targetLocations;

    public bool asd;

    [EnumToggleButtons]
    public InfoMessageType asdd;

    public FactionLocationSet[] TargetLocations => GetTargetLocations();

    private FactionLocationSet[] GetTargetLocations()
    {
        switch (TargetType)
        {
            case ETargetType.TARGET:
            //return targetLocations;
            //case ETargetType.SELF:
            //    return FactionLocationSet.Self;
            case ETargetType.ALLY_TEAM:
                return FactionLocationSet.AllyTeam;
            case ETargetType.ENEMY_TEAM:
                return FactionLocationSet.EnemyTeam;
        }

        return null;
    }
}