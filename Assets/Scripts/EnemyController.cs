using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Spawning,
        Moving,
        Dying
    }

    public float speed = 2;

    public Material flashMaterial;
    public Material defaultMaterial;

    GameObject target;
    State state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //target = GameObject.Find("Player"); // 하이어라키창에서 플레이어라는 오브젝트를 찾아서 돌려줌, 안쓰는게 좋음 효율이 구림
        //state = State.Moving;
    }

    public void Spawn(GameObject target)
    {
        this.target = target;
        state = State.Spawning;
        GetComponent<Character>().Initialize();
        GetComponent<Animator>().SetTrigger("Spawn");
        Invoke("StartMoving", 1);
        GetComponent<Collider2D>().enabled = false; // 스폰되는 과정에서 콜라이더 꺼줌 닿아도 데미지 안닳게
    }

    void StartMoving()
    {
        GetComponent<Collider2D>().enabled = true;
        state = State.Moving;
    }

    private void FixedUpdate()
    {
        if ( state == State.Moving)
        {
            Vector2 direction = target.transform.position - transform.position;
            // 타겟은 플레이어 오브젝트, 트랜스폼에서 위치를 가져옴
            // 타겟의 위치 벡터에서 내 현재의 위치를 빼면 내가 움직여야하는 방향이 나옴
            // 그 방향으로 움직이게 트렌스 레이트
            transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);

            if (direction.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            if (direction.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) // 콜라이더투디 콜리전은 충돌된 상대의 정보가 담겨있음
    {
        if (collision.tag == "Bullet")
        {
            float d = collision.gameObject.GetComponent<Bullet>().damage;
            // 게임오브젝트는 트리거를 발생시킨 상대 게임오브젝트를 불러옴
            // 불릿 컴포넌트 데미지를 가져옴

            if (GetComponent<Character>().Hit(d)) // 캐릭터의 히트의 데미지 호출
            {
                // 살아 있을 때
                Flash();
            }
            else
            {
                // 죽었을 때
                Die();
            }
        }
    }

    void Flash()
    {
        GetComponent<SpriteRenderer>().material = flashMaterial;
        Invoke("AfterFlash", 0.3f);
    }

    void AfterFlash()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    void Die()
    {
        state = State.Dying;
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 1.5f); //다른 함수를 일정시간이 지난 다음에 호출하도록 만드는 함수
                                    //""에 스트링으로 함수 이름 치고 뒤에 숫자 적으면 됨
    }
    void AfterDying()
    {
        gameObject.SetActive(false); // this는 에너미컨트롤러, gameobject는 에너미컨트롤러가 붙어있는 그 오브젝트 자체
    }
}
