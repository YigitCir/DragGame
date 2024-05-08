namespace DefaultNamespace.ServiceLocator
{
    public interface ILocalization
    {
        string GetLocalizedWord(string key);
    }

    public interface ISerializer
    {
        void Serialize();
    }
    
    public interface IAudioService
    {
        void Play();
    }
    
    public interface IGameService
    {
        void StartGame();
    }
}