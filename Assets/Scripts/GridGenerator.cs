using UnityEngine;
using System.Collections;

public class GridGenerator : MonoBehaviour {
	
	int texSize = 64;
	Material material;
	Renderer renderer; 

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
		Debug.Log("Grid started");
		Texture2D tex = new Texture2D(texSize, texSize);

		//LineDrawer.DrawLine(tex, 0, 0, 64, 64, Color.black);
		LineDrawer.DrawGrid(tex, 7);
		tex.Apply();
		Debug.Log("Grid finished");
		Debug.Log(renderer.material.mainTexture);
		renderer.material.mainTexture = tex;
		Debug.Log(renderer.material.mainTexture);

	
	}
	// Update is called once per frame
	void Update () {
	
	}
}
