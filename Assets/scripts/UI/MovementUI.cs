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
        [SerializeField] ButtonUI readyBtn;

        float timer;
        int characterID;
        bool isOn;
        int timerTotal;
        float totalDistanceToEffort;
        float onInitRestoreBar;
        float effort_player1;
        float effort_player2;

        private void Awake()
        {
            Events.OnMovementMade += OnMovementMade;
            readyBtn.Init(OnReadyClicked);
            totalDistanceToEffort = Settings.totalDistanceToEffort;
            onInitRestoreBar = Settings.onInitRestoreBar;

            effort_player1 = totalDistanceToEffort;
            effort_player2 = totalDistanceToEffort;
        }
        private void OnDestroy()
        {
            Events.OnMovementMade -= OnMovementMade;
        }
        void OnReadyClicked(int id)
        {
            End();
        }
        private void OnMovementMade(int characterID, float distance)
        {
            if (characterID == 1)
            {
                effort_player1 -= distance;
                if (effort_player1 <= 0)
                {
                    End();
                    effort_player1 = 0;
                }
            }
            else if (characterID == 2)
            {
                effort_player2 -= distance;
                if (effort_player2 <= 0)
                {
                    End();
                    effort_player2 = 0;
                }
            }
            SetValues(characterID);
        }
        void SetValues(int characterID)
        {
            if(characterID == 1)
                progressBar_moves.SetValue((float)effort_player1 / (float)totalDistanceToEffort);
            else
                progressBar_moves.SetValue((float)effort_player2 / (float)totalDistanceToEffort);
        }
        public void Reset()
        {
            panel.SetActive(false);
        }
        System.Action OnDone;
        public void Init(int characterID, int timerTotal, System.Action OnDone)
        {
            Debug.Log("Move Init");
            if (characterID == 1)
            {
                this.effort_player1 += totalDistanceToEffort * onInitRestoreBar;
                if (effort_player1 > totalDistanceToEffort) effort_player1 = totalDistanceToEffort;
            }
            else
            {
                this.effort_player2 += totalDistanceToEffort * onInitRestoreBar;
                if (effort_player2 > totalDistanceToEffort) effort_player2 = totalDistanceToEffort;
            }
            Debug.Log("characterID " + characterID + " this.effort_player1: " + this.effort_player1 + "   this.effort_player2: " + this.effort_player2);
            this.OnDone = OnDone;
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
            progressBar.SetValue(1);

            panel.SetActive(true);
            SetValues(characterID);
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
                OnDone();
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
