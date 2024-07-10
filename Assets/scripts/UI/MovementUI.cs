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
        float effort;

        private void Awake()
        {
            Events.OnMovementMade += OnMovementMade;
            readyBtn.Init(OnReadyClicked);
            totalDistanceToEffort = Settings.totalDistanceToEffort;
            onInitRestoreBar = Settings.onInitRestoreBar;
            ResetValues();
        }
        void ResetValues()
        {
            effort = totalDistanceToEffort;
        }
        private void OnDestroy()
        {
            Events.OnMovementMade -= OnMovementMade;
        }
        void OnReadyClicked(int id)
        {
            End();
        }
        private void OnMovementMade(float distance)
        {
            effort -= distance;
            Debug.Log("distance " + distance + "  effort: " + effort + "  totalDistanceToEffort: " + totalDistanceToEffort);
            if (effort <= 0)
            {
                effort = 0; End();
            }
            SetValues();
        }
        void SetValues()
        {
            progressBar_moves.SetValue((float)effort / (float)totalDistanceToEffort);
        }
        public void Reset()
        {
            panel.SetActive(false);
        }
        System.Action OnDone;
        public void Init(int characterID, int timerTotal, System.Action OnDone)
        {
            Debug.Log("Move Init");
            this.effort += totalDistanceToEffort * onInitRestoreBar;
            if (effort > totalDistanceToEffort) effort = totalDistanceToEffort;
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
            SetValues();
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
            print("END isOn" + isOn + OnDone);
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
