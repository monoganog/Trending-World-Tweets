////////////////////////////////////////////////////////////////////////////
// bl_SocialCountEditor
//
//
//                    Lovatto Studio 2016
////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(bl_SocialCounter))]
public class bl_SocialCountEditor : Editor
{
    public static AnimBool showFacebook;
    public static AnimBool showTwitter;
    public static AnimBool showYoutube;
    public static AnimBool showInstagram;
    public static AnimBool showGoogle;
    public static AnimBool showGithub;
    private GUISkin skin = null;

    public void OnEnable()
    {
        skin = Resources.Load("SocialCounterSkin") as GUISkin;

        showFacebook = new AnimBool(false, Repaint);
        showTwitter = new AnimBool(false, Repaint);
        showYoutube = new AnimBool(false, Repaint);
        showInstagram = new AnimBool(false, Repaint);
        showGoogle = new AnimBool(false, Repaint);
        showGithub = new AnimBool(false, Repaint);

    }

    public override void OnInspectorGUI()
    {
        bl_SocialCounter myTarget = (bl_SocialCounter)target;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Global", EditorStyles.boldLabel);
        myTarget.UpdateEach = EditorGUILayout.Slider("Update Each:", myTarget.UpdateEach, 10, 9999);
        myTarget.HideOnNonInternet = EditorGUILayout.ToggleLeft("Hide On Non Internet?", myTarget.HideOnNonInternet, EditorStyles.toolbarButton);
        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Social Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(skin.FindStyle("header"));
        Rect foldoutRect = EditorGUILayout.GetControlRect();
        showFacebook.target = EditorGUI.Foldout(foldoutRect, showFacebook.target,"Facebok", true);
        EditorGUILayout.EndVertical();

        using (var typeGroup = new EditorGUILayout.FadeGroupScope(showFacebook.faded))
        {
            if (typeGroup.visible)
            {
                EditorGUILayout.BeginVertical(skin.FindStyle("content"));
                myTarget.Facebook_ID = EditorGUILayout.TextField("Facebook URL:", myTarget.Facebook_ID);
                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical(skin.FindStyle("header"));
        Rect foldoutRect2 = EditorGUILayout.GetControlRect();
        showTwitter.target = EditorGUI.Foldout(foldoutRect2, showTwitter.target, "Twitter", true);
        EditorGUILayout.EndVertical();

        using (var typeGroup = new EditorGUILayout.FadeGroupScope(showTwitter.faded))
        {
            if (typeGroup.visible)
            {
                EditorGUILayout.BeginVertical(skin.FindStyle("content"));
                myTarget.Twitter_User = EditorGUILayout.TextField("Twitter User", myTarget.Twitter_User);
                EditorGUILayout.EndVertical();
            }
        }


        EditorGUILayout.BeginVertical(skin.FindStyle("header"));
        Rect foldoutRect3 = EditorGUILayout.GetControlRect();
        showYoutube.target = EditorGUI.Foldout(foldoutRect3, showYoutube.target, "Youtube", true);
        EditorGUILayout.EndVertical();

        using (var typeGroup = new EditorGUILayout.FadeGroupScope(showYoutube.faded))
        {
            if (typeGroup.visible)
            {
                EditorGUILayout.BeginVertical(skin.FindStyle("content"));
                myTarget.ChannelID = EditorGUILayout.TextField("Youtube Channel ID", myTarget.ChannelID);
                myTarget.YoutubeKey = EditorGUILayout.TextField("Youtube API Key", myTarget.YoutubeKey);
                EditorGUILayout.EndVertical();
            }
        }


        EditorGUILayout.BeginVertical(skin.FindStyle("header"));
        Rect foldoutRect4 = EditorGUILayout.GetControlRect();
        showInstagram.target = EditorGUI.Foldout(foldoutRect4, showInstagram.target, "Instagram", true);
        EditorGUILayout.EndVertical();

        using (var typeGroup = new EditorGUILayout.FadeGroupScope(showInstagram.faded))
        {
            if (typeGroup.visible)
            {
                EditorGUILayout.BeginVertical(skin.FindStyle("content"));
                myTarget.Instagram_UserID = EditorGUILayout.TextField("Instagram Username", myTarget.Instagram_UserID);
                myTarget.Instagram_AcessToken = EditorGUILayout.TextField("Instagram Access Toke", myTarget.Instagram_AcessToken);
                EditorGUILayout.EndVertical();
            }
        }


        EditorGUILayout.BeginVertical(skin.FindStyle("header"));
        Rect foldoutRect5 = EditorGUILayout.GetControlRect();
        showGoogle.target = EditorGUI.Foldout(foldoutRect5, showGoogle.target, "Google Plus", true);
        EditorGUILayout.EndVertical();

        using (var typeGroup = new EditorGUILayout.FadeGroupScope(showGoogle.faded))
        {
            if (typeGroup.visible)
            {
                EditorGUILayout.BeginVertical(skin.FindStyle("content"));
                myTarget.GooglePlus_User = EditorGUILayout.TextField("GooglePlus ID", myTarget.GooglePlus_User);
                myTarget.GooglePlus_Key = EditorGUILayout.TextField("GooglePlus API Key", myTarget.GooglePlus_Key);
                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.BeginVertical(skin.FindStyle("header"));
        Rect foldoutRect6 = EditorGUILayout.GetControlRect();
        showGithub.target = EditorGUI.Foldout(foldoutRect6, showGithub.target, "Github", true);
        EditorGUILayout.EndVertical();

        using (var typeGroup = new EditorGUILayout.FadeGroupScope(showGithub.faded))
        {
            if (typeGroup.visible)
            {
                EditorGUILayout.BeginVertical(skin.FindStyle("content"));
                myTarget.Github_User = EditorGUILayout.TextField("Github Username", myTarget.Github_User);
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(myTarget);
        }
    }

}