using UnityEngine;

public class EventPointerEnterCharacter : EventContext
{
    public CharacterViewBase characterView;
}

public class EventPointerExitCharacter : EventContext
{
    public CharacterViewBase characterView;
}

public class EventPointerClickCharacter : EventContext
{
    public CharacterViewBase character;
}