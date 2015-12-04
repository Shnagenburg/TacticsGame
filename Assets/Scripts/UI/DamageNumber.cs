using UnityEngine;
using System.Collections;

public class DamageNumber : MonoBehaviour {

	public float risingSpeed;
	public float duration;
	private TextMesh textMesh;
	private float timer = 0;


	// Use this for initialization
	void Start () {
		this.textMesh = GetComponent<TextMesh>();
		Destroy(this.gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		this.transform.position += Vector3.up * risingSpeed;

		Color color = textMesh.color;
		color.a = Mathf.Lerp(1.0f, 0.0f, timer / duration);
		textMesh.color = color;
	}

	public void SetNumber(int value) {
		GetComponent<TextMesh>().text = "" + value;
	}
}
