using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    [SerializeField] private Animator battleFaderAnimator;

    private void OnEnable()
    {
        EventBus.Register<EventStartBattle>(OnStartBattle);
    }

    private void OnStartBattle(EventStartBattle context)
    {
        battleFaderAnimator.SetTrigger("Start");
        StartCoroutine(SetupCameraDelay());
    }

    private IEnumerator SetupCameraDelay()
    {
        yield return new WaitForEndOfFrame();
    }

    private void OnDisable()
    {
        EventBus.Unregister<EventStartBattle>(OnStartBattle);
    }
}
