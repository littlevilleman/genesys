using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "Config/CharacterConfig", order = 1)]
public class CharacterConfig : ScriptableObject
{
    public int id;
    public string name;

    [PreviewField(Height = 100)]
    public Sprite portrait;
    
    public AnimatorController animator;

    public CharacterStats baseStats;
    public List<CharacterTraitConfig> traits;
    public List<CardConfig> cards;
}