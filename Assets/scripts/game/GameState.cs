namespace Box
{
    public class GameState
    {
        public GamesStatesManager gamesStatesManager;

        public void Initialize(GamesStatesManager gamesStatesManager)
        {
            this.gamesStatesManager = gamesStatesManager;
            OnInitialized();
        }
        public virtual void OnInitialized() { }
        public virtual void Init() { }
        public virtual void OnUpdate() { }
        public virtual void End() { }
    }
}