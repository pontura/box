using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

namespace Box.UI
{
    public class CharactersBar : MonoBehaviour
    {
        public ProgressBar player1;
        public ProgressBar player2;
        bool isOver;
        private void Start()
        {
            Events.OnHit += OnHit;
            player1.SetValue(1);
            player2.SetValue(1);

            player1.Init(1, OnGameOver);
            player2.Init(1, OnGameOver);
        }
        void OnGameOver()
        {
            if (isOver) return;
            isOver = true;
            Events.OnChangeState(GamesStatesManager.states.GAMEOVER);
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
