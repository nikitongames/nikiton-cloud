using UnityEngine;

public class GameManager : MonoBehaviour
{
    [TextArea] public string welcomeText = "Welcome!";

    void Start()
    {
        Debug.Log(welcomeText);
    }
}
