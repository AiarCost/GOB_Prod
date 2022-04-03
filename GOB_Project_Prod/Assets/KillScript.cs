using UnityEngine;

public class KillScript : MonoBehaviour
{

    public float killTimer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, killTimer);
    }


}
