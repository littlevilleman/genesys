using UnityEngine;

[CreateAssetMenu(fileName = "BattleConfig", menuName = "Config/Battle/BattleConfig", order = 1)]
public class BattleConfig : ScriptableObject
{
    public FormationConfig[] formations;
}