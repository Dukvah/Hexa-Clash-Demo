using Managers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level", menuName = "Create Level/Level", order = 1)]
    public class SOLevel : ScriptableObject
    {
        public GameObject levelPrefab;
        public int levelNumber;
        public string levelName;
        public Sprite levelIcon;
    }
}