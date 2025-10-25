using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scoz.Func;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets;
using UnityEngine.XR;

public class SelectTrainSceneUI : BaseUI
{
    public static SelectTrainSceneUI Instance { get; private set; }

    private AsyncOperationHandle<SceneInstance> handle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectCave()
    {
        Debug.Log("Cave");
        OpenScene("TrainCave");
    }

    public void SelectHunt()
    {
        Debug.Log("Hunt");
        OpenScene("TrainHunt");
    }

    public void SelectRock()
    {
        Debug.Log("Rock");
        OpenScene("TrainRock");
    }

    public void SelectVigor()
    {
        Debug.Log("Vigor");
        OpenScene("TrainVigor");
    }

    void OpenScene(string sceneName)
    {
        handle = Addressables.LoadSceneAsync("Assets/AddressableAssets/Scenes/" + sceneName + ".unity", LoadSceneMode.Single);
        handle.Completed += OnSceneLoaded;
    }

    void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {

    }
    
    private void OnDestroy()
    {
        Addressables.UnloadSceneAsync(handle);
    }

    public override void RefreshText()
    {
        
    }

    protected override void SetInstance()
    {
        Instance = this;
    }
}
