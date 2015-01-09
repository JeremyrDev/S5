using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SceneController : MonoBehaviour 
{
	public GameObject pauseOverlay;
	public GameObject o;
	List<GameObject> objects;

	public float objectSpeed = 8;
	public float score=0;
	public float highScore = 0;
	public float totalScore = 0;
	float timeBetweenSpawn = .75f;
	float miliseconds = 0;
	float seconds = 5;
	float spawnPosition = 0;
	float spawnTimer = 0;
	float timeBetweenSpawnStore = 1;
	float timeLimit = 5;

	public int lives = 3;
	public int blockCounter = 0;
	public int blockCounterTotal =0 ;
	int spawnCounter = 0;
	public int spawnReturned = 0;
	public int waveCounter = 1;
	int lc = 0;
	int rc = 0;
	int timeLimitBase = 5;

	public bool wait = false;
	public bool BONUSFired = false;
	public bool BONUSReturned = false;
	public bool fireBONUS = false;
	public bool gameOverBool = false;
	public bool intense = false;

	string milisecondString;
	string scoreString;
	string info;

	public GUIText endScoreText;
	public GUIText highScoreText;

	public GUIStyle infoStyle;
	public GUIStyle scoreStyle;
	public GUIStyle timeStyle;

	void OnGUI()
	{
		GUI.Label (new Rect (0,0,100,50), info, infoStyle);
		GUI.Label(new Rect(0, 0, Screen.width,0), blockCounterTotal.ToString(), scoreStyle);
		GUI.Label(new Rect(0, 0, Screen.width, 0),milisecondString, timeStyle);
	}

	
	void Start () 
	{
		totalScore = PlayerPrefs.GetFloat("Score");
		highScore = PlayerPrefs.GetFloat("HighScore");
		Time.timeScale = 1;
		endScoreText.enabled = false;
		highScoreText.enabled = false;
		objects = new List<GameObject>();
		for(int i = 0; i<20; i++)
		{
			GameObject obj = (GameObject)Instantiate(o);
			obj.SetActive(false);
			objects.Add(obj);
		}
	}
	public void GameOver()
	{
		gameOverBool = true;
		pauseOverlay.SetActive(true);
		Time.timeScale = 0.1f;
		blockCounterTotal+=blockCounter;
		endScoreText.text = blockCounterTotal.ToString();
		if(blockCounterTotal>highScore)
			highScore = blockCounterTotal;
		totalScore+=blockCounterTotal;
		PlayerPrefs.SetFloat("HighScore", highScore);
		PlayerPrefs.SetFloat("Score", totalScore);
		highScoreText.text = highScore.ToString()+"\n"+ totalScore;
		endScoreText.enabled = true;
		highScoreText.enabled = true;
	}
	// Update is called once per frame
	void Update () 
	{

		if(!gameOverBool){
			if(lives<=0)
				GameOver();
//			info = seconds + "." +(miliseconds*100).ToString("f0")+"\n"+
//				spawnPosition+"\n"+
//				lc+":"+rc+"\n"+
//				lives+"\n"+
//				blockCounter+":"+blockCounterTotal+"\n"+
//				spawnReturned+":"+spawnCounter+"\n"+
//					timeBetweenSpawn;
			info = seconds + "." +(miliseconds*100).ToString("f0") + "\n"+
				timeBetweenSpawn+ "\n"+
					spawnReturned+":"+spawnCounter;
			if(!wait){
				miliseconds -= 1 * Time.deltaTime;
				spawnTimer +=1*Time.deltaTime;
			}
			if (miliseconds <= 0)
			{
				if (seconds == 0)
				{
					seconds = timeLimit;
					wait = true;
				}
				miliseconds = .99f;
				seconds -= 1;
			}
			if(spawnTimer >= timeBetweenSpawn) // spawn Timer
			{
				if(!BONUSFired)
					Spawn();
				spawnTimer = 0 ;
			}
			if(wait && spawnReturned == spawnCounter){
				if(BONUSReturned){
					wait = false;
					BONUSFired = false;
					BONUSReturned = false;
					fireBONUS = false;
					spawnCounter = 0;
					spawnReturned = 0;
					waveCounter++;
					if(intense)
					{
						intense = false;
						timeBetweenSpawn = .5f;
						timeLimit = 5;
						objectSpeed = 8;
					}
					else
					{
						intense = true;
						timeBetweenSpawnStore-=.05f;
						timeBetweenSpawn = timeBetweenSpawnStore;
						timeLimitBase+=1;
						timeLimit = timeLimitBase;
						seconds = timeLimitBase;
						objectSpeed = waveCounter+8;
					}
				}
				else
				{
					fireBONUS = true;
					Spawn();
				}
		}
		}
	}
	public void Spawn()
	{
		if(!BONUSFired){
			int t = Random.Range(0, 2);
			if(t==0)
			{
	//			int l = 3;
	//			if(rc>=2)
	//				l=1;
				lc++;
				rc=0;
				if(lc>2){
					spawnPosition = 2f;
					lc=0;
				}
				else
					spawnPosition = -2f;
			}
			if(t==1)
			{
	//			int l = 3;
	//			if(lc>=2)
	//				l=1;
				rc++;
				lc=0;
				if(rc>2)
					spawnPosition = -2f;
				else
					spawnPosition = 2f;
			}
			for(int i = 0; i < 20;i++)
			{
				if(!objects[i].activeInHierarchy)
				{
					if(fireBONUS)
						objects[i].transform.position = new Vector3(0, 10, -1);
					else
						objects[i].transform.position = new Vector3(spawnPosition, 10, -1);
					objects[i].SetActive(true);
					if(spawnPosition == 0){
						if(!BONUSFired)
							Spawn();
					}
					if(fireBONUS){
						BONUSFired = true;
						fireBONUS = false;
					}
					spawnCounter++;
					return;
				}
			}
		}
	}
}
