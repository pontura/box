using UnityEngine;
namespace Box
{
    public class GameStateWait : GameState
    {
        float timer = 0;
        public override void Init()
        {
            base.Init();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            bool movementsDone = gamesStatesManager.dbManager.MovementsDone();
            if (movementsDone)
                gamesStatesManager.GotoPlay();
        }
    }

}