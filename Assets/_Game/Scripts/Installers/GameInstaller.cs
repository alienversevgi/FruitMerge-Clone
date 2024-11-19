using _Game.Scripts.Misc;
using FruitMerge.Events;
using FruitMerge.Game;
using FruitMerge.Managers;
using Zenject;

namespace _Game.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] private PrefabSettings _prefabSettings;
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            BindSignals();
            BindPools();
            Container.BindInterfacesAndSelfTo<NextQueueHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SafeAreaHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<MergeHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<CameraAdjustHandler>().AsSingle();
        }

        private void BindPools()
        {
            Container.BindMemoryPool<MergeEffect, MergeEffect.Pool>()
                .WithInitialSize(Const.PoolSizes.MERGE_EFFECT_SIZE)
                .FromComponentInNewPrefab(_prefabSettings.MergeEffect)
                .WithGameObjectName("MergeEffect")
                .UnderTransformGroup("MergeEffect_PoolHolder");
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
            Container.DeclareSignal<GameSignals.OnSafeAreaChanged>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnGameOver>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnEntityAdded>().OptionalSubscriber();
            Container.DeclareSignal<GameSignals.OnEntityRemoved>().OptionalSubscriber();
        }
    }
}