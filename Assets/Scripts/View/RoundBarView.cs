using System.Collections.Generic;
using UnityEngine;

public class RoundBarView : MonoBehaviour
{
    [SerializeField] private RoundBarCharacterView characterRoundViewPrefab;

    private List<RoundBarCharacterView> characterViews = new(); 

    private void OnEnable()
    {
        EventBus.Register<EventStartBattle>(OnStartBattle);
        EventBus.Register<EventUpdateRoundQueue>(OnUpdateRoundQueue);
    }

    private void OnStartBattle(EventStartBattle context)
    {
        int slot = 0;
        foreach (ICharacter character in context.rounds[0])
        {
            RoundBarCharacterView characterView = Instantiate(characterRoundViewPrefab, transform.GetChild(slot));
            characterView.Setup(character);
            characterViews.Add(characterView);
            slot++;
        }
    }

    private void OnUpdateRoundQueue(EventUpdateRoundQueue context)
    {
        int slot = 0;
        foreach (List<ICharacter> round in context.rounds)
        {
            foreach (var character in round)
            {
                var characterView = GetCharacterRoundView(character);
                characterView.transform.SetParent(transform.GetChild(slot));
                characterView.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                slot++;
            }
        }
    }

    private RoundBarCharacterView GetCharacterRoundView(ICharacter character)
    {
        return characterViews.Find(x => x.Character == character);
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventStartBattle>(OnStartBattle);
        EventBus.Unregister<EventUpdateRoundQueue>(OnUpdateRoundQueue);
    }
}