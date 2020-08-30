using System;

namespace Mediator.ControllerInterfaces
{
    public delegate void SendActionEvent(string actionCommand);

    public interface IController
    {
        event SendActionEvent OnSendActionEvent;
        event EventHandler OnIncorrectCommandRecieved;

        bool RecieveCommand(string command);
    }
}
