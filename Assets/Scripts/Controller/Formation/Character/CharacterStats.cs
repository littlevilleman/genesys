using System;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [Range(0, 5)] public int strenght;   //damage base [1-10]
    [Range(0, 5)] public int speed;      //turn priority
    [Range(0, 5)] public int spirit;     //mana per turn [1-8]
    [Range(0, 5)] public int mind;       //cards draw per turn [1-8]
}