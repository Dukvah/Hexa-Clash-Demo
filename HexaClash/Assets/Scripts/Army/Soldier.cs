using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    private int attackDamage;
    private int hp;
    private int level;
    private float attackSpeed;
    
    public int AttackDamage => attackDamage;
    public int Hp => hp;
    public int Level => level;
    public float AttackSpeed => attackSpeed;

    public virtual void Attack() { }
    public virtual void TakeDamage(int damage) { }
}