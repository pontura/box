using UnityEngine;
namespace Box
{
    public class GameStateMove : GameState
    {
        DraggingSystem draggingSystem;
        int totalTime = 4;
        float timer;

        public override void Init()
        {
            if (draggingSystem == null)
                draggingSystem = new DraggingSystem();
            draggingSystem.Init(gamesStatesManager.dbManager);
            draggingSystem.OnReady(Finish);
            base.Init();
            Events.UIMovement(totalTime);
            timer = 0;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            timer += Time.deltaTime;
            if (timer >= totalTime)
                Finish();
            else
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
