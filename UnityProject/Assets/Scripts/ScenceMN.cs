using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenceMN : MonoBehaviour
{
    [SerializeField] Text text;
    bool isLoaded = false;
    [SerializeField] bool isTest = false;
    void Start()
    {
        if (isTest)
        {
            Screen.SetResolution(1080, 2160, false);
            print("Screen.SetResolution");
        }
        text.text = "แตะที่ใดก็ได้ เพื่อไปต่อ";
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !isLoaded)
        {
            isLoaded = true;
            StartCoroutine(LoadSceneEI());
        }
    }

    IEnumerator LoadSceneEI()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        text.gameObject.GetComponent<Animator>().enabled = false;

        while (!operation.isDone)
        {
            Debug.Log("Load progress:" + operation.progress);
            text.text = "กำลังโหลด...(" + (operation.progress*100).ToString("00") + "%)";
            yield return null;
        }
    }
}
