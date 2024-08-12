using System;
using UnityEngine;

namespace Box.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text debugText;
        MainSignal mainSignal;

        [SerializeField] MovementUI movement;
        [SerializeField] PlayMoment playMoment;
        [SerializeField] GameOverScreen gameOver;

        private void Awake()
        {
            Events.SetText += SetText;
            Events.UIMovement += UIMovement;
            Events.OnChangeState += OnChangeState;
            mainSignal = GetComponent<MainSignal>();
            playMoment = GetComponent<PlayMoment>();
            gameOver = GetComponent<GameOverScreen>();
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
                    gameOver.Reset();
                    playMoment.Reset();
                    mainSignal.Init("MOVE!", OnMoveSignalDone);
                    break;
                case GamesStatesManager.states.PLAY:
                    gameOver.Reset();
                    movement.Reset();
                    playMoment.Init();
                    mainSignal.Init("ACTION!", OnMoveSignalDone);
                    break;
                case GamesStatesManager.states.GAMEOVER:
                    movement.Reset();
                    playMoment.Reset();
                    gameOver.Init();
                    mainSignal.Init("GAME OVER!", OnMoveSignalDone);
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