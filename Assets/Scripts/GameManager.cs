using System;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public float spawnTerm = 5;
    public float fasterEverySpawn = 0.05f;
    public float minSpawnTerm = 1;
    public TextMeshProUGUI scoreText;
    float timeAfterLastSpawn;
    float score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeAfterLastSpawn = 0;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeAfterLastSpawn += Time.deltaTime; // 시간이 지날수록, 업데이트 함수에 들어올수록 timeAfterLastSpawn이 늘어남
        score += Time.deltaTime;

        if (timeAfterLastSpawn > spawnTerm) // timeAfterLastSpawnd이 스폰텀을 넘어가면
        {
            timeAfterLastSpawn -= spawnTerm;
            SpawnEnemy();

            spawnTerm -= fasterEverySpawn;
            if (spawnTerm < minSpawnTerm) // 스폰텀이 민스폰타임보다 적으면, 스폰텀을 민스폰으로 맞춤, 스폰이 점점 빨라지지만 1초보다 빨라지진 않음
            {
                spawnTerm = minSpawnTerm;   
            }
        }

        scoreText.text = ((int)score).ToString();
        // 스코어텍스트는 TextMeshProUGUI 타입인데, 화면에 텍스트를 표시하게 해주는 녀석
        // TextMeshProUGUI타입 변수명.text를 하면 변화 시킬 수 있음
        // ToString으로 타입 변환
    }

    void SpawnEnemy()
    {
        float x = UnityEngine.Random.Range(-10f, 10f);
        float y = UnityEngine.Random.Range(-3.5f, 4.5f);

        GameObject obj = GetComponent<ObjectPool>().Get();
        obj.transform.position = new Vector3(x, y, 0);
        obj.GetComponent<EnemyController>().Spawn(player); // 에너미컨트롤러에게 스폰되는거다 명령, 타겟은 플레이어다
    }

}
