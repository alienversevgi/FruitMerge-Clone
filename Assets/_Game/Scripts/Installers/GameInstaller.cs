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
            Container.DeclareSignal<GameSignals.OnDragging>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnDraggingCompleted>().OptionalSubscriber();
        }
    }
}