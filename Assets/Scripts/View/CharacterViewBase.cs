using DG.Tweening;
using UnityEngine;

public abstract class CharacterViewBase : MonoBehaviour
{
    private ICharacter character;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private float formationLocation;
    private float battleLocation;
    private float outLocation;

    public ICharacter Character => character;

    private void OnEnable()
    {
        EventBus.Register<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Register<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Register<EventReceiveDamage>(OnReceiveDamage);
        EventBus.Register<EventCharacterDie>(OnCharacterDie);
    }

    public void Setup(ICharacter character)
    {
        this.character = character;
        animator.runtimeAnimatorController = character.GetConfig().animator;

        formationLocation = transform.position.x;
        battleLocation = .1f * (Character.GetFaction() == EFaction.Ally ? -1 : 1);
        outLocation = 2f * (Character.GetFaction() == EFaction.Ally ? -1 : 1);
    }

    private void OnPlayCardStart(EventPlayCardStart context)
    {
        if (character == context.source)
        {
            animator.SetTrigger("Attack");
            return;
        }
    }

    private void OnPlayCardEnd(EventPlayCardEnd context)
    {
    }

    private void OnReceiveDamage(EventReceiveDamage context)
    {
        if (character != context.target)
            return;

        animator.SetTrigger("ReceiveAttack");
    }

    private void OnCharacterDie(EventCharacterDie context)
    {
        if (character != context.character)
            return;

        animator.SetBool("Dead", true);
    }

    public void MoveToBattlePosition()
    {
        transform.DOMoveX(battleLocation, .5f);
    }

    public void MoveToFormationPosition()
    {
        transform.DOMoveX(formationLocation, .5f);
    }

    public void MoveOut()
    {
        transform.DOMoveX(outLocation, .5f);
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventCharacterDie>(OnCharacterDie);
        EventBus.Unregister<EventPlayCardStart>(OnPlayCardStart);
        EventBus.Unregister<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Unregister<EventReceiveDamage>(OnReceiveDamage);
    }
}