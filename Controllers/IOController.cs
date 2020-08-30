using Mediator;
using Mediator.ControllerInterfaces;
using System;
using System.Linq;

namespace Controllers
{
    public class IOController : IController
    {
        private const int mTagIndex = 0;
        private const int mTagCount = 1;

        public event SendActionEvent OnSendActionEvent;
        public event EventHandler OnIncorrectCommandRecieved;

        public MouseActionsHandler MouseActionsHandler { private set; get; }

        public IOController()
        {
            InitIOHandlers();
        }

        public void InitIOHandlers()
        {
            MouseActionsHandler = new MouseActionsHandler();        
        }

        public bool RecieveCommand(string command)
        {
            var commandArray = command.Split(GlobalRepository._Delimiter);

            switch (commandArray[mTagIndex][0])
            {
                case GlobalRepository.Actions._MouseMoveAction: 
                    {
                        const int moveCommandCoordsCount = 2;

                        MouseActionsHandler.SendMouseMoveCommand(commandArray
                            .Skip(mTagCount)
                            .Take(moveCommandCoordsCount)
                            .ToArray());

                        break;
                    }

                case GlobalRepository.Actions._MouseLayoutInfo:
                    {
                        int width = int.Parse(commandArray[1]);
                        int heigth = int.Parse(commandArray[2]);

                        var mobileMouseWorkArea = new Resolution(width, heigth);

                        MouseActionsHandler.MobilePhoneScreenSizeInPx = mobileMouseWorkArea;

                        break;
                    }

                case GlobalRepository.Actions._MouseLeftClickAction:
                    {
                        MouseActionsHandler.SendClickCommand();
                        break;
                    }

                case GlobalRepository.Actions._MouseRightClickAction:
                    {
                        MouseActionsHandler.SendRightClickCommand();
                        break;
                    }

                case GlobalRepository.Actions._MouseDoubleClickAction:
                    {
                        MouseActionsHandler.SendDoubleClickCommand();
                        break;
                    }
                default:
                    return false;
            }

            return true;
        }
    }
}
