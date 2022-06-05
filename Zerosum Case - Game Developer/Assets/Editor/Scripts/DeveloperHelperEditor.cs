using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeveloperHelperEditor : EditorWindow
{
    private string _inputName = "";

    [MenuItem("Helpers/Developer Helper")]
    private static void RenameSelected()
    {
        EditorWindow developerHelper = GetWindow<DeveloperHelperEditor>("Developer Helper");
        developerHelper.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select multiple objects that you want to rename.");

        int numOfSelectedObjects = Selection.gameObjects.Length;
        if(numOfSelectedObjects > 0)
        {
            _inputName = EditorGUILayout.TextField("Object name: ", Selection.activeGameObject.name);

            for (int index = 0; index < numOfSelectedObjects; index++)
            {
                Selection.gameObjects[index].name = _inputName;
            }
        }

        Repaint();
    }
}
