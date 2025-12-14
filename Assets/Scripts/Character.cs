using UnityEngine;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    public float MaxHp = 3;
    public GameObject HPGauge;
    float HP;
    float HPMaxWidth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HP = MaxHp;

        if (HPGauge != null) // 이프문으로 예외처리, HPGauge가 존재하는지부터 세팅
                             // 캐릭터 컴포넌트는 적에도 존재하는데 플레이어만 세팅해서.
        {
            HPMaxWidth = HPGauge.GetComponent<RectTransform>().sizeDelta.x;
            // 트랜스폼을 가지고올땐 그냥 오브젝트.트랜스폼을 하면 되는데, 렉트트렌스폼 같은 경우 겟컴포넌트로 가져와야함.
            // 사이즈델타라는 멤버 변수가 있음. 사이즈델타의 변수타입은 벡터2, 거기서 x를 가지고 오면 넓이를 가져올 수 있음
        }

    }

    public void Initialize()
    {
        HP = MaxHp;
    }
    /*
     * 살아있으면 true를 리턴한다.
     */
    public bool Hit(float damage) // 맞은 뒤 살아있는지 죽어있는지를 리턴하도록 
    {
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
        }

        if (HPGauge != null) // 적에는 hpgauge가 없으니가 예외처리
        { 
        HPGauge.GetComponent<RectTransform>().sizeDelta = new Vector2(HP / MaxHp * HPMaxWidth, 
            HPGauge.GetComponent<RectTransform>().sizeDelta.y);
            // hp가 변화하면 게이지의 넓이도 변화해야함. 사이즈델타는 사이즈델타.x식으로 설정할 수 없음 = 뉴 벡터2(가로, 세로)로 해줘야함
            // HP/MaxHP를 하면 maxHP에 대한 현재 HP의 비율이 나올거고, 원래의 넓이를 곱해주면 어느정도 넓이로 HPGauge를 세팅해야될지 알 수 있음
        }

        return HP > 0; // hp가 0보다 크면 트루, 작으면 펄스 리턴
    }
}
