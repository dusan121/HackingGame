using UnityEditor;
using UnityEngine;
 
[InitializeOnLoad]
public class CustomHierarchy : MonoBehaviour
{
    private static Vector2 offset = new Vector2(0, 2);
    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static bool keyDown = false;
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        // Space toggle

        Event e = Event.current;

        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.Space))
                    {
                        keyDown = true;
                    }
                    break;
                }
            case EventType.KeyUp:
                {
                    if (Event.current.keyCode == (KeyCode.Space) && keyDown)
                    {
                        keyDown = false;
                        ToogleSelection();
                    }
                    break;
                }
        }
    }
    static void ToogleSelection()
    {
        GameObject[] go = Selection.gameObjects;

        if(go.Length==0)
            return;
        for(int i=0;i<go.Length;i++)
        {
            Undo.RecordObject(go[i],"toogle active");
            go[i].SetActive(!go[i].activeSelf);
        }
    }
}
