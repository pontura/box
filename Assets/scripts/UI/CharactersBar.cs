using UnityEngine;

namespace Box.UI
{
    public class CharactersBar : MonoBehaviour
    {
        [SerializeField] ProgressBar player1;
        [SerializeField] ProgressBar player2;

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
            float resta = (float)power / (float)Settings.totalPower;
            bar.Add(resta);
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
