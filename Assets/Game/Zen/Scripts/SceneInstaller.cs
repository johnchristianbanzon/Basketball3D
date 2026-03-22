using Zenject;

public class SceneInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        //Container.Bind<ContainerToBind>().AsSingle();
        Container.Bind<CursorManager>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<GameManager>().FromComponentsInHierarchy().AsSingle();
    }
}