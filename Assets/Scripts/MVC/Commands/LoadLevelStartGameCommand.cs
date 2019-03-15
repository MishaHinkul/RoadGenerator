using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelStartGameCommand : BaseCommand
{
  public override void Execute()
  {
    Retain();
    Executor.StartCoroutine(LoadLevel());
  }

  private IEnumerator LoadLevel()
  {
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    AsyncOperation async = null; async = SceneManager.LoadSceneAsync("level");
    async.allowSceneActivation = false;
    async.priority = 4;
    async.allowSceneActivation = true;

    while (async.progress < 0.99f)
    {
      yield return wait;
    }
    Release();
  }


  [Inject]
  public ICoroutineExecutor Executor { get; private set; }
}