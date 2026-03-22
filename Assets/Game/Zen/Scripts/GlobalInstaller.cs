using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        UnityEngine.Debug.Log("GLOBAL BIND");


       // Container.Bind<ScoreManager>().FromComponentInHierarchy().AsSingle();
         Container.Bind<IScoreManager>().To<ScoreManager>().AsSingle();

    }
}
