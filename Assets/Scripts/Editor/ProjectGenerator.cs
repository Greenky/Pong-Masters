using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ProjectGenerator : EditorWindow {

    [MenuItem("Window/Project Generator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ProjectGenerator>("Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Use ony for empty project!", EditorStyles.boldLabel);
        if(GUILayout.Button("Generate project scructure"))
        {
            System.IO.Directory.CreateDirectory("Assets/3rd-Party");
            System.IO.Directory.CreateDirectory("Assets/Animations");
            System.IO.Directory.CreateDirectory("Assets/Audio");
            System.IO.Directory.CreateDirectory("Assets/Audio/Music");
            System.IO.Directory.CreateDirectory("Assets/Audio/SFX");
            System.IO.Directory.CreateDirectory("Assets/Materials");
            System.IO.Directory.CreateDirectory("Assets/Models");
            System.IO.Directory.CreateDirectory("Assets/Plugins");
            System.IO.Directory.CreateDirectory("Assets/Prefabs");
            System.IO.Directory.CreateDirectory("Assets/Resources");
            System.IO.Directory.CreateDirectory("Assets/Textures");
            System.IO.Directory.CreateDirectory("Assets/Sandbox");
            System.IO.Directory.CreateDirectory("Assets/Scenes");
            System.IO.Directory.CreateDirectory("Assets/Scenes/Levels");
            System.IO.Directory.CreateDirectory("Assets/Scenes/Other");
            System.IO.Directory.CreateDirectory("Assets/Scripts/Tests");
            System.IO.Directory.CreateDirectory("Assets/Shaders");
        }
        if (GUILayout.Button("Generate scene scructure"))
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            new GameObject("Management");
            new GameObject("GUI");
            var cameras = new GameObject("Cameras");
            var camera = new GameObject("Camera");
            camera.transform.parent = cameras.transform;
            camera.AddComponent<Camera>();
            new GameObject("Lights");
            var world = new GameObject("World");
            var terrain = new GameObject("Terrain");
            terrain.transform.parent = world.transform;
            var props = new GameObject("Props");
            props.transform.parent = world.transform;
            new GameObject("_Dynamic");

            EditorSceneManager.SaveScene(scene,"Assets/Scenes/Levels/DefaultScene.unity");
        }
    }
}
