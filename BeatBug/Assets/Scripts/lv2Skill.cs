using UnityEngine;

public class lv2Skill : MonoBehaviour
{
    public GameObject puruuu;

    public void sandopen()
    {
        Instantiate(puruuu, transform.position, Quaternion.identity);
    }
}
