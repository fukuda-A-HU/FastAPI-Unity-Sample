using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace AkunClient
{
    public class Main : MonoBehaviour
    {

        [SerializeField] private string url = "http://127.0.0.1:8000";
        [SerializeField] private Texture2D postTexture;
        [SerializeField] private string postText = "Hello World";

        [ContextMenu("ExecuteGetText")]
        public void ExecuteGetText(){
            StartCoroutine(GetText());
        }

        IEnumerator GetText() {
            UnityWebRequest www = UnityWebRequest.Get(url + "/text");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {
                // 結果をテキストで表示
                Debug.Log(www.downloadHandler.text);

                // または、バイナリデータとして結果を取得します。
                byte[] results = www.downloadHandler.data;

                // バイナリデータをテキストに変換して表示

                Debug.Log("Text Downloaded : "+System.Text.Encoding.UTF8.GetString(results));
            }
        }

        [ContextMenu("ExecutePostText")]
        public void ExecutePostText(){
            StartCoroutine(PostText());
        }
        IEnumerator PostText()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection("text", postText));

            UnityWebRequest www = UnityWebRequest.Post(url + "/text", formData);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Text Post complete!");
            }
        }

        [ContextMenu("ExecuteGetTexture")]
        public void ExecuteGetTexture(){
            StartCoroutine(GetTexture());
        }

        IEnumerator GetTexture() {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url + "/texture");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {
                Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Debug.Log("Texture Downloaded");
            }
        }

        [ContextMenu("ExecutePostTexture")]
        public void ExecutePostTexture(){
            StartCoroutine(PostTexture());
        }

        IEnumerator PostTexture()
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormFileSection("texture", postTexture.EncodeToPNG(), "texture.png", "image/png"));

            UnityWebRequest www = UnityWebRequest.Post(url + "/texture", formData);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Texture Post complete!");
            }
        }

        [ContextMenu("ExecuteGetJson")]
        public void ExecuteGetJson(){
            StartCoroutine(GetJson());
        }

        IEnumerator GetJson() {
            UnityWebRequest www = UnityWebRequest.Get(url + "/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {

                Debug.Log(www.downloadHandler.text);

                Data data = JsonUtility.FromJson<Data>(www.downloadHandler.text);

                Debug.Log("Json Data : "+data.id+" "+data.name+" "+data.value);
            }
        }

        [ContextMenu("ExecutePostJson")]
        public void ExecutePostJson(){
            StartCoroutine(PostJson());
        }

        IEnumerator PostJson()
        {
            Data data = new Data();
            data.id = 1;
            data.name = "Akun";
            data.value = 3.14f;

            string json = JsonUtility.ToJson(data);

            UnityWebRequest www = new UnityWebRequest(url + "/json", "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Json Post complete!");
            }
        }
    }
}