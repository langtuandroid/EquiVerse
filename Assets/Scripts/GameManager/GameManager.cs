using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [NonSerialized] //Erwin: Static variables kunnen niet geserialiseerd worden
        public static int WORLD_INDEX;
        [NonSerialized]
        public static int LEVEL_INDEX;
        public  bool tutorialActivated;
        public bool secondLevelTutorialActivated;
        [NonSerialized]
        public static bool firstTimePlaying = true;
    }
}
