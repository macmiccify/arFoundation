using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedMultiImageManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f,0.1f,0.1f);

    [SerializeField]
    private GameObject[] trackedPrefabs;

    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button dismissButton;

    private ARTrackedImageManager m_trackedImageManager;
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);
        m_trackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach (GameObject arObject in trackedPrefabs)
        {
            GameObject newClone = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newClone.name = arObject.name;
            arObjects.Add(arObject.name, newClone);
        }
    }
        private void Dismiss() => menuPanel.SetActive(false);

    private void OnEnable()
    {
        m_trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    private void OnDisable()
    {
        m_trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (var trackedImage in eventArgs.removed)
        {
            arObjects[trackedImage.name].SetActive(false);
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position);
        Debug.Log($"trackedImage.referenceImage.name: {trackedImage.referenceImage.name}");
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if(trackedPrefabs !=null)
        {
            arObjects[name].SetActive(true);
            arObjects[name].transform.position = newPosition;
            arObjects[name].transform.localScale = scaleFactor;
            foreach(GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if(go.name !=name)
                {
                    go.SetActive(false);
                }
            }


        }
    }

}
