using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController: MonoBehaviour
{
    public float speed = 3; // 인스펙터에서도 바꿀 수 있는데, 인스펙터가 우선 순위. 여기서 바꾼다고 수정 안됨. 언리얼하고 다른 점
    public GameObject bulletPerfab; // 프리팹의 자료형은 게임오브젝트임 타입@!!!\

    public Material flashMaterial;
    public Material defaultMaterial;

    Vector3 move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // 첫번째 프레임 업데이트 전에 스타트라는 메소드가 불리워진다.
    void Start()
    {
        
    }

    // Update is called once per frame
    // 매 프레임마다 업데이트라는 메소드가 불리워진다.
    void Update()
    {
         move = Vector3.zero; // 0,0,0 제로 벡터로 들어감

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) 
        {
           move += new Vector3(-1, 0, 0);
        }

        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            move += new Vector3(0, -1, 0);
        }

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            move += new Vector3(1, 0, 0);
        }

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            move += new Vector3(0, 1, 0);
        }

        move = move.normalized; // 노말라이즈는 어떤 벡터의 길이를 1로 만들어줌 원래는 루트2

        if (move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true; // x가 0보다 작다. 왼쪽으로 움직이면 flipx가 트루
        }
        if (move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // x가 0보다 크다. 오른쪽으로 움직이면 flipx가 펄스
        }

        // 애니메이션 작동
        if (move.magnitude > 0) // move.magnitude가 움직이느냐 마느냐 0보다 크면 움직인다
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
        else // 매그니튜드가 0이면 움직이지 않음
        {
            GetComponent<Animator>().SetTrigger("Stop");
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) 
            // if (Input.GetMouseButtonDown(0)) 입력 받으면 총알 발사, 겟마우스버튼다운은 눌리는 순간, (0은) 좌클릭을 뜻함 -> 예전 api라 안쓴대
            // 
        {
            Shoot();
        }

        //if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) // Input.GetKey는 어떠한 키가 눌려있는 상태일 때 참을 돌려주는 함수
        //                                     // KeyCode.는 어떠한 키를 지정하려고 할 때 만들어져 있는 enum 타입
        //{
        //    transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0)); 
        //    // Time.deltaTime은 업데이트가 지난 번 불려지고 그 다음에 불리워질 때까지의 시간 갭을 의미함
        //    // transform이라는 것은 이미 MonoBehaviour에 정의돼있는 멤버 변수
        //    // Transform 타입의 변수, Translate라는 함수가 미리 준비돼 있음 - 얼만큼 움직여라 라는 뜻
        //}

        //if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) 
        //{
        //    transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        //}

        //if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        //{
        //    transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        //}

        //if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        //{
        //    transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        //}
    }

    void Shoot()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue);
        worldPos -= (transform.position + new Vector3(0, -0.5f, 0));
        // transform.position은 플레이어 자체의 위치


        GameObject newBullet = GetComponent<ObjectPool>().Get();

        if (newBullet != null)
        {
            newBullet.transform.position = transform.position + new Vector3(0, -0.5f); // 현재 플레이어 위치로 지정
            newBullet.GetComponent<Bullet>().Direction = worldPos;
            // 뉴불릿이라는 게임 오브젝트 안에 직접 만든 불릿 컴포넌트가 붙어 있음, 붙이고 방향을 설정
        }


        //GameObject newBullet = Instantiate<GameObject>(bulletPerfab);
        // 설계도를 가지고 게임 오브젝트를 실제로 만들어서 하이어라키 창에 등장하게 만들어야함.. 
        // 그때 사용하는게 인스턴시에이트, Instantiate<GameObject> 게임오브젝트를 인스턴시에이트한다 라는 뜻
        // 뒤에 프리팹을 써줘야함. 불러와야하는 건 불릿프리팹
        // 여기 뒤에 트랜스폼을 붙일 수 있게 돼 있는데, 하이어라키 상에서 부모를 지정해 줄 거냐 아니냐
        // 인스턴시에이트는 찍어내는거다보니 플레이 중 렉이 걸릴 수 있음, 가능하면 최소화해야함
    }

    private void FixedUpdate() // 물리학적 계산이 들어갈 부분에서는 따로 처리
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    // 이즈트리거가 아닌 두 오브젝트가 충돌했을 때 발생하는 함수
    // 불릿같은 경우 OnTriggerEnter2D 함수와 Collider2D, 지금은 OnCollisionEnter2D에 Collision2D이 들어옴
    // 콜라이더는 내가 충돌한 상대방의 콜라이더를 가져오고, 콜리전은 충돌 정보 그 자체를 가져옴
    {
        //Debug.Log("Hit");
        if (collision.gameObject.tag == "Enemy") // 콜리전 게임오브젝트의 태그가 에너미라면
        {
            if (GetComponent<Character>().Hit(1))
            {
                Flash();
            }
            else
            {
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
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 1.5f); //다른 함수를 일정시간이 지난 다음에 호출하도록 만드는 함수
                                    //""에 스트링으로 함수 이름 치고 뒤에 숫자 적으면 됨
    }
    void AfterDying()
    {
        //gameObject.SetActive(false); 
        SceneManager.LoadScene("GameOverScene");
    }
}
