using UnityEngine;
using System.Collections;

public interface ICoroutineExecutor
{
    Coroutine StartCroutine(string methodName);
    Coroutine StartCoroutine(IEnumerator method);
    Coroutine StartCoroutine(string methodName, object value);

    void StopCoroutine(string methodName);
    void StopCoroutine(IEnumerator method);
    void StopCoroutine(Coroutine method);
    void StopAllCoroutine();
}
