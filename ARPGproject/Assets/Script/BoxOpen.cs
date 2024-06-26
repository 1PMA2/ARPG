using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxOpen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform boxUp;
    [SerializeField] private Transform room;

    List<GameObject> monsters = new List<GameObject>();
    void Start()
    {
        
    }

    private void OnEnable()
    {
        int randomX = Random.Range(-5, 6);
        int randomY = Random.Range(0, 360);
        int randomZ = Random.Range(-5, 6);
        Vector3 randomPos = new Vector3(randomX, 0.94f, randomZ);

        transform.position = room.position + randomPos;
        transform.rotation = Quaternion.Euler(0, randomY, 0);
    }

    private void OnDisable()
    {
        monsters.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            UIManager.Instance.ActiveItemUI(true);
            room.GetComponent<Room>().CloseDoorUp();
            StartCoroutine(BoxLidOpen());
            Invoke(nameof(InvokeSummonMonsterCoroutine), 0.2f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator BoxLidOpen()
    {
        float elapsedTime = 0f;
        float lidAngle = boxUp.localRotation.eulerAngles.x;
       

        while (elapsedTime < 0.5f)
        {
            float lerp = Mathf.Lerp(lidAngle, 45, elapsedTime / 0.5f);
            boxUp.localRotation = Quaternion.Euler(new Vector3(lerp, 0, 0));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        boxUp.localRotation = Quaternion.Euler(new Vector3(45, 0, 0));
    }

    private void InvokeSummonMonsterCoroutine()
    {
        StartCoroutine(SummonMonster(5));
    }

    private IEnumerator SummonMonster(int num)
    {
        int count = 0;
        Vector3 randomPos;
        int randomX;
        int randomY;
        int randomZ;

        while (count < num)
        {
            randomX = Random.Range(-7, 8);
            randomY = Random.Range(0, 360);
            randomZ = Random.Range(-7, 8);
            //summon effect
            randomPos = new Vector3(randomX, 0f, randomZ);

            GameObject monster = MonsterManager.Instance.GetMonster(room.position + randomPos, Quaternion.Euler(0, randomY, 0));
            monsters.Add(monster);

            UnitChecker unitChecker = monster.GetComponent<UnitChecker>();
            unitChecker.OnDisabled += OnMonsterDisabled;

            count++;
            yield return null;
        }

        count = 0;
        StopCoroutine(SummonMonster(num));
    }

    private void OnMonsterDisabled()
    {
        if (monsters.TrueForAll(monster => monster == null || !monster.gameObject.activeSelf))
        {
            foreach (var monster in monsters)
            {
                if (monster != null)
                {
                    monster.GetComponent<UnitChecker>().OnDisabled -= OnMonsterDisabled;
                }
            }

            if (room != null)
                room.GetComponent<Room>().OpenDoor();

            monsters.Clear();


        }
    }

}
