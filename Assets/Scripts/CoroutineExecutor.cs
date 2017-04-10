using UnityEngine;
using System.Collections;
using strange.extensions.injector.api;
using strange.extensions.context.api;

namespace strange.examples.strangerocks
{
    [Implements(typeof(ICoroutineExecutor), InjectionBindingScope.CROSS_CONTEXT)]
    public class CoroutineExecutor : ICoroutineExecutor
    {

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        private CoroutineMonoBehaviour mb;

        [PostConstruct]
        public void PostConstruct()
        {
            mb = contextView.AddComponent<CoroutineMonoBehaviour>();
        }

        public Coroutine StartCroutine(string methodName)
        {
            return mb.StartCoroutine(methodName);
        }
        public Coroutine StartCoroutine(IEnumerator method)
        {
            return mb.StartCoroutine(method);
        }
        public Coroutine StartCoroutine(string methodName, object value)
        {
            return mb.StartCoroutine(methodName, value);
        }

        public void StopCoroutine(string methodName)
        {
            mb.StopCoroutine(methodName);
        }
        public void StopCoroutine(IEnumerator method)
        {
            mb.StopCoroutine(method);
        }
        public void StopCoroutine(Coroutine method)
        {
            mb.StopCoroutine(method);
        }
        public void StopAllCoroutine()
        {
            mb.StopAllCoroutines();
        }
    }

    public class CoroutineMonoBehaviour : MonoBehaviour
    {
    }
}