using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelStartGameCommand : BaseCommand
{
    [Inject]
    public ICoroutineExecutor executor { get; private set; }

    AsyncOperation async;

    public override void Execute()
    {
        Retain();
        executor.StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        async = Application.LoadLevelAsync("level");
        async.allowSceneActivation = false;
        async.priority = 4;
        async.allowSceneActivation = true;
        while (async.progress < 0.99f)
        {
            yield return new WaitForSeconds(0.1f);
        }
            Release();
    }
}