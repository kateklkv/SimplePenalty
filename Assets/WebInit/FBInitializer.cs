using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class FBInitializer : MonoBehaviour
{
    [HideInInspector] public FirebaseStatus FbStatus = FirebaseStatus.Waiting;
    
    private string _logText = "";
    private const int KMaxLogSize = 16382;
    private Firebase.DependencyStatus _dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    private void Awake()
    {
        Firebase.FirebaseApp
            .CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                _dependencyStatus = task.Result;

                if (_dependencyStatus == Firebase.DependencyStatus.Available)
                    InitializeFirebase();
                else
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + _dependencyStatus);
            });
    }

    // Initialize remote config, and set the default values.
    private void InitializeFirebase()
    {
        System.Collections.Generic.Dictionary<string, object> defaults =
            new System.Collections.Generic.Dictionary<string, object>();
        defaults.Add("key", "");

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
            .SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                DebugLog("RemoteConfig configured and ready!");
                FetchDataAsync();
            });
    }
    
    public Task FetchDataAsync()
    {
        DebugLog("Fetching data...");
        
        Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);

        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            DebugLog("Fetch canceled.");
            FbStatus = FirebaseStatus.Failed;
        }
        else if (fetchTask.IsFaulted)
        {
            DebugLog("Fetch encountered an error.");
            FbStatus = FirebaseStatus.Failed;
        }
        else if (fetchTask.IsCompleted)
        {
            DebugLog("Fetch completed successfully!");
            FbStatus = FirebaseStatus.Connected;
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
                        DebugLog(String.Format("Remote data loaded and ready (last fetch time {0}).",
                            info.FetchTime));
                    });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        DebugLog("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        DebugLog("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                DebugLog("Latest Fetch call still pending.");
                break;
        }
    }

    public void DebugLog(string s)
    {
        print(s);
        _logText += s + "\n";

        while (_logText.Length > KMaxLogSize)
        {
            int index = _logText.IndexOf("\n");
            _logText = _logText.Substring(index + 1);
        }
    }

    public enum FirebaseStatus
    {
        Waiting = 0,
        Connected = 1,
        Failed = 2
    }
}