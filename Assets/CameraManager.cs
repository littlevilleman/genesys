using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera attackCamera;
    [SerializeField] private CinemachineVirtualCamera battleIntroCamera;

    private void OnEnable()
    {
        EventBus.Register<EventPlayCardStart>(OnPlayCard);
        EventBus.Register<EventPlayCardEnd>(OnPlayCardEnd);
        EventBus.Register<EventStartBattle>(OnStartBattle);
    }

    private void OnStartBattle(EventStartBattle context)
    {
        StartCoroutine(PlayIntroBattleCamera());
    }

    private IEnumerator PlayIntroBattleCamera()
    {
        battleIntroCamera.gameObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        battleIntroCamera.gameObject.SetActive(false);
    }

    private void OnPlayCard(EventPlayCardStart context)
    {
        mainCamera.gameObject.SetActive(false);
        attackCamera.gameObject.SetActive(true);
    }
    private void OnPlayCardEnd(EventPlayCardEnd context)
    {
        mainCamera.gameObject.SetActive(true);
        attackCamera.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        EventBus.Unregister<EventPlayCardStart>(OnPlayCard);
        EventBus.Unregister<EventPlayCardEnd>(OnPlayCardEnd);
    }
}
