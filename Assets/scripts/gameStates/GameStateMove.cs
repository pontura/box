using UnityEngine;
namespace Box
{
    public class GameStateMove : GameState
    {
        DraggingSystem draggingSystem;
        float timer;

        public override void Init()
        {
            if (draggingSystem == null)
                draggingSystem = new DraggingSystem();
            draggingSystem.Init(gamesStatesManager.dbManager);
            draggingSystem.OnReady(Finish);
            base.Init();
            Events.UIMovement(Settings.movementDuration, Finish);
            timer = 0;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            draggingSystem.OnUpdate();
        }
        void Finish()
        {
            draggingSystem.Reset();
            gamesStatesManager.MovementEnd();
        }
        public override void End()
        {
            base.End();
        }
    }
}
