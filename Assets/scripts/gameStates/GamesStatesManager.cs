using Box.Photon;
using UnityEngine;

namespace Box
{
    public class GamesStatesManager : MonoBehaviour
    {
        [SerializeField] bool debugMode;
        GameState active;

        GameState move;
        GameState play;
        GameState wait;
        GameState loading;

        public DBManager dbManager;

        public CharacterManager ch1;
        public CharacterManager ch2;

        public PhotonGameManager photonGameManager;

        public CharacterManager GetCharacter(int id)
        {
            if (id == 1) return ch1; else return ch2;
        }

        public states state;
        public enum states
        {
            LOADING,
            MOVE_1,
            MOVE_2,
            WAITING,
            PLAY
        }
        private void Awake()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
            debugMode = false;
#endif
        }
        void Start()
        {
            move = new GameStateMove();
            play = new GameStatePlay();
            wait = new GameStateWait();
            loading = new GameStateLoading();

            loading.Initialize(this);
            move.Initialize(this);
            play.Initialize(this);
            wait.Initialize(this);

            if (debugMode)
                SetState(states.MOVE_1);
            else
            {
                if (photonGameManager.Master())
                    Settings.characterActive = 1;  
                else
                    Settings.characterActive = 2;
                SetState(states.LOADING);
            }

        }
        void SetState(states state)
        {
            if (active != null)
                active.End();
            switch (state)
            {
                case states.LOADING:
                    active = loading; break;
                case states.MOVE_1:
                    if (debugMode)
                        Settings.characterActive = 1;
                    active = move; break;
                case states.MOVE_2:
                    if (debugMode)
                        Settings.characterActive = 2;
                    active = move; break;
                case states.WAITING:
                    active = wait; break;

                case states.PLAY:
                    dbManager.OnParseMovements();
                    active = play; break;
            }
            Events.SetText("new state: " + state + " from: " + this.state);

            this.state = state;
            Events.OnChangeState(state);
            active.Init();
        }
        public void MovementEnd()
        {
            string movementString = "";

            switch (state)
            {
                case states.MOVE_1:
                    movementString = dbManager.GetMove(1);
                    if(debugMode)  SetState(states.MOVE_2);
                    else SetState(states.WAITING);
                    break;

                case states.MOVE_2:
                    movementString = dbManager.GetMove(2);
                    if (debugMode) GotoPlay();
                    else SetState(states.WAITING); break;
            }

            Events.SetText("MovementEnd: " + movementString);

            if (!debugMode)
                photonGameManager.SendMessageToOther(movementString);
        }
        public void PlayModeDone()
        {
            if (debugMode)
                SetState(states.MOVE_1);
            else
            {
                switch(Settings.characterActive)
                {
                    case 1: SetState(states.MOVE_1); break;
                    case 2: SetState(states.MOVE_2); break;
                }
            }
            dbManager.Reset();
        }
        public void GotoPlay()
        {
            SetState(states.PLAY);
        }
        void Update()
        {
            if (active != null)
                active.OnUpdate();
        }
    }
}
