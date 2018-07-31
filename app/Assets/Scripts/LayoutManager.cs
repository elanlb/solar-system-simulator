using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutManager : MonoBehaviour {

    public List<LayoutGroup> layoutGroups;
    public List<PlatformSpecificText> platformSpecificTexts;

    enum Headset
    {
        VIVE,
        WMR
    }

    Headset headset;

    // Use this for initialization
    void Start() {
        headset = SteamVR.instance.GetStringProperty(Valve.VR.ETrackedDeviceProperty.Prop_ManufacturerName_String).ToLower().Contains("htc") ? Headset.VIVE : Headset.WMR;

        foreach (LayoutGroup lg in layoutGroups)
        {
            setLayout(lg);
        }

        foreach(PlatformSpecificText pst in platformSpecificTexts)
        {
            setPlatformSpecificText(pst);
        }
    }

    void setPlatformSpecificText(PlatformSpecificText pst)
    {
        switch (headset)
        {
            case Headset.WMR:
                Destroy(pst.viveText.gameObject);
                break;
            case Headset.VIVE:
                Destroy(pst.wmrText.gameObject);
                break;
        }
    }

    void setLayout(LayoutGroup lg)
    {
        switch (headset)
        {
            case Headset.WMR:
                lg.obj.transform.localPosition = lg.wmr.position;
                lg.obj.transform.localEulerAngles = lg.wmr.rotation;
                break;
            case Headset.VIVE:
                lg.obj.transform.localPosition = lg.vive.position;
                lg.obj.transform.localEulerAngles = lg.vive.rotation;
                break;
        }
    }

    [System.Serializable]
    public class LayoutGroup
    {
        public GameObject obj;
        public Layout vive;
        public Layout wmr;
    }

    [System.Serializable]
    public class Layout{
        public Vector3 position;
        public Vector3 rotation;
    }

    [System.Serializable]
    public class PlatformSpecificText
    {
        public Text viveText;
        public Text wmrText;
    }
}
