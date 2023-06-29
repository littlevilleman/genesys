using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private static DeckManager instance;
    private Dictionary<int, List<ICard>> decks = new();
    
    public static DeckManager Instance => instance;


    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }

    public void Setup(CharacterConfig config)
    {
        //foreach (CharacterConfig characterConfig in config)
        //{
        List<ICard> cards = new ();
        foreach (CardConfig cardConfig in config.cards)
        {
            cards.Add(cardConfig.Build());
        }

        decks.Add(config.id, cards);
        //}
    }

    public ICard DrawRandomCard(ICharacter character)
    {
        if (decks.TryGetValue(character.GetConfig().id, out List<ICard> cards) && cards.Count > 0)
        {
            int random = Random.Range(0, cards.Count);
            return cards[random];
        }

        Debug.LogError($"Cards not found for character {character.GetConfig().id}");
        return null;
    }
}
