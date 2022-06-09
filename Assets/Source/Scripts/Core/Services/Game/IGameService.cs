namespace Core.Services
{
    public interface IGameService : IService
    {
        LevelData CurrentLevel { get; }
    }
}