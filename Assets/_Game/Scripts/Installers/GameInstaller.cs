using FruitMerge.Events;
using FruitMerge.Game;
using Zenject;

namespace _Game.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            BindSignals();
            Container.BindInterfacesAndSelfTo<NextQueueHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SafeAreaHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraAdjustHandler>().AsSingle();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<GameSignals.OnDragging>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnDraggingCompleted>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnEntityReleased>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnNextQueueUpdated>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnQueueAnimationCompleted>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnMergeCompleted>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnScoreGained>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.SafeAreaChanged>().OptionalSubscriber();
        }
    }
}