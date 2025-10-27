using UnityEngine;

namespace Nikiton.Core
{
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            NetworkManager.Initialize();
            DataManager.Initialize();
            UIManager.Initialize();
        }
    }
}
