using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
using Mapbox.Unity.Map;
using TMPro;
public class InstagramAPIIntegration : MonoBehaviour
{
    public GameObject instagramGameobejctprefab;
    public AbstractMap map;
    public float radius = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchInstagramPicture());
    }

    IEnumerator FetchInstagramPicture()
    {
        Vector3 centerLocationUnit = map.GeoToWorldPosition(map.CenterLatitudeLongitude);


        string url = "https://api.instagram.com/v1/users/self/media/recent/?access_token=1189340012.924d248.506408fc7a8046a5ab3d8f47cab80293";

        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string apiResponse = webRequest.downloadHandler.text;
                IDictionary apiParse = (IDictionary)Json.Deserialize(apiResponse);
             
                IList apiInstagramPicturesList = (IList)apiParse["data"];

                foreach (IDictionary instagramPicture in apiInstagramPicturesList)
                {
                    IDictionary images = (IDictionary)instagramPicture["images"];
                    IDictionary standardResolation = (IDictionary)images["standard_resolution"];
                    string mainPicUrl = (string)standardResolation["url"];

                    IDictionary location = (IDictionary)instagramPicture["location"];
                    if (location != null)
                    {
                        double latitude = (double)location["latitude"];
                        double longitude = (double)location["longitude"];
                        string placeName = (string)location["name"];
                        Vector3 instaPicPos = map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(latitude, longitude));

                        if (Mathf.Abs(Vector3.Distance(centerLocationUnit, instaPicPos)) < radius)
                        {
                            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(mainPicUrl))
                            {
                                yield return www.SendWebRequest();
                                if (webRequest.isNetworkError || webRequest.isHttpError)
                                {
                                    Debug.Log(webRequest.error);
                                }
                                else
                                {
                                    var texture = DownloadHandlerTexture.GetContent(www);
                                    GameObject instagramPic = Instantiate(instagramGameobejctprefab);
                                    instagramPic.transform.Find("mainPicture").GetComponent<MeshRenderer>().material.mainTexture = texture;
                                    instagramPic.transform.position = map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(latitude, longitude)) + new Vector3(0, 0.3f, 0);
                                    instagramPic.transform.SetParent(map.transform);

                                    IDictionary user = (IDictionary)instagramPicture["user"];
                                    string userName = (string)user["username"];
                                    string profilePicUrl = (string)user["profile_picture"];
                                    using (UnityWebRequest profilePicture = UnityWebRequestTexture.GetTexture(profilePicUrl))
                                    {
                                        yield return profilePicture.SendWebRequest();
                                        if (profilePicture.isNetworkError || profilePicture.isHttpError)
                                        {
                                            Debug.Log(profilePicture.error);
                                        }
                                        else
                                        {
                                            var profilePicTexture = DownloadHandlerTexture.GetContent(profilePicture);
                                            instagramPic.transform.Find("profilePicture").GetComponent<MeshRenderer>().material.mainTexture = profilePicTexture;
                                            instagramPic.transform.Find("username").GetComponent<TextMeshPro>().text = userName;
                                            instagramPic.transform.Find("placeName").GetComponent<TextMeshPro>().text = placeName;
                                        }
                                    }


                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
