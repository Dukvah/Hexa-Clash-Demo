using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private List<HexGround> hexGrounds =new();
    private HexGround lastHoveredHexGround;

    
    public HexGround HoveredHexGround(GameObject hoveredObject)
    {
        foreach (var hexGround in hexGrounds)
        {
            if (hoveredObject == hexGround.gameObject && hexGround.IsEmpty)
            {
                HoverExit();
                hexGround.OnHoverEnter();
                lastHoveredHexGround = hexGround;
                return hexGround;
            }
        }
        
        return null;
    }

    public void HoverExit()
    {
        if (lastHoveredHexGround != null)
            lastHoveredHexGround.OnHoverExit();
    }
}


