using Core.Attributes;

namespace Core.Services
{
    [InjectionAlias(typeof(IGameService))]
    public class GameService : Service, IGameService
    {
        public LevelData CurrentLevel { get; private set; }

        public override void Run()
        {
            Subscribe(NotificationType.LevelLoaded, OnLevelLoaded);
            base.Run();
        }

        private void OnLevelLoaded(NotificationType notificationType, NotificationParams notificationParams)
        {
            CurrentLevel = (LevelData) notificationParams.Data;
        }
    }
}