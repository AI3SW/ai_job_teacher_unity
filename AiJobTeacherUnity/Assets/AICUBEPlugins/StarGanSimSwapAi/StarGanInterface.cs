using System.Threading.Tasks; // Task, is an object that handles threads, in essence its the same as a Coroutine
using System;
public interface StarGanInterface 
{
    //
    /// <summary>
    /// the event that end users will register to.
    /// @TODO, you will need to invoke it when the string is returned to you. 
    /// IE: On_ReceiveASR_Results?.invoke(data);
    /// </summary>
    event Action<AIcube.REST.FaceTech.Output> On_Receive_Results;

    /// <summary>
    /// Sends the photo image to the respective AI models for StarGan
    /// </summary>
    /// <param name="imgstr"></param>
    /// <returns></returns>
    Task<bool> SendStarGan(string imgstr, int style_id);

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
}
