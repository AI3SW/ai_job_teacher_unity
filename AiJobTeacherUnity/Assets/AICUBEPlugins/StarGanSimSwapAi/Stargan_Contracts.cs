using System;
using System.Collections.Generic;
namespace AIcube.REST.FaceTech
{

    [System.Serializable]
    public class Input
    {
        public int style_id;
        public string session_id;
        public string img;
            
    }

    public class Output
    {
        public string output_img;
    }
    
}