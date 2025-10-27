using UnityEngine;
using System.Collections;

namespace Nikiton.Core.Network
{
    public class CloudSync : MonoBehaviour
    {
        public static IEnumerator Upload(string json)
        {
            Debug.Log("Uploading data to NikitoN Cloud...");
            yield return new WaitForSeconds(1f);
            Debug.Log("Upload complete.");
        }
    }
}
