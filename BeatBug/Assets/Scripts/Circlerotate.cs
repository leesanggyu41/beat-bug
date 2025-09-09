using UnityEngine;

public class Circlerotate : MonoBehaviour
{
    public float innerRadius = 2f;
    public float outerRadius = 3.5f;
    private float targetRadius;
    public Transform center;
    public float radius = 3f;
    public float speed = 2f;
    private float angle = 0f;

    void Start()
    {
        targetRadius = outerRadius;
    }

    void Update()
    {
        // A/D로 궤도 바꾸기
        if (Input.GetKeyDown(KeyCode.A))
            targetRadius = innerRadius;
        if (Input.GetKeyDown(KeyCode.D))
            targetRadius = outerRadius;

        // 부드럽게 반지름 보간
        float radius = Mathf.Lerp(Vector3.Distance(transform.position, center.position), targetRadius, Time.deltaTime * 50f);

        angle -= speed * Time.deltaTime;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        transform.position = center.position + new Vector3(x, y, 0);
    }
}
