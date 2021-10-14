using System.Threading.Tasks; // Task, is an object that handles threads, in essence its the same as a Coroutine
using System;
public interface DeepfaceInterface 
{
    //
    /// <summary>
    /// the event that end users will register to.
    /// @TODO, you will need to invoke it when the string is returned to you. 
    /// IE: On_ReceiveASR_Results?.invoke(data);
    /// </summary>
    event Action<Astar.REST.DeepFaceTech.ImageOutput> On_Receive_Results;

    /// <summary>
    /// Sends the photo image to the respective AI models for StarGan
    /// </summary>
    /// <param name="imgstr"></param>
    /// <returns></returns>
    Task<bool> SendDeepFace(string imgstr, string user_id,int job_id);

    /// <summary>
    /// Retrieves The list of Job from DB
    /// </summary>
    /// <param name="imgstr"></param>
    /// <param name="user_id"></param>
    /// <param name="job_id"></param>
    /// <returns></returns>
    Task<Astar.REST.DeepFaceTech.JobListOutput> GetJobList();
    /// <summary>
    /// for debuging purposes in realtime, to know what is the name of the model.
    /// </summary>
    /// <returns></returns>
    string GetName();

    /// <summary>
    ///  For starting the game sequence
    /// </summary>
    public void StartSession();
    /// <summary>
    ///  For starting the game sequence
    /// </summary>
    public void EndSession();

    public void InitConnection();

    public Astar.REST.Options GetServerOptions();
}
