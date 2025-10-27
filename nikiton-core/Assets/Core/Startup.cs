using UnityEngine;

namespace Nikiton.Core
{
    public class Startup : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            var core = new GameObject("CoreManager");
            core.AddComponent<CoreManager>();
        }
    }
}
