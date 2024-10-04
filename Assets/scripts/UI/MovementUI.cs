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
        [SerializeField] ButtonUI readyBtn;

        float timer;
        int characterID;
        bool isOn;
        int timerTotal;
        float totalDistanceToEffort;
        float onInitRestoreBar;
        [SerializeField] float value = 1;
        [SerializeField] float value_player1;
        [SerializeField] float value_player2;

        private void Awake()
        {
            Events.OnMovementMade += OnMovementMade;
            readyBtn.Init(OnReadyClicked);
            totalDistanceToEffort = Settings.totalDistanceToEffort;
            onInitRestoreBar = Settings.onInitRestoreBar;
            value_player1 = 1;
            value_player2 = 1;
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
            value -= distance / (float)totalDistanceToEffort;
            if (characterID == 1)
                value_player1 = value;
            else
                value_player2 = value;
        }
        public void Reset()
        {
            panel.SetActive(false);
        }
        System.Action OnDone;
        public void Init(int characterID, int timerTotal, System.Action OnDone)
        {
            value = 1;
            if (characterID == 1)
            {
                this.value_player1 += onInitRestoreBar;
                if (value_player1 > 1) value_player1 = 1;
                value = value_player1;
            }
            else
            {
                this.value_player2 += onInitRestoreBar;
                if (value_player2 > 1) value_player2 = 1;
                value = value_player2;
            }
            this.OnDone = OnDone;
            this.timerTotal = timerTotal;
            isOn = true;
            timer = timerTotal;
            this.characterID = characterID;
            Color c;
            switch(characterID)
            {
                case 1: c = Color.red; break;
                default: c = Color.blue; break;
            }
            c.a = 0.2f;
            progressBar.SetColor(c);

            panel.SetActive(true);
            
        }
        void Update()
        {
            if (!isOn) return;

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
            value -= Time.deltaTime / timerTotal;
            progressBar.SetValue(value);
            if (value <= 0)
                End();
        }
    }
}
