using UnityEngine;

namespace Box.UI
{
    public class CharactersBar : MonoBehaviour
    {
        public ProgressBar player1;
        public ProgressBar player2;

        private void Start()
        {
            Events.OnHit += OnHit;
            player1.SetValue(1);
            player2.SetValue(1);
        }
        private void OnDestroy()
        {
            Events.OnHit -= OnHit;
        }
        private void OnHit(int playerID, float power)
        {
            ProgressBar bar = GetPowerbar(playerID);
            float damage = ((float)power / (float)Settings.totalPower);
            bar.Add(-damage);

            if (bar.value <= 0)
                Events.OnGameOver();

            Debug.Log("playerID " + playerID + " power: " + power + " bar:" + bar.value + " damage: " + damage);
        }
        ProgressBar GetPowerbar(int playerID)
        {
            switch (playerID)
            {
                case 1: return player1;
                default: return player2;
            }
        }
}
}
