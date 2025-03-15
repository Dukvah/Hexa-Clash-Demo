using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelHexStack", menuName = "Hexagon/HexagonStack")]
public class SOLevelHexStackData : ScriptableObject
{
    public List<WaveHexStack> waveHexStacks = new(); // Each element in the array is a wave soldier hexagon
}

[Serializable]
public class WaveHexStack // A waves hexagon stacks
{
    public List<HexStackData> hexStackDatas = new();
}

[Serializable]
public class HexStackData // A hexagon stack
{
    public List<HexStackObject> hexStackObjects = new();
}

[Serializable]
public class HexStackObject // A hexagon
{
    public SOAllySoldierData allySoldierData;
}