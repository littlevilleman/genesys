using UnityEngine;

public class FormationView : MonoBehaviour
{
    [SerializeField] private EFaction faction;
    [SerializeField] private CharacterViewBase [] characters = new CharacterViewBase[4];
    [SerializeField] private CharacterViewBase characterViewPrefab;

    private Formation formation;
    public Formation Formation => formation;

    private void OnEnable()
    {
        EventBus.Register<EventSetupFormation>(OnSetupFormation);
        EventBus.Register<EventSetupFormationCharacter>(OnSetupFormationCharacter);
        EventBus.Register<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Register<EventPlayCardEnd>(OnPlayCardEnd);
    }

    private void OnSetupFormation(EventSetupFormation context)
    {
        if (context.formation.Faction != faction)
            return;

        Debug.Log("Setup formation " + context.formation + " " + context.formation.Faction);
        formation = context.formation;
    }

    private void OnSetupFormationCharacter(EventSetupFormationCharacter context)
    {
        Debug.Log("Character " + context.character);

        if (context.formation != formation)
            return;

        if (context.character == null)
        {
            Debug.Log("NULL " + context.character);
            characters[context.location].gameObject.SetActive(false);
            return;
        }

        characters[context.location].Setup(context.character);
    }

    private void OnPlayCardStart(EventPlayCardStart context)
    {
        foreach (CharacterView character in characters)
        {
            if (character.Character == context.source)
                character.MoveToBattlePosition();
            else if (BattleManager.Instance.GetTargets(context.card, context.source, context.focus).Contains(character.Character))
                character.MoveToBattlePosition();
            else if (character.Character == context.focus)
                character.MoveToBattlePosition();
            else
                character.MoveOut();
        }
    }

    private void OnPlayCardEnd(EventPlayCardEnd context)
    {
        foreach (CharacterView character in characters)
        {
            character.MoveToFormationPosition();
        }
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventSetupFormation>(OnSetupFormation);
        EventBus.Unregister<EventSetupFormationCharacter>(OnSetupFormationCharacter);
        EventBus.Unregister<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Unregister<EventPlayCardEnd>(OnPlayCardEnd);
    }
}