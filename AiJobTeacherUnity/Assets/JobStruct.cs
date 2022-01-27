using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AIcube.AITeacher
{


    [Serializable]
    public class Version
    {
        public string version;

    }

    [Serializable]
    public class FullJobData
    {
        public int id;//sort by id
        public string name;
        public List<string> descriptions;
        public Texture2D OriginalSpr;
        public Texture2D FutureSpr;
        public string hint;
        public lockType lockstate;

    }

    [Serializable]
    public struct PlayerJsonSaveData
    {
        public string user_id;
        public string imageStr;
        public string version;
        public PrivacyPolicy policy;
        public List<JobData> JobProfile;
    }

    [Serializable]
    public class JobData
    {
        public int id;
        public string name;
        public int lockstate;
        public string ImageStrAI;
        public string ImageStrOriginal;
        public List<string> descriptions;
    }
    [Serializable]
    public class PrivacyPolicy
    {
        public bool agree = false;
    }
    [Serializable]
    public enum lockType
    {
        Unknown = 0,
        Locked = 1,
        Unlocked = 2,
    }
}
