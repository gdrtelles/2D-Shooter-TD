using UnityEngine;
using System.Collections;

public class GUIsystem : MonoBehaviour {


	// Update is called once per frame
	void OnGUI() {
		GUILayout.BeginArea(new Rect(Screen.width/2 - 100, Screen.height/2 , 200, 400));
		if(GUI.Button(new Rect(50,100,80,20), "play"))
		{
		  	 Application.LoadLevel("prototype");
		}
		GUILayout.EndArea();
	}
}
