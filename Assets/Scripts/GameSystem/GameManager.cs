using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [NonSerialized]
        public static int WORLD_INDEX;
        [NonSerialized]
        public static int LEVEL_INDEX;
        public bool level1;
        public bool level2;
        public bool level3;
        public bool level4;
        [NonSerialized]
        public static bool firstTimePlaying = true;
    }
}
