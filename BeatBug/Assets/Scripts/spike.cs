using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class spike : MonoBehaviour
{

    public Transform center;
    public float point;

    private void Awake()
    {
        center = GameObject.FindGameObjectWithTag("Center").GetComponent<Transform>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 newPos = center.transform.position - transform.position;
        float rotZ = Mathf.Atan2(newPos.y, newPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ-point);
    }

}
