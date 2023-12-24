using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [NonSerialized]
        public static int WORLD_INDEX;
        [NonSerialized]
        public static int LEVEL_INDEX;
        
        public bool tutorialActivated;
    }
}
