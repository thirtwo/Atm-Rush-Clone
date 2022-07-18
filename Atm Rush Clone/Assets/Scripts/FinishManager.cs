using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject money;
    [SerializeField] private Transform lastMoney;
    [SerializeField] private List<Color> colors;
    [SerializeField] private List<GameObject> finishCubes;
    private int cubeIndex = 0;
    private GameObject lastCube;
    private CameraFollow cameraFollow;


    private void Start()
    {
        GameManager.OnGameFinished += GameManager_OnGameFinished;
        cameraFollow = FindObjectOfType<CameraFollow>();
    }

    private void GameManager_OnGameFinished(bool isWin)
    {
        if (isWin)
        {
            lastCube = finishCubes[cubeIndex];
            lastCube.transform.position += Vector3.forward * 2;
            lastCube.GetComponent<MeshRenderer>().material.color = colors[cubeIndex % colors.Count];
            StartCoroutine(CoFinishAnim());
        }
    }

    private IEnumerator CoFinishAnim()
    {
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        cameraFollow.target = player.transform;
        cameraFollow.AddOffset(new Vector3(0, 0, -5));
        yield return new WaitForSeconds(0.3f);
        var moneyCount = PlayerPrefs.GetInt("Money");
        for (int i = 0; i < moneyCount; i++)
        {
            var last = Instantiate(money, lastMoney.position, Quaternion.identity);
            lastMoney.position += Vector3.up / 2;
            player.transform.position += Vector3.up / 2;
            if(lastCube.transform.position.y + 3 < player.transform.position.y)
            {
                ++cubeIndex;
                if (cubeIndex > finishCubes.Count) yield break;
                var cube = finishCubes[cubeIndex];
                if(cube != null)
                {
                    lastCube.transform.position += Vector3.forward * 2;
                    lastCube = cube;
                    lastCube.GetComponent<MeshRenderer>().material.color = colors[cubeIndex % colors.Count];
                    lastCube.transform.position += Vector3.back * 2;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
