using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("컴포넌트 설정")]
    [SerializeField]
    private Animator BossAnim;
    [SerializeField]
    private GameObject HPBarParent; // HP 바를 포함하는 부모 GameObject
    public int Lv;

    // 이 변수는 Inspector에서 직접 연결하는 것이 좋습니다.
    [SerializeField]
    private Slider HPbarSlider;

    [Header("능력치")]
    public int maxHp = 10;
    public int currentHp; // 변수명을 currentHp로 변경
    public bool isDie = false; // 변수명 isdie를 isDie로 변경

    [Header("스킬")]
    public float maxCool;
    public float minCool;
    // 움직임의 강도 (카메라가 좌우로 얼마나 밀려날지)
    public float pushDistance = 1.5f;

    // 움직이는 데 걸리는 시간 (애니메이션 속도에 맞춰 조정 필요)
    public float moveDuration = 0.2f;

    public float kill3Dis = 5f;

    public float killDur = 0.2f;

    public Transform point;
    public GameObject skillOj;

    //  체력바 감소 속도 조절용 변수
    private const float HealthBarDecreaseSpeed = 2f;
    private Coroutine damageCoroutine; // 코루틴 참조 저장용

    [SerializeField]
    private AudioSource BGM;
    [SerializeField]
    private GameObject Camera;
    private Vector3 originalPos;
    [SerializeField]
    private GameObject Center;
    [SerializeField]
    private GameObject next;

    private void Start()
    {
        currentHp = maxHp;

        //  슬라이더 컴포넌트가 Inspector에 연결되어 있다고 가정합니다.
        // 연결되어 있지 않다면 아래 코드를 사용해 찾을 수 있습니다.
        if (HPbarSlider == null && HPBarParent != null)
        {
            HPbarSlider = HPBarParent.GetComponentInChildren<Slider>();
        }

        if (HPbarSlider != null)
        {
            HPbarSlider.maxValue = maxHp;
            HPbarSlider.value = maxHp;
        }

        // 시작 시 HP바 숨기기 (원한다면)
        HPBarParent.SetActive(false);
    }

    void Update()
    {
        //  실제 HP 값에 맞춰 슬라이더의 Value를 부드럽게 업데이트
        if (HPbarSlider != null && HPbarSlider.value > currentHp)
        {
            // 현재 Value를 목표값(currentHp)까지 부드럽게 감소
            HPbarSlider.value = Mathf.MoveTowards(HPbarSlider.value, currentHp, HealthBarDecreaseSpeed * Time.deltaTime);
        }

        if (currentHp <= 0 && !isDie)
        {
            Die();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            currentHp = 1;
        }

    }
    public void skillcool()
    {
        if(Lv <= 1)
        {
            return;
        }

        float cooldown = Random.Range(minCool,maxCool);

        Invoke("Skill", cooldown);
    }

    public void Skill()
    {
        if(Lv == 2|| Lv == 3)
        {
            BossAnim.SetTrigger("skill");
            
        }
        if(Lv == 4)
        {
            int h = Random.Range(0,3);

            if(h == 0)
            {
                BossAnim.SetTrigger("skill1");
            }
            else if(h == 1)
            {
                BossAnim.SetTrigger("skill2");
            }
            else
            {
                BossAnim.SetTrigger("skill3");
            }
        }
    }

    public void LV2Skill()
    {
        Instantiate(skillOj, point.position, Quaternion.identity);
        skillcool();
    }

    public void LV3Skill()
    {
        StartCoroutine(gamespeed());
    }
    public void LV4Skill1()
    {
        // 현재 카메라의 원래 위치를 저장
        originalPos = Camera.transform.localPosition;

        // 흔들림 효과 시작 (흔들림 강도 0.1, 지속 시간 0.5초로 설정)
        StartCoroutine(Shake(0.2f, 2f));
    }
    public void LV4Skill2_1()
    {
        originalPos = new Vector3(0,-2,0); // 현재 위치를 원본으로 저장

        // 오른쪽(X축 양의 방향)으로 밀기
        Vector3 targetPos = originalPos + Vector3.right * pushDistance;
        StartCoroutine(MoveCameraSmoothly(targetPos, moveDuration,false));
    }
    public void LV4Skill2_2()
    {
        Vector3 targetPos = originalPos + Vector3.left * pushDistance;
        StartCoroutine(MoveCameraSmoothly(targetPos, moveDuration, false));
    }
    public void LV4skill2_3()
    {
        StartCoroutine(MoveCameraSmoothly(originalPos, moveDuration,true));
    }
    public void Lv4skill1_1()
    {
        originalPos = Center.transform.localPosition; // 현재 위치를 원본으로 저장

        // 오른쪽(X축 양의 방향)으로 밀기
        Vector3 targetPos = originalPos + Vector3.left * kill3Dis;
        StartCoroutine(MoveCameraSmoothly(targetPos, killDur, false));
    }
    public void Lv4skill3_2()
    {
        StartCoroutine(MoveCameraSmoothly(originalPos, killDur, true));
    }
    public void Hit(int damage) //  대미지를 인수로 받도록 개선
    {
        if (isDie) return;

        currentHp -= damage;

        //  체력바의 시각적 감소는 Update()에서 자동으로 처리됩니다.

        BossAnim.SetTrigger("Hit");

        //  피격 시 HP바를 활성화하는 로직은 코루틴으로 처리 (일정 시간 후 숨기기)
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine); // 기존 코루틴 중지
        }
        damageCoroutine = StartCoroutine(ShowHPBarTemporarily());
    }

    public IEnumerator gamespeed()
    {
        // 1. 랜덤 값 설정 (0 또는 1)
        //  CS0104 오류 해결을 위해 UnityEngine.Random 사용
        int r = UnityEngine.Random.Range(0, 2);

        // 목표 속도 및 지속 시간 설정
        // 0.7f와 1.3f로 유지
        float targetTimeScale = (r == 1) ? 1.3f : 0.7f;
        float duration = 1.5f; // 속도를 변화시키는 데 걸리는 시간 (서서히)

        float resetTime = (r == 1) ? 13 : 7; // 속도를 유지하는 시간

        // 2. 게임 속도와 오디오 피치를 목표치까지 서서히 변경
        float timeElapsed = 0f;
        float startScale = Time.timeScale;
        float startPitch = BGM.pitch; //  현재 오디오 피치 저장

        // duration 시간 동안 목표 속도까지 Lerp로 부드럽게 변화
        while (timeElapsed < duration)
        {
            float lerpValue = timeElapsed / duration;
            // Lerp를 사용하여 시작 속도에서 목표 속도로 보간
            Time.timeScale = Mathf.Lerp(startScale, targetTimeScale, lerpValue);
            // AudioListener.pitch도 Time.timeScale과 동일하게 보간
            BGM.pitch = Mathf.Lerp(startPitch, targetTimeScale, lerpValue);

            timeElapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 정확성을 위해 최종 속도/피치를 목표치로 설정
        Time.timeScale = targetTimeScale;
        BGM.pitch = targetTimeScale; //  오디오 피치도 목표치로 설정

        // 3. 목표 속도를 resetTime 동안 유지
        yield return new WaitForSeconds(resetTime);

        // 4. 게임 속도와 오디오 피치를 다시 1배(원래 속도)로 서서히 되돌리기
        startScale = Time.timeScale; // 현재 속도에서 시작
        startPitch = BGM.pitch; //  현재 피치에서 시작
        timeElapsed = 0f;

        // duration 시간 동안 1.0f까지 Lerp로 부드럽게 변화
        while (timeElapsed < duration)
        {
            float lerpValue = timeElapsed / duration;
            // 현재 속도에서 1.0f로 보간
            Time.timeScale = Mathf.Lerp(startScale, 1.0f, lerpValue);
            //  AudioListener.pitch도 1.0f로 보간
            BGM.pitch = Mathf.Lerp(startPitch, 1.0f, lerpValue);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 정확성을 위해 최종 속도/피치를 1.0f로 설정
        Time.timeScale = 1.0f;
        BGM.pitch = 1.0f; // 오디오 피치도 1.0f로 복구

        skillcool();
    }
    private IEnumerator Shake(float maxMagnitude, float duration)
    {
        float elapsed = 0.0f;

        // 흔들림이 시작될 때의 최대 강도를 저장합니다.
        float currentMagnitude = maxMagnitude;

        // duration(지속 시간) 동안 반복
        while (elapsed < duration)
        {
            // 1. 경과 시간에 따라 현재 강도를 계산합니다.
            // 시간이 지날수록 (elapsed / duration) 비율이 0에서 1로 증가합니다.
            // Lerp(시작값, 끝값, 비율)을 사용해 maxMagnitude에서 0까지 부드럽게 감소시킵니다.
            currentMagnitude = Mathf.Lerp(maxMagnitude, 0f, elapsed / duration);

            // 2. 현재 감쇠된 강도를 사용하여 랜덤 떨림 값 계산
            float x = Random.Range(-1f, 1f) * currentMagnitude;
            float y = Random.Range(-1f, 1f) * currentMagnitude;

            // 3. 카메라 위치를 설정
            Camera.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime; // 경과 시간 업데이트

            yield return null; // 다음 프레임까지 대기
        }

        // 흔들림이 끝나면 카메라 위치를 원본 위치로 정확히 되돌립니다.
       Camera.transform.localPosition = originalPos;

        skillcool();
    }
    private IEnumerator MoveCameraSmoothly(Vector3 targetPos, float duration, bool hr)
    {
        float timeElapsed = 0f;
        Vector3 startPos = Center.transform.localPosition;

        while (timeElapsed < duration)
        {
            // Lerp를 사용하여 시작 위치에서 목표 위치까지 부드럽게 보간
            Center.transform.localPosition = Vector3.Lerp(startPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 정확성을 위해 최종 위치를 목표 위치로 설정
        Center.transform.localPosition = targetPos;
        if(hr == true)
        {
            skillcool();
        }
    }
    //  체력바를 보여주고 일정 시간 후 숨기는 코루틴
    public IEnumerator ShowHPBarTemporarily()
    {
        if (HPBarParent != null)
        {
            HPBarParent.SetActive(true);
        }

        // 실제 슬라이더 값이 currentHp와 같아질 때까지 기다립니다.
        yield return new WaitUntil(() => HPbarSlider.value <= currentHp);

        // 값 동기화 후, 1초 기다립니다.
        yield return new WaitForSeconds(1f);

        if (HPBarParent != null)
        {
            HPBarParent.SetActive(false);
        }
        damageCoroutine = null; // 코루틴 참조 해제
    }

    public void Die()
    {
        isDie = true;
        currentHp = 0; // 혹시 모를 상황 대비

        //  체력바를 완전히 0으로 설정
        if (HPbarSlider != null)
        {
            HPbarSlider.value = 0;
        }

        BossAnim.SetTrigger("Die"); // 죽음 애니메이션 재생
        if(Lv == 4)
        {
            Invoke("nextLv", 15);
        }
        else
        {
            Invoke("nextLv", 5);                
        }

        
    }

    void nextLv()
    {
        next.SetActive(true);
        Invoke("nextt",5);
    }

    void nextt()
    {
        Lv += 1;
        SceneManager.LoadScene("LV" + Lv);
    }
}


