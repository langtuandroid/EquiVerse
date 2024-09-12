using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneOpener : EditorWindow {
    [MenuItem("Scene/Levels/Level 1-1")]
    public static void Level11() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level 1-1.unity");
    }

    [MenuItem("Scene/Levels/Level 1-2")]
    public static void Level12() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level 1-2.unity");
    }
    
    [MenuItem("Scene/Levels/Level 1-3")]
    public static void Level13() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level 1-3.unity");
    }
    
    [MenuItem("Scene/Levels/Level 1-4")]
    public static void Level14() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level 1-4.unity");
    }
    
    [MenuItem("Scene/Levels/Level 1-5")]
    public static void Level15() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level 1-5.unity");
    }
    
    [MenuItem("Scene/Levels/Level 2-1")]
        public static void Level21() {
            if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Levels/Level 2-1.unity");
        }

    [MenuItem("Scene/Menus/Main Menu")]
    public static void MainMenu() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Menus/MainMenu.unity");
    }

    [MenuItem("Scene/Menus/New Companion")]
    public static void NewCompanion() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Menus/NewCompanionScene.unity");
    }
    
    [MenuItem("Scene/Menus/ProgressHubScene")]
    public static void CompanionSelection() {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Menus/ProgressHubScene.unity");
    }
}