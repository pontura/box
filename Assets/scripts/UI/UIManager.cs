using System;
using UnityEngine;

namespace Box.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text debugText;
        static UIManager mInstance;
        static UIManager Instance { get { return mInstance; } }
        MainSignal mainSignal;

        [SerializeField] MovementUI movement;
        [SerializeField] PlayMoment playMoment;
        [SerializeField] GameOverScreen gameOverScreen;

        private void Awake()
        {
            mInstance = this;
            mainSignal = GetComponent<MainSignal>();
            playMoment = GetComponent<PlayMoment>();
            gameOverScreen = GetComponent<GameOverScreen>();

            Events.SetText += SetText;
            Events.UIMovement += UIMovement;
            Events.OnChangeState += OnChangeState;
        }
        private void OnDestroy()
        {
            Events.SetText -= SetText;
            Events.UIMovement -= UIMovement;
            Events.OnChangeState -= OnChangeState;
        }

        private void OnChangeState(GamesStatesManager.states state)
        {
            switch(state)
            {
                case GamesStatesManager.states.MOVE_1:
                case GamesStatesManager.states.MOVE_2:
                    playMoment.Reset();
                    mainSignal.Init("MOVE!", OnMoveSignalDone);
                    break;
                case GamesStatesManager.states.PLAY:
                    movement.Reset();
                    playMoment.Init();
                    mainSignal.Init("ACTION!", OnMoveSignalDone);
                    break;
                case GamesStatesManager.states.GAMEOVER:
                    movement.Reset();
                    playMoment.Reset();
                    gameOverScreen.Init(); 
                    break;

            }
        }
        private void UIMovement(int totalTime, System.Action OnDone)
        {
            movement.Init(Settings.characterActive, totalTime, OnDone);
        }
        void OnMoveSignalDone()
        {

        }
        void SetText(string s)
        {
            string old = debugText.text;
            debugText.text = s + "\n" + old;
        }

    }
}