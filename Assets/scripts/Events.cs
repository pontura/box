using UnityEngine;
namespace Box
{
    public static class Events
    {
        public static System.Action<Vector2> OnDrag = delegate { };
        public static System.Action<int, float> OnHit = delegate { };
        public static System.Action<string> SetText = delegate { };
        public static System.Action<string> ReceiveMessage = delegate { };
        public static System.Action<int, float> OnMovementMade = delegate { };
        public static System.Action<GamesStatesManager.states> OnChangeState = delegate { };
        public static System.Action<int,System.Action> UIMovement = delegate { };

    }
}
