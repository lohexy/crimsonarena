using UnityEngine;
using TMPro;

public class StatMenuUI : MonoBehaviour
{
    [Header("Посилання на гравця")]
    public PlayerStats playerStats;

    [Header("Текстові поля характеристик")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI hpRegenText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI blockText;
    public TextMeshProUGUI luckText;
    public TextMeshProUGUI pointsText;

    void OnEnable()
    {
        RefreshStatTexts();
    }

    public void RefreshStatTexts()
    {
        if (playerStats == null) return;

        hpText.text = $"ХП: {playerStats.currentHealth}/{playerStats.maxHealth}";
        hpRegenText.text = $"Реген ХП: {playerStats.hpRegenPerSecond:F1}/сек";
        damageText.text = $"Урон: {playerStats.baseDamage} (+{playerStats.bonusDamage})";
        speedText.text = $"Швидкість: {playerStats.moveSpeed:F1}";
        critChanceText.text = $"crit chance: {(playerStats.critChance * 100f):F0}%";
        critDamageText.text = $"crit dmg: x{playerStats.critDamageMultiplier:F1}";
        dodgeText.text = $"Ухилення: {(playerStats.dodgeChance * 100f):F0}%";
        blockText.text = $"Блок: {(playerStats.blockChance * 100f):F0}%";
        luckText.text = $"Удача: {playerStats.luck:F1}";
        
        if (pointsText != null)
        {
            pointsText.text = $"Очки: {playerStats.statPoints}";
        }
    }

    public void OnClickUpgradeHP() { playerStats.UpgradeHealth(); RefreshStatTexts(); }
    public void OnClickUpgradeRegen() { playerStats.UpgradeRegen(); RefreshStatTexts(); }
    public void OnClickUpgradeDamage() { playerStats.UpgradeDamage(); RefreshStatTexts(); }
    public void OnClickUpgradeSpeed() { playerStats.UpgradeSpeed(); RefreshStatTexts(); }
    public void OnClickUpgradeCritDamage() { playerStats.UpgradeCritDamage(); RefreshStatTexts();}
    public void OnClickUpgradeCritChance() { playerStats.UpgradeCrit(); RefreshStatTexts(); }
    public void OnClickUpgradeDodge() { playerStats.UpgradeDodge(); RefreshStatTexts(); }
    public void OnClickUpgradeBlock() { playerStats.UpgradeBlock(); RefreshStatTexts(); }
    public void OnClickUpgradeLuck() { playerStats.UpgradeLuck(); RefreshStatTexts(); }
}