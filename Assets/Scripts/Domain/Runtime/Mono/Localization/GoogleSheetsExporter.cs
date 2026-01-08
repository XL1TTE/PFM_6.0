#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;

public class GoogleSheetsExporter : EditorWindow
{
    private string sheetsUrl = "";
    private string outputPath = "Assets/Resources/Localization";

    [MenuItem("Tools/Localization/Download from Google Sheets")]
    public static void ShowWindow()
    {
        GetWindow<GoogleSheetsExporter>("Google Sheets Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Google Sheets Export Settings", EditorStyles.boldLabel);

        sheetsUrl = EditorGUILayout.TextField("Google Sheets URL:", sheetsUrl);
        outputPath = EditorGUILayout.TextField("Output Path:", outputPath);

        if (GUILayout.Button("Download and Convert"))
        {
            DownloadSheets();
        }

        EditorGUILayout.HelpBox(
            "1. Share your Google Sheet publicly\n" +
            "2. Use format: https://docs.google.com/spreadsheets/d/YOUR_ID/gviz/tq?tqx=out:csv&sheet=SHEET_NAME\n" +
            "3. Separate sheets with #SHEET: marker",
            MessageType.Info);
    }

    private void DownloadSheets()
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                string csvData = client.DownloadString(sheetsUrl);

                LocalizationData data = new LocalizationData();

                ProcessCSVToJson(csvData, data);

                string json = JsonUtility.ToJson(data, true);
                string fullPath = Path.Combine(outputPath, "localization_data.json");

                Directory.CreateDirectory(outputPath);
                File.WriteAllText(fullPath, json);

                AssetDatabase.Refresh();

                Debug.Log($"Localization data saved to: {fullPath}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to download from Google Sheets: {e.Message}");
        }
    }

    private void ProcessCSVToJson(string csvData, LocalizationData data)
    {
    }
}
#endif