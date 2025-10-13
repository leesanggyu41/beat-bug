using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class spikeEdier : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;
    public int targetIndex = 10;  // 체크할 주파수 인덱스
    public float threshold = 1f;

    private float[] spectrum;

    public GameObject spike;
    public Transform spikeposition;

    private float lastSpawnTime = 0f;
    public float spawnCooldown = 0.2f; // 0.2초마다만 생성 가능

    public GameObject circle;

    // SpikeGroup 저장용
    private GameObject currentSpikeGroup;

    public GameObject newpaturn;

    private int r = 0;

    void Start()
    {
        spectrum = new float[sampleSize];
        // 초기 SpikeGroup 생성
        currentSpikeGroup = new GameObject("SpikeGroup");
        currentSpikeGroup.transform.position = circle.transform.position;
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // 저음 인덱스 평균
        float sum = 0f;
        for (int i = 1; i <= 10; i++) sum += spectrum[i];
        float freqValue = (sum / 10) * 50f;

        if (freqValue > threshold && Time.time - lastSpawnTime >= spawnCooldown)
        {
            TriggerFunction();
            lastSpawnTime = Time.time;
        }

        Debug.Log("저음 값: " + freqValue);
    }

    void TriggerFunction()
    {
        Debug.Log("주파수 초과! 가시 생성");
        // 현재 SpikeGroup의 자식으로 가시 생성
        GameObject gasi = Instantiate(spike, spikeposition.position, spikeposition.rotation, currentSpikeGroup.transform);
    }

    public void reset()
    {
#if UNITY_EDITOR
        string prefabPath = "Assets/lv1/" + currentSpikeGroup.name + r + ".prefab";
        r++;

        // 안전하게 다음 프레임에 Prefab 저장 + 삭제
        GameObject spikeGroupToSave = currentSpikeGroup; // 참조 보관
        EditorApplication.delayCall += () =>
        {
            PrefabUtility.SaveAsPrefabAsset(spikeGroupToSave, prefabPath);
            Debug.Log("Prefab 저장: " + prefabPath);

            // Prefab 저장 후 삭제
            DestroyImmediate(spikeGroupToSave);
        };
#else
    Destroy(currentSpikeGroup);
#endif

        // 새 패턴용 오브젝트 생성
        GameObject newPatternInstance = Instantiate(newpaturn, Vector3.zero, Quaternion.identity);
        circle = newPatternInstance;

        // 새 SpikeGroup 생성
        currentSpikeGroup = new GameObject("SpikeGroup");
        currentSpikeGroup.transform.position = circle.transform.position;
    }
}