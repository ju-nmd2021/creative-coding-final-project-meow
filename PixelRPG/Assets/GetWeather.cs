using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;

class FetchIp
{
    public string ip;
}

public class GetWeather : ScriptableObject
{
    public void GetWeatherConditions(MonoBehaviour classname, System.Action<string> action)
    {
        classname.StartCoroutine(MakeGetRequest("https://api.ipify.org?format=json", (json) => {
            FetchIp jsonIp = JsonUtility.FromJson<FetchIp>(json);
            // print(jsonIp.ip);
            classname.StartCoroutine(MakeGetRequest("https://ipapi.co/" + jsonIp.ip + "/latlong/", (str) => {
                // print(str);
                classname.StartCoroutine(MakeGetRequest("https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" + str + "?key=A4DHEJ7LFM5PWY24SH8BACW22&contentType=json",
                    action));
            }));
        }));
    }


    public static IEnumerator MakeGetRequest(string url, System.Action<string> action = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Request was successful, and you can access the response.
               // Debug.Log("Response: " + webRequest.downloadHandler.text);
               if (action != null)
                {
                    action(webRequest.downloadHandler.text);
                }
            }
            else
            {
                // Request failed; handle the error.
                Debug.LogError("Error: " + webRequest.error);
            }
        }
    }
}

public class IPManager
{
    public static string GetIP(ADDRESSFAM Addfam)
    {
        //Return null if ADDRESSFAM is Ipv6 but Os does not support it
        if (Addfam == ADDRESSFAM.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string output = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
#endif 
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    //IPv4
                    if (Addfam == ADDRESSFAM.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (Addfam == ADDRESSFAM.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return output;
    }
}

public enum ADDRESSFAM
{
    IPv4, IPv6
}