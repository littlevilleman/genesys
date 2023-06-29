using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [Header("Game Config")]
    [SerializeField] private List<CharacterConfig> characterConfig;

    [Header("UI Config")]
    [SerializeField] private List<CardStateConfig> cardStateConfig;

    private static ConfigManager instance;
    private Dictionary<ECardViewState, CardStateConfig> cardStateConfigDict = new ();
    private Dictionary<int, CharacterConfig> characterConfigDict = new();
    public static ConfigManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        foreach (CardStateConfig config in cardStateConfig)
        {
            cardStateConfigDict.Add(config.state, config);
        }

        foreach (CharacterConfig config in characterConfig)
        {
            characterConfigDict.Add(config.id, config);
        }
    }

    public CardStateConfig GetCardStateColor(ECardViewState state)
    {
        if (cardStateConfigDict.TryGetValue(state, out var config))
            return config;

        Debug.LogError($"Card Color Config not found for state {state}");
        return null;
    }

    public CharacterConfig GetCharacterConfig(int id)
    {
        if (characterConfigDict.TryGetValue(id, out var config))
            return config;

        Debug.LogError($"Character Config not found for id {id}");
        return null;
    }

    private void OnDestroy()
    {
        cardStateConfigDict.Clear();
    }
}