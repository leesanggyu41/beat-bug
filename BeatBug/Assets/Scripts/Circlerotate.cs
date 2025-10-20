using UnityEngine;

public class Circlerotate : MonoBehaviour
{
    public float innerRadius = 2f;
    public float outerRadius = 3.5f;
    public float targetRadius;
    public Transform center;
    public float radius = 3f;
    public float speed = 2f;
    private float angle = 0f;

    public float BPM = 120f;

    private bool isGo;
    public bool isDie;

    public AudioSource Audio;



    void Start()
    {
        targetRadius = outerRadius;

        // ���ϴ� ���� ��ġ ����
        transform.position = new Vector3(0f, 0.6f, 0f);

        // �߽����κ��� ���� ���� ���
        Vector3 dir = (transform.position - center.position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x);
    }

    void Update()
    {
        if (isDie)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            isGo = true;
            Audio.Play();
        }
        if (isGo)
        {
            // A/D�� �˵� �ٲٱ�
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetRadius = innerRadius;
                
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                targetRadius = outerRadius;
                
            }


            // �ε巴�� ������ ����
            float radius = Mathf.Lerp(Vector3.Distance(transform.position, center.position), targetRadius, Time.deltaTime * 50f);

            angle -= speed * Time.deltaTime;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            transform.position = center.position + new Vector3(x, y, 0);
        }

    }
}
