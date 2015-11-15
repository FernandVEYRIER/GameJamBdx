using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenMap))]
public class TerrainEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		GenMap myScript = (GenMap)target;
		if(GUILayout.Button("Build Object"))
		{
			myScript.Build();
		}

		if(GUILayout.Button("Destroy Object"))
		{
			myScript.DestroyTerrain();
		}
	}
}
