using UnityEngine;

public class SpikePatternManager : MonoBehaviour
{
    public GameObject[] spikePatterns; // 만들어놓은 SpikeGroup Prefab 배열
    [SerializeField] private int currentIndex = -1;
    private GameObject currentPatternInstance;

    public Transform spawnPoint; // 패턴 생성 위치
    public AudioSource bgm;

    void Start()
    {
        SpawnNextPattern();
    }

    // 플레이어가 ResetZone에 닿으면 호출
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ResetCurrentPattern();
        }
    }

    void ResetCurrentPattern()
    {
        // 현재 패턴 삭제
        if (currentPatternInstance != null)
        {
            Destroy(currentPatternInstance);
        }

        // 다음 패턴 생성
        currentIndex++;
        if (currentIndex >= spikePatterns.Length) currentIndex = 0; // 반복 재생
        SpawnNextPattern();
        if(currentIndex == 1) bgm.Play();
    }

    void SpawnNextPattern()
    {
        currentPatternInstance = Instantiate(spikePatterns[currentIndex]);
    }
}
