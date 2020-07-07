using UnityEngine;
using UnityEditor;

//Useful for testing and debuging, allows to raise events 
//during playmode inside the inspector
[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEvent e = target as GameEvent;

        if (GUILayout.Button("Raise"))
        {
            e.Raise();
        }
    }
}
