using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("컴포넌트 설정")]
    [SerializeField]
    private Animator BossAnim;
    [SerializeField]
    private GameObject[] paturns; // 보스 패턴 오브젝트
    [SerializeField]
    private GameObject HPBarParent; // HP 바를 포함하는 부모 GameObject

    // 이 변수는 Inspector에서 직접 연결하는 것이 좋습니다.
    [SerializeField]
    private Slider HPbarSlider;

    [Header("능력치")]
    public int maxHp = 10;
    public int currentHp; // 변수명을 currentHp로 변경
    public bool isDie = false; // 변수명 isdie를 isDie로 변경

    //  체력바 감소 속도 조절용 변수
    private const float HealthBarDecreaseSpeed = 2f;
    private Coroutine damageCoroutine; // 코루틴 참조 저장용

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

        // BossAnim.SetTrigger("Die"); // 죽음 애니메이션 재생
        // Destroy(gameObject, 3f); // 3초 후 오브젝트 파괴
    }
}


