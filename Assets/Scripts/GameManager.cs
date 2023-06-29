using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameDataManager gameDataManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private BattleConfig battleConfig;

    private EGameState state;

    public GameDataProfile Data => gameDataManager.GameDataProfile;
    public EGameState State => state;

    public void LoadGameProfile()
    {
        gameDataManager.LoadDataProfile();

        //load world
        //load characters
        //load cards
    }

    private void Start()
    {
        battleManager.SetupBattle(battleConfig);
        battleManager.LaunchBattle(1.5f);
        var config = ConfigManager.Instance.GetCharacterConfig(battleConfig.formations[0].characters[0].id);
        deckManager.Setup(config);

        state = EGameState.Battle;
    }
}

public enum EGameState
{
    None, CheckPoint, Travel, Battle
}