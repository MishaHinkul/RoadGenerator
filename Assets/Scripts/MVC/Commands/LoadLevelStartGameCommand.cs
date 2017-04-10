using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelStartGameCommand : BaseCommand
{
    [Inject]
    public ICoroutineExecutor executor { get; private set; }
    public override void Execute()
    {
        Retain();
        executor.StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        SceneManager.LoadScene("menu");
        yield return new WaitForEndOfFrame();
        Release();
    }
}