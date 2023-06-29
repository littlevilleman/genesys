using System;
using UnityEngine;
using UnityEngine.UI;

public class PointerView : MonoBehaviour
{
    [SerializeField] private Image pointerImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite highlightCardSprite;
    [SerializeField] private Sprite selectedCardSprite;

    private RectTransform rectTransform;
    private Vector3 screenSize;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        screenSize = new Vector3(Screen.width, Screen.height) / 100;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        EventBus.Register<EventUpdatePointer>(OnUpdatePointer);
    }

    private void OnUpdatePointer(EventUpdatePointer context)
    {
        Cursor.visible = false;
        pointerImage.sprite = GetStateSprite(context.state);
        pointerImage.SetNativeSize();
        pointerImage.enabled = pointerImage.sprite != null;
    }

    private Sprite GetStateSprite(EPointerState state)
    {
        switch (state)
        {
            case EPointerState.Default:
                return defaultSprite;
            case EPointerState.Hidden:
                return highlightCardSprite;
            case EPointerState.CardSelect:
                return selectedCardSprite;
        }

        return defaultSprite;
    }

    private void Update()
    {
        Vector3 v = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        rectTransform.position = new Vector3(v.x * screenSize.x, v.y * screenSize.y) - screenSize / 2;
    }


    private void OnDisable()
    {
        EventBus.Unregister<EventUpdatePointer>(OnUpdatePointer);
    }
}
