using UnityEngine;
namespace Box
{
    public class GameStateLoading : GameState
    {
        float timer = 0;
        public override void Init()
        {
            base.Init();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            bool twoPlayersLoaded = gamesStatesManager.photonGameManager.TwoPlayersLoaded();
            if (twoPlayersLoaded)
                gamesStatesManager.PlayModeDone();
        }
    }

}