using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// This package changed in 2019.1
#if UNITY_2019_1_OR_NEWER

using UnityEngine.UIElements;

#elif UNITY_2018_4_OR_NEWER
using UnityEngine.Experimental.UIElements;
#endif

namespace GraviaSoftware.SmartNS.SmartNS.Editor
{
    // Create a new type of Settings Asset.
    public class SmartNsSettings : ScriptableObject
    {
        public const string ScriptRootTooltip = "Whatever you place here will be stripped off the beginning of the namespace. Normally this should be 'Assets', as Unity will automatically place new scripts in '/Assets'. But if you keep all your scripts in 'Assets/Code', you could out 'Assets/Code' here to strip that out of the namespace. Note that any scripts created at the level of the Script Root will not be given a namespace, unless Universal namespacing is used.";
        public const string NamespacePrefixTooltip = "This will be added to the beginning of the namespace. This is useful for placing the project or company name in your namespace.";
        public const string UniversalNamespaceTooltip = "Instead of using the 'Smart' functionality, based on the current directory, this will place all code into the same namespace you specify here.";
        public const string IndentUsingSpacesTooltip = "Enables the use of spaces for indentation instead of tabs.";
        public const string NumberOfSpacesTooltip = "How many spaces to use per indentation level.";
        public const string UpdateNamespacesWhenMovingScriptsTooltip = "(Experimental) When exabled, SmartNS will run on any scripts you move within your project, updating their namespaces. Note: This doesn't work when moving directories that contain scripts.";
        public const string DirectoryIgnoreListTooltip = "(Experimental) Prevents SmartNS from acting on any scripts located within the following directories, or any child directories. Useful for preventing SmartNS from acting on certain directories, such as where you keep 3rd-party assets.";
        public const string EnableDebugLoggingTooltip = "This turns on some extra logging for SmartNS. Not usually interesting to anyone but the developer.";

#pragma warning disable 0414
        [FormerlySerializedAs("m_ScriptRoot")] [SerializeField]
        private string mScriptRoot;
        [FormerlySerializedAs("m_NamespacePrefix")] [SerializeField]
        private string mNamespacePrefix;
        [FormerlySerializedAs("m_UniversalNamespace")] [SerializeField]
        private string mUniversalNamespace;
        [FormerlySerializedAs("m_IndentUsingSpaces")] [SerializeField]
        private bool mIndentUsingSpaces;
        [FormerlySerializedAs("m_NumberOfSpaces")] [SerializeField]
        private int mNumberOfSpaces;
        //[SerializeField]
        //private string m_DefaultScriptCreationDirectory;
        [FormerlySerializedAs("m_UpdateNamespacesWhenMovingScripts")] [SerializeField]
        private bool mUpdateNamespacesWhenMovingScripts;
        [FormerlySerializedAs("m_DirectoryIgnoreList")] [SerializeField]
        private string mDirectoryIgnoreList;
        [FormerlySerializedAs("m_EnableDebugLogging")] [SerializeField]
        private bool mEnableDebugLogging;
#pragma warning restore 0414

        private const string DefaultSmartNsSettingsDirectoryPath = "Assets/SmartNS";
        private const string DefaultSmartNsSettingsAssetName = "SmartNSSettings.asset";

        internal static SmartNsSettings GetOrCreateSettings()
        {
            var smartNsSettings = GetSmartNsSettingsAsset();

            if (smartNsSettings == null)
            {
                // We don't have any setting. Create one wherever the c# class is.

                smartNsSettings = ScriptableObject.CreateInstance<SmartNsSettings>();
                smartNsSettings.mScriptRoot = "Assets";
                smartNsSettings.mNamespacePrefix = "";
                smartNsSettings.mUniversalNamespace = "";
                smartNsSettings.mIndentUsingSpaces = true;
                smartNsSettings.mNumberOfSpaces = 4;
                //smartNSSettings.m_DefaultScriptCreationDirectory = "";
                smartNsSettings.mUpdateNamespacesWhenMovingScripts = false;
                smartNsSettings.mDirectoryIgnoreList = "";
                smartNsSettings.mEnableDebugLogging = false;

                // Try to create the asset at the default location. If the directory doesn't exist, just put it under Assets.
                string fullAssetPath = "";
                if (AssetDatabase.IsValidFolder(DefaultSmartNsSettingsDirectoryPath))
                {
                    fullAssetPath = Path.Combine(DefaultSmartNsSettingsDirectoryPath, DefaultSmartNsSettingsAssetName);
                }
                else
                {
                    fullAssetPath = Path.Combine("Assets", DefaultSmartNsSettingsAssetName);
                }
                AssetDatabase.CreateAsset(smartNsSettings, fullAssetPath);
                AssetDatabase.SaveAssets();
            }
            return smartNsSettings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }

        public static bool SettingsFileExists()
        {
            return GetSmartNsSettingsAsset() != null;
        }

        private static string GetSettingsFilePath()
        {
            // Although there is a default location for thr Settings, we want to be able to find it even if the
            // player has moved them around. This will locate the settings even if they're not in the default location.
            var smartNsSettingsAssetGuids = AssetDatabase.FindAssets("t:SmartNSSettings");

            if (smartNsSettingsAssetGuids.Length > 1)
            {
                var paths = string.Join(", ", smartNsSettingsAssetGuids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)));
                Debug.LogWarning(string.Format("Multiple SmartNSSettings.asset files exist in this project. This may lead to confusion, as any of the settings files may be chosen arbitrarily. You should remove all but one of the following so that you only have one SmartNSSettings.asset files: {0}", paths));
            }

            if (smartNsSettingsAssetGuids.Length > 0)
            {
                return AssetDatabase.GUIDToAssetPath(smartNsSettingsAssetGuids.First());
            }

            return null;
        }

        public static SmartNsSettings GetSmartNsSettingsAsset()
        {
            SmartNsSettings smartNsSettings = null;

            // Although there is a default location for thr Settings, we want to be able to find it even if the
            // player has moved them around. This will locate the settings even if they're not in the default location.
            var smartNsSettingsAssetGuids = AssetDatabase.FindAssets("t:SmartNSSettings");

            if (smartNsSettingsAssetGuids.Length > 1)
            {
                var paths = string.Join(", ", smartNsSettingsAssetGuids.Select(guid => AssetDatabase.GUIDToAssetPath(guid)));
                Debug.LogWarning(string.Format("Multiple SmartNSSettings.asset files exist in this project. This may lead to confusion, as any of the settings files may be chosen arbitrarily. You should remove all but one of the following so that you only have one SmartNSSettings.asset files: {0}", paths));
            }

            if (smartNsSettingsAssetGuids.Length > 0)
            {
                smartNsSettings = AssetDatabase.LoadAssetAtPath<SmartNsSettings>(AssetDatabase.GUIDToAssetPath(smartNsSettingsAssetGuids.First()));
            }

            var settingsFilePath = GetSettingsFilePath();
            if (settingsFilePath == null)
            {
                return null;
            }
            else
            {
                return AssetDatabase.LoadAssetAtPath<SmartNsSettings>(settingsFilePath);
            }
        }

        // There's no real need for this, given that we auto-create settings either when creating C# files or when opening Project Settings
        //[MenuItem("GameObject/SmartNS/Create SmartNS Settings")]
        //public static void EnsureSmartNSSettings()
        //{
        //    if (SettingsFileExists())
        //    {
        //        EditorUtility.DisplayDialog("Settings Exist", $"A SmartNS settings file already exists at path: {GetSettingsFilePath()}", "OK");
        //    }
        //    else
        //    {
        //        GetOrCreateSettings();
        //        EditorUtility.DisplayDialog("Settings Created", $"Created SmartNS settings file at path: {GetSettingsFilePath()}", "OK");
        //    }
        //}
    }

    // Create SmartNSSettingsProvider by deriving from SettingsProvider:
    public class SmartNsSettingsProvider : SettingsProvider
    {
        private SerializedObject mSmartNsSettings;

        private class Styles
        {
            public static GUIContent ScriptRoot = new GUIContent("Script Root", SmartNsSettings.ScriptRootTooltip);
            public static GUIContent NamespacePrefix = new GUIContent("Namespace Prefix", SmartNsSettings.NamespacePrefixTooltip);
            public static GUIContent UniversalNamespace = new GUIContent("Universal Namespace", SmartNsSettings.UniversalNamespaceTooltip);
            public static GUIContent IndentUsingSpaces = new GUIContent("Indent using Spaces", SmartNsSettings.IndentUsingSpacesTooltip);
            public static GUIContent NumberOfSpaces = new GUIContent("Number of Spaces", SmartNsSettings.NumberOfSpacesTooltip);
            //public static GUIContent DefaultScriptCreationDirectory = new GUIContent("Default Script Creation Directory", "(Experimental) If you specify a path here, any scripts created directly within 'Assets' will instead be created in the folder you specify. (No need to prefix this with 'Assets'.)");
            public static GUIContent UpdateNamespacesWhenMovingScripts = new GUIContent("Update Namespaces When Moving Scripts", SmartNsSettings.UpdateNamespacesWhenMovingScriptsTooltip);
            public static GUIContent DirectoryIgnoreList = new GUIContent("Directory Deny List (One directory per line)", SmartNsSettings.DirectoryIgnoreListTooltip);
            public static GUIContent EnableDebugLogging = new GUIContent("Enable Debug Logging", SmartNsSettings.EnableDebugLoggingTooltip);
        }

        public SmartNsSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope) { }

        public static bool IsSettingsAvailable()
        {
            return SmartNsSettings.SettingsFileExists();
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            // This function is called when the user clicks on the SmartNSSettings element in the Settings window.
            mSmartNsSettings = SmartNsSettings.GetSerializedSettings();
        }

        public override void OnDeactivate()
        {
            AssetDatabase.SaveAssets();
        }

        public override void OnGUI(string searchContext)
        {
            mSmartNsSettings.Update();

            // Use IMGUI to display UI:
            EditorGUILayout.LabelField(string.Format("Version {0}", SmartNs.SmartNsVersionNumber));

            // Preferences GUI
            EditorGUILayout.HelpBox("SmartNS adds a namespace to new C# scripts based on the directory in which they are created. Optionally, a 'Universal' namespace can be used for all scripts.", MessageType.None);
            EditorGUIUtility.labelWidth = 245.0f;

            EditorGUILayout.PropertyField(mSmartNsSettings.FindProperty("m_ScriptRoot"), Styles.ScriptRoot);
            EditorGUILayout.PropertyField(mSmartNsSettings.FindProperty("m_NamespacePrefix"), Styles.NamespacePrefix);
            EditorGUILayout.PropertyField(mSmartNsSettings.FindProperty("m_UniversalNamespace"), Styles.UniversalNamespace);
            var useSpacesProperty = mSmartNsSettings.FindProperty("m_IndentUsingSpaces");
            var useSpaces = EditorGUILayout.PropertyField(useSpacesProperty, Styles.IndentUsingSpaces);
            if (useSpacesProperty.boolValue)
            {
                EditorGUILayout.PropertyField(mSmartNsSettings.FindProperty("m_NumberOfSpaces"), Styles.NumberOfSpaces);
            }
            EditorGUILayout.PropertyField(mSmartNsSettings.FindProperty("m_EnableDebugLogging"), Styles.EnableDebugLogging);

            EditorGUILayout.Space();
            var boldStyle = new GUIStyle();
            boldStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Experimental", boldStyle);
            //EditorGUILayout.PropertyField(m_SmartNSSettings.FindProperty("m_DefaultScriptCreationDirectory"), Styles.DefaultScriptCreationDirectory);
            EditorGUILayout.PropertyField(mSmartNsSettings.FindProperty("m_UpdateNamespacesWhenMovingScripts"), Styles.UpdateNamespacesWhenMovingScripts);

            EditorGUILayout.LabelField(Styles.DirectoryIgnoreList);
            ignoreListScrollPos = EditorGUILayout.BeginScrollView(ignoreListScrollPos, GUILayout.Height(120));
            mSmartNsSettings.FindProperty("m_DirectoryIgnoreList").stringValue = EditorGUILayout.TextArea(mSmartNsSettings.FindProperty("m_DirectoryIgnoreList").stringValue);
            EditorGUILayout.EndScrollView();

            mSmartNsSettings.ApplyModifiedProperties();
        }

        private Vector2 ignoreListScrollPos;

        // Register the SettingsProvider
        [SettingsProvider]
        public static SettingsProvider CreateSmartNsSettingsProvider()
        {
            if (!IsSettingsAvailable())
            {
                // Make sure settings exist.
                SmartNsSettings.GetOrCreateSettings();
            }

            //Debug.Log("Settings Available");
            var provider = new SmartNsSettingsProvider("Project/SmartNS", SettingsScope.Project);

            // Automatically extract all keywords from the Styles.
            provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
            return provider;
        }
    }
}