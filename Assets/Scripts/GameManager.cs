using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{

    public GameObject obstacle;
    public Transform spawnPoint;
    int score = 0;

    public TextMeshProUGUI scoreText;
    public GameObject playButton;
    public GameObject player;
    public GameObject ground;
    public GameObject tree;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            float waitTime = Random.Range(0.75f, 2f);
            yield return new WaitForSeconds(waitTime);
            Instantiate(obstacle, spawnPoint.position, Quaternion.identity);
        }
    }

        IEnumerator SpawnTree()
    {
        while (true)
        {
            float waitTime = 1;
            yield return new WaitForSeconds(waitTime);
            Instantiate(tree, spawnPoint.position, Quaternion.identity);
        }
    }

    void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void GameStart()
    {
        player.SetActive(true);
        playButton.SetActive(false);
        ground.SetActive(true);
        tree.SetActive(true);
        StartCoroutine("SpawnObstacles");
        StartCoroutine("SpawnTree");
        InvokeRepeating("ScoreUp", 2f, 1f);
        music.Play();
    }

}
