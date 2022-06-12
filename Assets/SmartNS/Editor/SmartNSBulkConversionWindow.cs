using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GraviaSoftware.SmartNS.SmartNS.Editor
{
    public class SmartNsBulkConversionWindow : EditorWindow
    {
        [MenuItem("Window/SmartNS/Bulk Namespace Conversion...")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            SmartNsBulkConversionWindow window = (SmartNsBulkConversionWindow)EditorWindow.GetWindow(typeof(SmartNsBulkConversionWindow));
            window.titleContent = new GUIContent("Bulk Namespace Converter");
            window.Show();
        }

        private string baseDirectory = "";
        private bool isProcessing = false;
        private List<string> assetsToProcess;
        private int progressCount;

        private string scriptRootSettingsValue;
        private string prefixSettingsValue;
        private string universalNamespaceSettingsValue;
        private bool useSpacesSettingsValue;
        private int numberOfSpacesSettingsValue;
        private string directoryDenyListSettingsValue;
        private bool enableDebugLogging;

        private HashSet<string> ignoredDirectories;

        private static string GetClickedDirFullPath()
        {
            if (Selection.assetGUIDs.Length > 0)
            {
                var clickedAssetGuid = Selection.assetGUIDs[0];
                var clickedPath = AssetDatabase.GUIDToAssetPath(clickedAssetGuid);
                var clickedPathFull = Path.Combine(Directory.GetCurrentDirectory(), clickedPath);

                FileAttributes attr = File.GetAttributes(clickedPathFull);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    // This is a directory. Return it.
                    return clickedPath;
                }
                else
                {
                    // Strip off the file name.
                    var lastForwardSlashIndex = clickedPath.LastIndexOf('/');
                    var lastBackSlashIndex = clickedPath.LastIndexOf('\\');

                    if (lastForwardSlashIndex >= 0)
                    {
                        return clickedPath.Substring(0, lastForwardSlashIndex);
                    }
                    else if (lastBackSlashIndex >= 0)
                    {
                        return clickedPath.Substring(0, lastBackSlashIndex);
                    }
                }
            }

            return null;
        }

        private void OnGUI()
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                baseDirectory = GetClickedDirFullPath();
            }

            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                baseDirectory = "Assets";
            }

            GUILayout.Label("SmartNS Bulk Namespace Conversion", EditorStyles.boldLabel);

            int yPos = 20;
            GUI.Box(new Rect(0, yPos, position.width, 220), @"This tool will automatically add or correct the namespaces on any C# scripts in your project, making them consistent with your SmartNS settings.

BE CAREFUL!

This is a potentially destrucive tool. It will modify the actual file contents on your script, possibly incorrectly. There is no 'Undo'. Don't use this tool unless you have an easy way to revert the changes it makes, such as version control.

See the Documentation.txt file for more information on this. But in general, you probably shouldn't run this on 3rd-party code you got from the asset store.");

            yPos += 220;

            GUI.Box(new Rect(0, yPos, position.width, 100), @"Instructions:
 - Click the 'Base Directory' button to choose the base directory. Only scripts in, or under, that directory will be processed.
 - The click 'Begin Namespace Conversion'
 - Look in the Unity Console log for errors and other information on the progress.");

            yPos += 100;

            var baseDirectoryLabel = new GUIContent(string.Format("Base Directory: {0}", baseDirectory), "SmartNS will search all scripts in, or below, this directory. Use this to limit the search to a subdirectory.");

            if (GUI.Button(new Rect(3, yPos, position.width - 6, 20), baseDirectoryLabel))
            {
                var fullPath = EditorUtility.OpenFolderPanel("Choose root folder", baseDirectory, "");
                baseDirectory = fullPath.Replace(Application.dataPath, "Assets").Trim();
                if (string.IsNullOrWhiteSpace(baseDirectory))
                {
                    baseDirectory = "Assets";
                }
            }

            yPos += 30;

            if (!isProcessing)
            {
                var submitButtonContent = new GUIContent("Begin Namespace Conversion", "Begin processing scripts");
                var submitButtonStyle = new GUIStyle(GUI.skin.button);
                submitButtonStyle.normal.textColor = new Color(0, .5f, 0);
                if (GUI.Button(new Rect(position.width / 2 - 350 / 2, yPos, 350, 30), submitButtonContent, submitButtonStyle))
                {
                    string assetBasePath = (string.IsNullOrWhiteSpace(baseDirectory) ? "Assets" : baseDirectory).Trim();
                    if (!assetBasePath.EndsWith("/"))
                    {
                        assetBasePath += "/";
                    }

                    assetsToProcess = GetAssetsToProcess(assetBasePath);

                    if (EditorUtility.DisplayDialog("Are you sure?",
                        string.Format("This will process a total of {0} scripts found in or under the '{1}' directory, updating their namespaces based on your current SmartNS settings. You should back up your project before doing this, in case something goes wrong. Are you sure you want to do this?", assetsToProcess.Count, assetBasePath),
                        string.Format("I'm sure. Process {0} scripts", assetsToProcess.Count),
                        "Cancel"))
                    {
                        var smartNsSettings = SmartNsSettings.GetSerializedSettings();
                        scriptRootSettingsValue = smartNsSettings.FindProperty("m_ScriptRoot").stringValue;
                        prefixSettingsValue = smartNsSettings.FindProperty("m_NamespacePrefix").stringValue;
                        universalNamespaceSettingsValue = smartNsSettings.FindProperty("m_UniversalNamespace").stringValue;
                        useSpacesSettingsValue = smartNsSettings.FindProperty("m_IndentUsingSpaces").boolValue;
                        numberOfSpacesSettingsValue = smartNsSettings.FindProperty("m_NumberOfSpaces").intValue;
                        directoryDenyListSettingsValue = smartNsSettings.FindProperty("m_DirectoryIgnoreList").stringValue;
                        enableDebugLogging = smartNsSettings.FindProperty("m_EnableDebugLogging").boolValue;

                        // Cache this once now, for performance reasons.
                        ignoredDirectories = SmartNs.GetIgnoredDirectories();

                        progressCount = 0;
                        isProcessing = true;
                    }
                }
            }

            if (isProcessing)
            {
                var cancelButtonContent = new GUIContent("Cancel", "Cancel script conversion");
                var cancelButtonStyle = new GUIStyle(GUI.skin.button);
                cancelButtonStyle.normal.textColor = new Color(.5f, 0, 0);
                if (GUI.Button(new Rect(position.width / 2 - 50 / 2, yPos, 50, 30), cancelButtonContent, cancelButtonStyle))
                {
                    isProcessing = false;
                    progressCount = 0;
                    AssetDatabase.Refresh();
                    Log("Cancelled");
                }

                yPos += 40;

                if (progressCount < assetsToProcess.Count)
                {
                    EditorGUI.ProgressBar(new Rect(3, yPos, position.width - 6, 20), (float)progressCount / (float)assetsToProcess.Count, string.Format("Processing {0} ({1}/{2})", assetsToProcess[progressCount], progressCount, assetsToProcess.Count));
                    Log("Processing " + assetsToProcess[progressCount]);

                    SmartNs.UpdateAssetNamespace(assetsToProcess[progressCount],
                        scriptRootSettingsValue,
                        prefixSettingsValue,
                        universalNamespaceSettingsValue,
                        useSpacesSettingsValue,
                        numberOfSpacesSettingsValue,
                        directoryDenyListSettingsValue,
                        enableDebugLogging,
                        directoryIgnoreList: ignoredDirectories);

                    progressCount++;
                }
                else
                {
                    // We done.
                    isProcessing = false;
                    ignoredDirectories = null;
                    progressCount = 0;
                    AssetDatabase.Refresh();
                    Debug.Log("Bulk Namespace Conversion complete.");
                }
            }
        }

        private List<string> GetAssetsToProcess(string assetBasePath)
        {
            var ignoredDirectories = SmartNs.GetIgnoredDirectories();

            Func<string, bool> isInIgnoredDirectory = (assetPath) =>
            {
                var indexOfAsset = Application.dataPath.LastIndexOf("Assets");
                var fullFilePath = Application.dataPath.Substring(0, indexOfAsset) + assetPath;
                var fileInfo = new FileInfo(fullFilePath);
                return ignoredDirectories.Contains(fileInfo.Directory.FullName);
            };

            return AssetDatabase.GetAllAssetPaths()
                    .Where(s => s.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                        // We ALWAYS require that the scripts be within Assets, regardless of anything else. We don't want to clobber Packages, for example.
                        && s.StartsWith("Assets", StringComparison.OrdinalIgnoreCase)
                        && s.StartsWith(assetBasePath, StringComparison.OrdinalIgnoreCase)
                        && !isInIgnoredDirectory(s)).ToList();
        }

        private void Update()
        {
            if (isProcessing)
            {
                // Without this, we don't get updates every frame, and the whole window just creeps along.
                Repaint();
            }
        }

        private void Log(string message)
        {
            Debug.Log(string.Format("[SmartNS] {0}", message));
        }
    }
}