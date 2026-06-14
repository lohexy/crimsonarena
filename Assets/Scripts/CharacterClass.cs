using UnityEngine;

[CreateAssetMenu(fileName = "NewClass", menuName = "FantasyGame/Character Class")]
public class CharacterClass : ScriptableObject
{
    public string className;
    [TextArea] public string description;
    
    [Header("Початкові Характеристики")]
    public int startMaxHealth = 100;
    public float startHpRegen = 1f;
    public float startMoveSpeed = 5f;

    [Header("Параметри Бою та Ренджі")]
    public int startBaseDamage = 10;
    public float attackRangeMelee = 1.5f;
    public float attackRangeRanged = 5.0f;
    public float attackSpeedMultiplier = 1.0f;
    public float damageMultiplier = 1.0f;

    [Header("Крити / Захист")]
    [Range(0f, 1f)] public float startCritChance = 0.05f;
    public float critDamageMultiplier = 2.0f;
    [Range(0f, 1f)] public float startDodgeChance = 0.05f;
    [Range(0f, 1f)] public float startBlockChance = 0.0f;

    [Header("Множники Луту та Удача")]
    public float goldMultiplier = 1.0f;
    public float xpMultiplier = 1.0f;
    public float luck = 1.0f;

    [Header("Налаштування прокачки (Крок та Максимум)")]
    public int healthStep = 10;        public int healthMax = 500;
    public float regenStep = 0.2f;     public float regenMax = 10f;
    public float speedStep = 0.2f;     public float speedMax = 10f;
    public int damageStep = 2;         public int damageMax = 100;
    
    public float critStep = 0.02f;     public float critMax = 0.5f;
    public float critDmgStep = 0.1f;   public float critDmgMax = 5.0f;
    
    public float dodgeStep = 0.01f;    public float dodgeMax = 0.3f;
    public float blockStep = 0.02f;    public float blockMax = 0.6f;
    public float luckStep = 0.1f;      public float luckMax = 3.0f;

    [Header("Візуальний вигляд та Зброя")]
    public Sprite characterSprite;
    public GameObject startingWeaponPrefab;

    [Tooltip("Масштаб спрайту в грі")]
public float classSpriteScale = 1f;
}