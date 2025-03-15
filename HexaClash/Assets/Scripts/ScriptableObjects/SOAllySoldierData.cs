using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AllySoldierData", menuName = "Soldier/AllySoldierData")]
public class SOAllySoldierData : ScriptableObject
{
    public string soldierName;
    public int level;
    public SoldierRarity soldierRarity;
    public SoldierTypes soldierType;
    public Sprite menuSprite;
    public Material hexagonMaterial;

    [SerializeField] private bool starterPack;
    public bool IsOpen { get; private set; }
    
    [SerializeField] private float attackDamage;
    [SerializeField] private float healthPoint;
    [SerializeField] private float attackSpeed;

    public float AttackDamage => attackDamage;
    public float HealthPoint => healthPoint;
    public float AttackSpeed => attackSpeed;
    
    public void LoadSoldierProperties()
    {
        IsOpen = PlayerPrefs.GetInt($"{soldierName}IsOpen", 0) != 0 || starterPack;
        level = PlayerPrefs.GetInt($"{soldierName}Level", 1);
        attackDamage += level;
        healthPoint += level;
        attackSpeed += level * .1f;
    }

    private void SaveSoldierProperties()
    {
        PlayerPrefs.SetInt($"{soldierName}Level", level);
    }

    public void LevelUp()
    {
        level++;
        SaveSoldierProperties();
        LoadSoldierProperties();
    }
}

public enum SoldierRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legend
}
public enum SoldierTypes 
{
    Knight,
    Wizard,
    Swordsman,
    Ranged,
    Spearman
}