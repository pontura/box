using UnityEngine;

namespace Box
{
    public static class Settings
    {
        public static int characterActive;
        public static int totalPower = 1000;
        public static Vector2 limits = new Vector2(5.5f, 3.2f);

        // DRAG:
        public static int movementDuration = 8;
        public static float offsetToDraw = 0.15f;
        public static float offsetTime = 0.025f;
        public static float maxDistanceFromAnchor = 5; // Arms length;
        public static float maxDistanceAllowed = 2f; // each movement dragging calculates this

        // PLAYBACK:
        public static float playSpeed = 1f;
        public static float movementLerp = 0.01f;

        //EFFORT
        public static float onInitRestoreBar = 0.3f; // cuanto hace restore la barra de effort por jugada
        public static float totalDistanceToEffort = 200;
        public static float effortByDistance = 50;

    }

}