using UnityEngine;

[CreateAssetMenu(fileName = "EnemySoldierData", menuName = "Soldier/EnemySoldierData")]
public class SOEnemySoldierData : MonoBehaviour
{
    public string soldierName;
    public int level;
    public SoldierRarity rarity;
    public int attackDamage;
    public int hp;
    public float attackSpeed;
}
