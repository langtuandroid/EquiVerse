using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneOpener : EditorWindow {
    [MenuItem("Scene/Level 1-1")]
    public static void Level11() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Level 1-1.unity");
    }

    [MenuItem("Scene/Level 1-2")]
    public static void Level12() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Level 1-2.unity");
    }
    
    [MenuItem("Scene/Level 1-3")]
    public static void Level13() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Level 1-3.unity");
    }
    
    [MenuItem("Scene/Level 1-4")]
    public static void Level14() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Level 1-4.unity");
    }

    [MenuItem("Scene/Main Menu")]
    public static void MainMenu() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    [MenuItem("Scene/New Companion")]
    public static void NewCompanion() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/NewCompanionScene.unity");
    }
}