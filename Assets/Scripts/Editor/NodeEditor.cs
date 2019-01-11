using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// This is the window editor script.
/// Where all the magic happens.
/// </summary>
/// 

//This is needed to make each new window unique
//Without this every window would have the same object/script attached to it
[System.Serializable]
public class Window
{
    public Rect Rect;
    public Nodes script = null;
    public Window ParentWindows = null;
    public List<Window> ChildWindow = new List<Window>();

    public Window(Rect a_r)
    {
        Rect = a_r;
    }
}
//This was needed to save the connection but it does not save them correctly
[System.Serializable]
public class Rect
{
    public float x;
    public float y;
    public float width;
    public float height;

    public Rect(float x, float y, float width, float height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public Rect(UnityEngine.Rect rec)
    {
        x = rec.x;
        y = rec.y;
        width = rec.width;
        height = rec.height;
    }

    public UnityEngine.Rect UERect()
    {
        return new UnityEngine.Rect(x, y, width, height);
    }
}
//Rect is not saveable data, so it needs to be converted into floats
//This allows for the windows to stay as they are in the window when it's closed
//It does NOT save the object in each window. It saves connections but no correctly
[System.Serializable]
public class WindowSave
{
    public float x;
    public float y;
    public float w;
    public float h;
    //public Nodes script;
    public Window pW;
    public List<Window> cW;
}


[System.Serializable]
public class NodeEditor : EditorWindow
{
    private Nodes script;
    private Agent agent;
    private GameObject penguin;
    private string scriptname;
    private Window selectedNode;
    Vector3 mousePosition;
    bool clickedOnWindow;
    GUIStyle guiStyle = new GUIStyle();
    GUIStyle notActiveStyle = new GUIStyle();

    List<Window> windows = new List<Window>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedWindows = new List<int>();

    //Creates a menu in Unity at the top
    //This is how you open the window
    [MenuItem("BT Editor/Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
    }

    //Enums for the switch statement, making it easier to choose which state to use
    public enum UserActions
    {
        addNode, addTransitionNode, deleteNode
    }

    private void OnEnable()
    {
        penguin = GameObject.Find("Penguin");
        agent = penguin.GetComponent<Agent>();
        //guiStyle.normal.background = MakeTex(1, 1, new Color(1, 0, 0, 1));
        //notActiveStyle.normal.background = MakeTex(1, 1, new Color(1, 1, 1, 1));
    }

    private void Awake()
    {
        //Load everything on Awake instead of OnEnable as that would also load the windows
        //when the editor is played
        FileStream fs = new FileStream("save.txt", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        List<WindowSave> wsList = bf.Deserialize(fs) as List<WindowSave>;
        fs.Close();

        foreach (var w in wsList)
        {
            windows.Add(new Window(new Rect(w.x, w.y, w.w, w.h)));
            windows[windows.Count - 1].ChildWindow = w.cW;
            windows[windows.Count - 1].ParentWindows = w.pW;
            // DrawNodeCurve(new Rect(w.pW.Rect.x, w.pW.Rect.y, w.pW.Rect.width, w.pW.Rect.height), new Rect(w.cW));
            //script = (Nodes)EditorGUILayout.ObjectField(w.script, typeof(Nodes), true);
        }

    }

    //Save everything when the Window is closed
    private void OnDisable()
    {
        List<WindowSave> wsList = new List<WindowSave>();
        foreach (var w in windows)
        {
            WindowSave ws = new WindowSave();
            ws.x = w.Rect.x;
            ws.y = w.Rect.y;
            ws.w = w.Rect.width;
            ws.h = w.Rect.height;
            ws.pW = w.ParentWindows;
            ws.cW = w.ChildWindow;
            //ws.script = w.script;
            wsList.Add(ws);
        }


        FileStream fs = new FileStream("save.txt", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, wsList);
        fs.Close();
    }

    void OnGUI()
    {
        if(agent == null)
        {
            penguin = GameObject.Find("Penguin");
            agent = penguin.GetComponent<Agent>();
        }

        //This is used to set the colour of the window
        guiStyle.normal.background = MakeTex(1, 1, new Color(1, 0, 0, 1));
        notActiveStyle.normal.background = MakeTex(1, 1, new Color(1, 1, 1, 1));

        Event e = Event.current;
        mousePosition = e.mousePosition;
        UserInput(e);

        //Allows for the attachment of two windows
        if (windowsToAttach.Count == 2)
        {
            windows[windowsToAttach[0]].ParentWindows = windows[windowsToAttach[1]];

            if (!windows[windowsToAttach[1]].ChildWindow.Contains(windows[windowsToAttach[0]]))
            {
                windows[windowsToAttach[1]].ChildWindow.Add(windows[windowsToAttach[0]]);
            }

            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach = new List<int>();
        }

        //Draws the connection to the windows
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i] != null)
            {
                for (int j = 0; j < windows[i].ChildWindow.Count; j++)
                {
                    DrawNodeCurve(windows[i].Rect.UERect(), windows[i].ChildWindow[j].Rect.UERect());
                }
                if (windows[i].ParentWindows != null)
                {
                    //Handles.color = Color.blue;
                    //DrawNodeCurve(windows[i].Rect, windows[i].ParentWindows.Rect);
                    // Handles.color = Color.black;
                }
            }
        }

        //if (attachedWindows.Count >= 2)
        //{
        //    for (int i = 0; i < attachedWindows.Count; i += 2)
        //    {
        //        //if (windows[attachedWindows[i]].Rect != null && windows[attachedWindows[i + 1]].Rect != null)
        //        //{
        //            //DrawNodeCurve(windows[attachedWindows[i]].Rect, windows[attachedWindows[i + 1]].Rect);
        //        //}
        //    }
        //}


        BeginWindows();

        //if (GUILayout.Button("Create Node"))
        //{
        //    windows.Add(new Window
        //    {
        //        Rect = new Rect(10, 10, 200, 200),
        //        script = null
        //    });
        //}

        //Creates a window
        for (int i = 0; i < windows.Count; i++)
        {
            //GUI.color = (1 == 1) ? Color.red : Color.cyan;
            windows[i].Rect = new Rect(GUI.Window(i, windows[i].Rect.UERect(), DrawNodeWindow,"", GetStyle(windows[i])));
        }
        Repaint();
        EndWindows();
    }

    //What should be seen when the window is created
    void DrawNodeWindow(int id)
    {
        AddScript(id);
        GUI.DragWindow();
    }

    //This function is the interaction with the BT
    //Checks whether the active state enum is the same
    //And if their bool is true, the window colour will change to show that the state is active
    private GUIStyle GetStyle(Window a_w)
    {
        if (a_w.script != null)
        {
           if(Application.isPlaying && agent != null && agent.activeStates.type == a_w.script.type)
           {
                //agent.activeStates = a_w.script;
                return guiStyle;

           }
           if (a_w.script.isActive)
           {
               return guiStyle;
           }
        }
        return notActiveStyle;
    }

    //User inputs, mouse 1 is left click
    //This will open a menu to select an option
    void UserInput(Event e)
    {
        if (e.button == 1)
        {
            if (e.type == EventType.MouseDown)
            {
                RightClick(e);
            }
        }

        if (e.button == 0)
        {
            if (e.type == EventType.MouseDown)
            {

            }
        }
    }

    //If you right click on the main window it will bring up a menu to select to create a new window
    //If the mouse is over said new window it will allow you to delete the new window
    void RightClick(Event e)
    {
        clickedOnWindow = false;
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i].Rect.UERect().Contains(e.mousePosition))
            {
                selectedNode = windows[i];
                clickedOnWindow = true;
                break;
            }
        }

        if (!clickedOnWindow)
        {
            AddNewNode(e);
        }
        else
        {
            ModifyNode(e);
        }
    }

    //This creates new node menu
    void AddNewNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Node"), false, ContextCallBack, UserActions.addNode);
        menu.ShowAsContext();
        e.Use();
    }

    //This creates the delete node menu
    void ModifyNode(Event e)
    {
        GenericMenu menu = new GenericMenu();
        if(clickedOnWindow)
        {
            menu.AddItem(new GUIContent("Delete"), false, ContextCallBack, UserActions.deleteNode);
        }
        menu.ShowAsContext();
        e.Use();

    }

    //This is called when a item is added or deleted
    void ContextCallBack(object o)
    {
        UserActions a = (UserActions)o;
        switch(a)
        {
            //This will create a new window on the mouse position
            case UserActions.addNode:
                windows.Add(new Window(new Rect(mousePosition.x, mousePosition.y, 200, 200)));
                break;

            //This will delete the specific node that the mouse is hovered over
            case UserActions.deleteNode:
                Window target = selectedNode;
                windows.Remove(target);
                for (int i = 0; i < attachedWindows.Count; i++)
                {
                    attachedWindows.RemoveAt(i);
                    if(target.ParentWindows != null)
                    {
                        target.ParentWindows.ChildWindow.Remove(target);
                    }
                }
                break;
        }
    }

    //This is called when the window is created
    //Such a the title and the object field
    void AddScript(int id)
    {
        EditorGUILayout.BeginVertical();
        {
            //If an object is present then it use that as it's title
            //If not then default too No GameObject
            if (windows[id].script != null)
            {
                EditorGUILayout.LabelField(windows[id].script.name, EditorStyles.boldLabel);
            }
            else
            {
                EditorGUILayout.LabelField("No GameObject", EditorStyles.boldLabel);
            }

            //When this button is pressed it allows for linking to nodes together
            //Another windows 'Attach' button must be clicked to link them
            if (GUILayout.Button("Attach"))
            {
                windowsToAttach.Add(id);
            }

            EditorGUILayout.BeginHorizontal();

            //This is the section which allows for objects to be entered into.
            //Unfortunately it was a struggle to get it to just read from a script
            //So the script must be on a prefab to work. There's other reasons too
            GUILayout.Label("Script");
            script = (Nodes)EditorGUILayout.ObjectField(windows[id].script, typeof(Nodes), true);
            if (script != windows[id].script)
            {
                windows[id].script = script;
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    //I did not write this I obtained it from https://forum.unity.com/threads/simple-node-editor.189230/
    //This forum page helped me get a better grasp on writing a window
    void DrawNodeCurve(UnityEngine.Rect start, UnityEngine.Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.6f);

        for (int i = 0; i < 3; i++)
        {
            // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }

    //Also obtained from the Unity forums https://forum.unity.com/threads/giving-unitygui-elements-a-background-color.20510/
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for (int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}
