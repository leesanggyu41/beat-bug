using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class spikeEdier : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;
    public int targetIndex = 10;  // üũ�� ���ļ� �ε���
    public float threshold = 1f;

    private float[] spectrum;

    public GameObject spike;
    public Transform spikeposition;

    private float lastSpawnTime = 0f;
    public float spawnCooldown = 0.2f; // 0.2�ʸ��ٸ� ���� ����

    public GameObject circle;

    // SpikeGroup �����
    private GameObject currentSpikeGroup;

    public GameObject newpaturn;

    private int r = 0;

    void Start()
    {
        spectrum = new float[sampleSize];
        // �ʱ� SpikeGroup ����
        currentSpikeGroup = new GameObject("SpikeGroup");
        currentSpikeGroup.transform.position = circle.transform.position;
    }

    void Update()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // ���� �ε��� ���
        float sum = 0f;
        for (int i = 1; i <= 10; i++) sum += spectrum[i];
        float freqValue = (sum / 10) * 50f;

        if (freqValue > threshold && Time.time - lastSpawnTime >= spawnCooldown)
        {
            TriggerFunction();
            lastSpawnTime = Time.time;
        }

        Debug.Log("���� ��: " + freqValue);
    }

    void TriggerFunction()
    {
        Debug.Log("���ļ� �ʰ�! ���� ����");
        // ���� SpikeGroup�� �ڽ����� ���� ����
        GameObject gasi = Instantiate(spike, spikeposition.position, spikeposition.rotation, currentSpikeGroup.transform);
    }

    public void reset()
    {
#if UNITY_EDITOR
        string prefabPath = "Assets/lv1/" + currentSpikeGroup.name + r + ".prefab";
        r++;

        // �����ϰ� ���� �����ӿ� Prefab ���� + ����
        GameObject spikeGroupToSave = currentSpikeGroup; // ���� ����
        EditorApplication.delayCall += () =>
        {
            PrefabUtility.SaveAsPrefabAsset(spikeGroupToSave, prefabPath);
            Debug.Log("Prefab ����: " + prefabPath);

            // Prefab ���� �� ����
            DestroyImmediate(spikeGroupToSave);
        };
#else
    Destroy(currentSpikeGroup);
#endif

        // �� ���Ͽ� ������Ʈ ����
        GameObject newPatternInstance = Instantiate(newpaturn, Vector3.zero, Quaternion.identity);
        circle = newPatternInstance;

        // �� SpikeGroup ����
        currentSpikeGroup = new GameObject("SpikeGroup");
        currentSpikeGroup.transform.position = circle.transform.position;
    }
}