using Factories;
using Managers;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TilesFactory>().AsSingle();
        Container.Bind<BallsFactory>().AsSingle();
        Container.Bind<BonusFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<TileManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<BallManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<BonusManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MainController>().AsSingle();
        InstallExecutionOrder();
    }

    private void InstallExecutionOrder()
    {
        Container.BindExecutionOrder<BonusManager>(-12);
        Container.BindExecutionOrder<TileManager>(-11);
        Container.BindExecutionOrder<BallManager>(-10);
        Container.BindExecutionOrder<MainController>(-1);
    }
}