using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ModelQuad))]
public class TerrainEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		ModelQuad myScript = (ModelQuad)target;
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
