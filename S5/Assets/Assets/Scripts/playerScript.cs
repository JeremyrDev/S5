using UnityEngine;
using System.Collections;

public class playerScript : MonoBehaviour {

	Vector3 to;
	Vector3 pausedVector;
	float speed = 14;
	public GameObject PauseOverlay;
	SceneController sc;
	GameObject p;
	public bool paused=false;

	public bool left = false;
	public bool right = false;
	
	void Start () 
	{
		to = new Vector3(0f, -6.4f, -1);
		PauseOverlay.SetActive(false);
		p = GameObject.Find("Camera");
		sc = p.GetComponent<SceneController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!sc.gameOverBool){
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				right = false;
				to = new Vector3(-2.2f, -6.4f, -1);
				left = true;
			}
			if(Input.GetKeyUp(KeyCode.LeftArrow))
				if(left)
					to = new Vector3(-2.4f,-6.4f, -1);
			
			if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				to = new Vector3(0, -6.4f, -1);
			}
			
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				left = false;
				to = new Vector3(2.2f, -6.4f, -1);
				right = true;
			}
			if(Input.GetKeyUp(KeyCode.RightArrow))
				if(right)
					to = new Vector3(2.4f, -6.4f, -1);
			if(Input.GetKeyDown(KeyCode.U))
			{
				Time.timeScale+=.5f;
			}
			if(Input.GetKeyDown(KeyCode.I))
			{
				Time.timeScale-=.5f;
			}
			if (Input.GetKeyDown(KeyCode.C))
			{
				PlayerPrefs.SetFloat("Score", 0);
				PlayerPrefs.SetFloat("HighScore", 0);
			}

			if (Input.GetKeyDown(KeyCode.P))
			{
				if (paused)
				{
					paused = false;
					PauseOverlay.SetActive(false);
					Time.timeScale = 1;
					to = pausedVector;
				}
				else
				{
					pausedVector = to;
					paused = true;
					PauseOverlay.SetActive(true);
					Time.timeScale = 0;
				}
			}
			LerpMove();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			PlayerPrefs.SetFloat("Score", 0);
			PlayerPrefs.SetFloat("HighScore", 0);
			Application.LoadLevel(Application.loadedLevel);
		}
		if(Input.GetKeyDown(KeyCode.U))
		{
			Time.timeScale+=.5f;
		}
		if(Input.GetKeyDown(KeyCode.I))
		{
			Time.timeScale-=.5f;
		}
	}
	
	void LerpMove()
	{
		transform.position = Vector3.Lerp(transform.position, to, speed*Time.deltaTime);
	}
	void OnTriggerEnter(Collider c)
	{
		Debug.Log(c.tag);
	}
}
