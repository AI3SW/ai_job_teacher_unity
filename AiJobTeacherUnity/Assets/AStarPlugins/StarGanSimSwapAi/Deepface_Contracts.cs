using System;
using System.Collections.Generic;
namespace Astar.REST.DeepFaceTech
{

    [System.Serializable]
    public class ImageInput
    {

        public string user_id;
        public int job_id;
        public string img;


    }

    [System.Serializable]
    public class ImageOutput
    {
        public string output_img;
    }

    [System.Serializable]
    public class JobListOutput
    {
        public List<Job> jobs;
    }

    [System.Serializable]
    public class Job
    {
        public int id;
        public string name;
        public List<string> descriptions;
        public string img;
    }

    [System.Serializable]
    public class TTSInput
    {
        public string rawAudio;
    }

    [System.Serializable]
    public class TTSOutput
    {
        public byte[] audioResult;
    }
    public class Error
    {
        public string error;
    }
}