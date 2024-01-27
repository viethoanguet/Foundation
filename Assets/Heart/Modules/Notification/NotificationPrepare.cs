using System.Collections;
using System.Collections.Generic;
using System.IO;
using Pancake.Apex;
using UnityEngine;
using UnityEngine.Networking;

namespace Pancake.Notification
{
    [HideMonoScript]
    [EditorIcon("script_noti")]
    public class NotificationPrepare : GameComponent
    {
#if UNITY_ANDROID
        [Array, SerializeField] private ScriptableNotificationVariable[] notificationVariables;

        private void Start()
        {
            var strs = new List<string>();

            foreach (var variable in notificationVariables)
            {
                if (!variable.bigPicture) continue;
                if (!strs.Contains(variable.namePicture)) strs.Add(variable.namePicture);
            }

            foreach (string s in strs)
            {
                App.StartCoroutine(PrepareImage(Application.persistentDataPath, s));
            }
        }

        private IEnumerator PrepareImage(string destDir, string filename)
        {
            string path = Path.Combine(destDir, filename);
            if (File.Exists(path)) yield break;
            using var uwr = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, filename));
            yield return uwr.SendWebRequest();
            File.WriteAllBytes(path, uwr.downloadHandler.data);
        }
#endif
    }
}