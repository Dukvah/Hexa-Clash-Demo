using System.Collections.Generic;
using UnityEngine;

public class MidUpgradesMenu : MonoBehaviour
{
    [SerializeField] private List<HeroCard> heroCards = new();
    
    private void OnEnable()
    {
        HeroCardsInitializer();
    }
    
    private void HeroCardsInitializer()
    {
        var delay = 0.1f;
        foreach (var heroCard in heroCards)
        {   
            heroCard.SetCard(delay);
            delay += 0.05f;
        }
    }
    
}