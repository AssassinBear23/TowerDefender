using UnityEngine;


public partial class StaticMethods
{
    /// <summary>
    /// Quits the application. If running in the Unity editor, stops playing. If running in a build, quits the application.
    /// </summary>
    public static void QuitApplication()
    {
        // If we are running in the editor, stop playing and if we are running in a build, quit the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
