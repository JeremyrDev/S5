using UnityEngine;
using System.Collections;

public class ObjectScript : MonoBehaviour {
	public Vector3 endPos;
	public float speed = 2;
	public float baseSpeed = 4;
	SceneController sc;
	GameObject p;
	public bool start = false;
	public Material regularMaterial;
	public Material passedMaterial;
	public Material hitMaterial;
	public Material BONUSMaterial;
	bool added = false;
	void Start()
	{
		p = GameObject.Find("Camera");
		sc = p.GetComponent<SceneController>();
	}
	void OnEnable()
	{
		start = true;
		endPos = new Vector3(transform.position.x, -10, -1);
		gameObject.renderer.material = regularMaterial;
		gameObject.rigidbody.isKinematic = true;
		gameObject.rigidbody.useGravity = false;
		gameObject.collider.enabled = true;
		gameObject.transform.eulerAngles = new Vector3(270, 0, 0);
		gameObject.tag = "object";
		if(transform.position.x == 0){
			transform.localScale = new Vector3(.88f, 1,.04f);
		}
		else{
			transform.localScale = new Vector3(.58f, 1,.14f);
		}
		if(sc.fireBONUS){
			gameObject.renderer.material = BONUSMaterial;
			gameObject.tag = "BONUS";
		}
	}
	void Destroy()
	{
		gameObject.SetActive(false);
	}
	void Update()
	{
		if(sc.intense)
		{
			if(!added)
			{
				baseSpeed +=1;
				added = true;
			}

			if(transform.position.x != 0)
				speed = sc.objectSpeed;
			else
				speed=8;
		}
		else
		{
			speed = 8;
			added = false;
		}
		if(start){
			transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);
		}
		if (transform.position == endPos || transform.position.y < -10 || transform.position.y > 10)
		{
			if(gameObject.tag != "BONUS")
				sc.spawnReturned++;
			Destroy();
		}
	}
	void OnMouseDown()
	{
		if(sc.lives > 0){
			sc.score -= 10;
			gameObject.rigidbody.isKinematic = false;
			gameObject.rigidbody.useGravity = true;
			gameObject.rigidbody.AddExplosionForce(1500, new Vector3(transform.position.x, transform.position.y-2, transform.position.z+2),  15);
			gameObject.collider.enabled = false;
			gameObject.renderer.material = hitMaterial;
			sc.lives--;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			//sc.scoreString = "GAME OVER";
			if(gameObject.tag == "BONUS")
			{
				gameObject.rigidbody.isKinematic = false;
				gameObject.rigidbody.useGravity = true;
				gameObject.rigidbody.AddExplosionForce(3500, new Vector3(other.transform.position.x, other.transform.position.y-2, other.transform.position.z+2),  15);
				gameObject.collider.enabled = false;
				gameObject.renderer.material = regularMaterial;
				sc.BONUSReturned = true;
				sc.spawnReturned++;
			}
			else
			{
				gameObject.rigidbody.isKinematic = false;
				gameObject.rigidbody.useGravity = true;
				gameObject.rigidbody.AddExplosionForce(1500, new Vector3(other.transform.position.x, other.transform.position.y-2, other.transform.position.z+2),  15);
				gameObject.collider.enabled = false;
				gameObject.renderer.material = hitMaterial;
				sc.lives--;
			}
		}
		if (other.tag == "p")
		{
			//sc.scoreString = "GAME OVER";
			Debug.Log("-BlOCK PASSED-");
			sc.blockCounter+=5;
			sc.blockCounterTotal+=5;
			if(gameObject.tag == "BONUS" && !sc.BONUSReturned){
				sc.BONUSReturned = true;
				sc.spawnReturned++;
			}
			gameObject.renderer.material = passedMaterial;
			//sc.scoreString = sc.score.ToString();
			//Time.timeScale = 0;
		}
	}
}
