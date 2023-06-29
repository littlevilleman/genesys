using UnityEngine;
using UnityEngine.UI;

public class TurnActionView : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;

    private void OnEnable()
    {
        endTurnButton.onClick.AddListener(OnClickEndTurnButton);
    }

    private void OnClickEndTurnButton()
    {
        EventBus.Send(new EventTryEndTurn());
    }

    private void OnDisable()
    {
        endTurnButton.onClick.RemoveListener(OnClickEndTurnButton);
    }
}
