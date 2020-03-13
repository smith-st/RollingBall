using Settings;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Rolling Ball/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
{
    public GameSettings settings;
    public GamePrefabs prefabs;

    public override void InstallBindings()
    {
        Container.BindInstance(settings);
        Container.BindInstance(prefabs);
    }
}


