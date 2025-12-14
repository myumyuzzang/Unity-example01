using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알 하나를 생성한 것임..

    public float speed = 10;
    public float damage = 1;
    Vector2 direction; // 게터와 세터를 가져와서 세팅을 할때 연산하게끔
    public Vector2 Direction
    {
        set
        {
            direction = value.normalized; // 디렉션에다가 들어온 값을 노말라이즈해서 넣겠다 라는 뜻
            // 어느 쪽 방향으로 가려고 벡터를 집어 넣었을 때, 총알에게 방향을 알려주는 것
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.Direction = new Vector2(10, 1); // (10,1) 이라는 벡터 사용, 살짝 위로 나가는 총알 테스트코드
    }

    // Update is called once per frame
    void Update()
    {
        // 업데이트에서 움직여줘야함.
        // 방향에다가 스피드 곱해주고 델타타임을 곱해주면 방향 속도 시간 
        // 특정한 방향 속도로 이동함
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) // 콜라이더 두개가 부딪혔다. 충돌이 시작됐다.
    {
        if (collision.tag == "Wall" || collision.tag == "Enemy") // 콜리전으로 들어온, 즉 나와 지금 부딪힌 상대방의 태그를 알 수 있음. 나는 불릿!
            // 부딪힌게 벽이라면 총알이 없어져야함
        {
            gameObject.SetActive(false); // 불릿이 사용이 끝나면 디스트로이해버렸는데, 원래 위치로 돌려놓음 액티브하지 않은 상태로 돌려줌

            //Destroy(gameObject);
            // 게임 오브젝트가 없어질 땐 디스트로이 사용, gameObject는 이 컴포넌트가 붙어 있는 gameObject 자체를 의미함
            // this는 불릿이라는 인스턴스, gameObject라고 쓰면 이 스크립트가 붙어있는 오브젝트, bullet이 사라짐
        }
    }
}
