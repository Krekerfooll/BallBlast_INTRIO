using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlust.Utils
{
    public static class RandomUtils
    {
        public static int RandomRange(this Vector2Int range, int divisible)
        {
            return RandomRange(range.x, range.y, divisible);
        }

        public static int RandomRange(this Vector2Int range)
        {
            return Random.Range(range.x, range.y);
        }

        public static int RandomRange(int min, int max, int divisible)
        {
            return Random.Range(min / divisible, max / divisible + 1) * divisible;
        }
    }
}
