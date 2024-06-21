using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box.UI
{
    public class MovementUI : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] ProgressBar progressBar;
        [SerializeField] ProgressBar progressBar_moves;

        float timer;
        int characterID;
        bool isOn;
        int timerTotal;

        private void Awake()
        {
            Events.OnMovementMade += OnMovementMade;
        }
        private void OnDestroy()
        {
            Events.OnMovementMade -= OnMovementMade;
        }

        private void OnMovementMade(int move, int totalMoves)
        {
            if (move > totalMoves)
            {
                move = totalMoves; End();
            }
            progressBar_moves.SetValue((float)move / (float)totalMoves);
        }
        public void Reset()
        {
            panel.SetActive(false);
        }
        public void Init(int characterID, int timerTotal)
        {
            this.timerTotal = timerTotal;
            isOn = true;
            timer = timerTotal;
            this.characterID = characterID;
            switch(characterID)
            {
                case 1:
                    progressBar_moves.SetColor(Color.red);
                    break;
                case 2:
                    progressBar_moves.SetColor(Color.blue);
                    break;
            }
            progressBar_moves.SetValue(0);
            progressBar.SetValue(1);

            panel.SetActive(true);
        }
        void Update()
        {
            if (!isOn) return;
            if (timer <= 0)
                End();
            else
                SetProgress();
        }
        void End()
        {
            if (isOn)
            {
                isOn = false;
            }
        }
        void SetProgress()
        {
            timer -= Time.deltaTime;
            if (timer > timerTotal)
            {
                End();
                timer = timerTotal;
            }
            progressBar.SetValue(timer / timerTotal);
        }
    }
}
