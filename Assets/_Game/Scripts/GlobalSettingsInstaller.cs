using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = nameof(GlobalSettingsInstaller))]
public class GlobalSettingsInstaller : ScriptableObjectInstaller<GlobalSettingsInstaller>
{
    [SerializeField] private ScriptableObject[] _settings;

    public override void InstallBindings()
    {
        foreach (ScriptableObject setting in _settings)
        {
            Container.BindInterfacesAndSelfTo(setting.GetType()).FromInstance(setting);
        }
    }
}