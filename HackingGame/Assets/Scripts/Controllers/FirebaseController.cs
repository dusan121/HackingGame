using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;

public class StringParametarEvent : UnityEvent<string> { }

public class FirebaseController : MonoBehaviour
{
    public static FirebaseController instance;
    private void Awake()
    {
        instance = this;
    }

    public string userId;

    public static StringParametarEvent cloudDataLoaded = new StringParametarEvent();

    void Start()
    {
        Debug.Log("Checking Dependencies");
        var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                ReadData();
            }
            else
            {
                cloudDataLoaded.Invoke("faild");

            }
        }, taskScheduler);
    }

    public void WriteData()
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(userId).SetRawJsonValueAsync(PlayerPrefsController.instance.GetDataForCloudSave());
    }

    public static string databaseResult;

    public void ReadData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users/" + userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                cloudDataLoaded.Invoke("faild");
            }
            else if (task.IsCompleted)
            {
                string resultJson = task.Result.GetRawJsonValue();
                if (string.IsNullOrEmpty(resultJson))
                {
                    cloudDataLoaded.Invoke("new_user");
                }
                else
                {
                    cloudDataLoaded.Invoke(resultJson);
                }
            }
        });
    }
}
