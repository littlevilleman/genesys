using UnityEngine;
using UnityEngine.UI;

public class RoundBarCharacterView : MonoBehaviour
{
    [SerializeField] private Image image;

    private ICharacter character;

    public ICharacter Character => character;

    public void Setup(ICharacter setupCharacter)
    {
        character = setupCharacter;
        image.sprite = character.GetConfig().portrait;
    }
}