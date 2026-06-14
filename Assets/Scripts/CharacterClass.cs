using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "FantasyGame/Character Class")]
public class CharacterClass : ScriptableObject
{
    public string className;
    [TextArea] public string description;
    
    [Header("Початкові Характеристики")]
    public int startHealth = 100;
    public float moveSpeed = 4f;
    public int baseDamage = 10;

    [Header("Візуальний вигляд")]
    public Sprite characterSprite;
    public GameObject startingWeaponPrefab;
}