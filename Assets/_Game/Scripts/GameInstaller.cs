using FruitMerge.Events;
using Zenject;

namespace _Game.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            BindSignals();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<GameEvents.OnDragging>().OptionalSubscriber();
            Container.DeclareSignal<GameEvents.OnDraggingCompleted>().OptionalSubscriber();
        }
    }
}