using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class spikeEdier : MonoBehaviour
{
    public AudioSource audioSource;
    public int sampleSize = 1024;
    public float threshold = 1f;

    public int tr = 0;
    public int Randomm = 0;

    private float[] spectrum;

    public GameObject[] spike;
    public GameObject attackzone;
    public Transform spikeposition;

    private float lastSpawnTime = 0f;
    public float spawnCooldown = 0.2f;

    public GameObject circle;
    public Transform point;

    private GameObject currentSpikeGroup;

    public GameObject newpaturn;

    public Circlerotate Player;

    private int r = 0;

    //  키에 따라 선택되는 주파수 영역
    // 1 = 저음, 2 = 중음, 3 = 고음
    public int selectedFreq = 1;

    void Start()
    {
        spectrum = new float[sampleSize];
        currentSpikeGroup = new GameObject("SpikeGroup");
        currentSpikeGroup.transform.position = circle.transform.position;
    }

    void Update()
    {
        //  키 입력 처리
        if (Input.GetKeyDown(KeyCode.I)) selectedFreq = 1;
        if (Input.GetKeyDown(KeyCode.O)) selectedFreq = 2;
        if (Input.GetKeyDown(KeyCode.P)) selectedFreq = 3;

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float freqValue = 0f;
        string keyName = "";

        switch (selectedFreq)
        {
            case 1: // 저음
                float lowSum = 0f;
                for (int i = 1; i <= 50; i++) lowSum += spectrum[i];
                freqValue = (lowSum / 50f) * 200f;
                keyName = "저음";
                break;

            case 2: // 중음
                float midSum = 0f;
                for (int i = 51; i <= 200; i++) midSum += spectrum[i];
                freqValue = (midSum / 150f) * 500f;
                keyName = "중음";
                break;

            case 3: // 고음
                float highSum = 0f;
                for (int i = 201; i <= 512 && i < spectrum.Length; i++) highSum += spectrum[i];
                freqValue = (highSum / 312f) * 1000f;
                keyName = "고음";
                break;
        }

        if (freqValue > threshold && Time.time - lastSpawnTime >= spawnCooldown)
        {
            TriggerFunction();
            lastSpawnTime = Time.time;
        }

        Debug.Log($"{keyName} 감지 값: {freqValue:F3}");
    }

    void TriggerFunction()
    {
        Debug.Log("주파수 초과! 가시 생성");

        if (Randomm != 2)
        {
            GameObject gasi = Instantiate(spike[tr], spikeposition.position, spikeposition.rotation, currentSpikeGroup.transform);

            if (tr == 0)
            {
                tr++;
                Player.targetRadius = Player.innerRadius;
            }
            else
            {
                tr = 0;
                Player.targetRadius = Player.outerRadius;
            }
        }
        else
        {
            GameObject gasi = Instantiate(attackzone, spikeposition.position, spikeposition.rotation, currentSpikeGroup.transform);
        }

        Randomm = Random.Range(0, 11);
    }

    public void reset()
    {
#if UNITY_EDITOR
        string prefabPath = "Assets/lv2/" + currentSpikeGroup.name + r + ".prefab";
        r++;

        GameObject spikeGroupToSave = currentSpikeGroup;
        EditorApplication.delayCall += () =>
        {
            PrefabUtility.SaveAsPrefabAsset(spikeGroupToSave, prefabPath);
            Debug.Log("Prefab 저장: " + prefabPath);
            DestroyImmediate(spikeGroupToSave);
        };
#else
        Destroy(currentSpikeGroup);
#endif

        Vector3 dir = new Vector3 (0, -2, 0);
        GameObject newPatternInstance = Instantiate(newpaturn, dir, Quaternion.identity);
        circle = newPatternInstance;

        currentSpikeGroup = new GameObject("SpikeGroup");
        currentSpikeGroup.transform.position = circle.transform.position;
        circle.transform.SetParent(currentSpikeGroup.transform);
        circle.transform.localPosition = Vector3.zero;
    }
}
