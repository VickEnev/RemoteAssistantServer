using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Mediator;
using Mediator.ControllerInterfaces;
using Controllers.Win32FunctionsNamespace;

namespace Controllers
{
    public class MouseActionsHandler
    {
        private enum CoordsArrayPosition : int
        {
            X = 0,
            Y = 1
        }

        private enum ClickType : short
        {
            LeftClick = 0,
            RightClick,
            DoubleClick
        }

        private double mHeigthMultiplier = 1.0;
        private double mWidthMultiplier = 1.0;


        private Resolution mMobilePhoneScreenSizeInPx = new Resolution();
        public Resolution MobilePhoneScreenSizeInPx
        {
            get
            {
                return mMobilePhoneScreenSizeInPx;
            }
            set
            {
                if (mMobilePhoneScreenSizeInPx.Equals(value))
                    return;

                mMobilePhoneScreenSizeInPx = value;
                CalculateMultipliers();
            }
        }


        public MouseActionsHandler()
        {
        }

        public void SendMouseMoveCommand(string[] moveCoords)
        {
            if (!int.TryParse(moveCoords[(int)CoordsArrayPosition.X], out int mouseX))
            {
                //Log.Write($"Invalid X coordinate - {moveCoords[(int)CoordsArrayPosition.X]}");
                return;
            }

            if (!int.TryParse(moveCoords[(int)CoordsArrayPosition.Y], out int mouseY))
            {
               // Log.Write($"Invalid Y coordinate - {moveCoords[(int)CoordsArrayPosition.Y]}");
                return;
            }

            int magicModifierForX = 2;
            int magicModifierForY = 4;

            var coords = Win32Functions.GetMouseCoordinatesOnScreen();

            int x = coords.X + -((int)(mouseX * mHeigthMultiplier) * magicModifierForX);
            int y = coords.Y + -((int)(mouseY * mWidthMultiplier) * magicModifierForY);

            Win32Functions.SetCursorPos(x, y);
        }

        public void SendClickCommand()
        {
            ClickGenerator(ClickType.LeftClick);
        }

        public void SendRightClickCommand()
        {
            ClickGenerator(ClickType.RightClick);
        }

        public void SendDoubleClickCommand()
        {
            ClickGenerator(ClickType.DoubleClick);
        }

        private void CalculateMultipliers()
        {
            Resolution screenRes = ScreenResolution.GetCurrentScreenResolution();

            mWidthMultiplier = screenRes.Width / (MobilePhoneScreenSizeInPx.Width + 0.0);
            mHeigthMultiplier = screenRes.Heigth / (MobilePhoneScreenSizeInPx.Heigth + 0.0);
        }

        private void ClickGenerator(ClickType clickType)
        {
            MouseEventFlags mouseEventFlagsDown;
            MouseEventFlags mouseEventFlagsUp;
            uint inputs = 0;

            switch (clickType)
            {
                case ClickType.LeftClick:

                    mouseEventFlagsDown = MouseEventFlags.LEFTDOWN;
                    mouseEventFlagsUp = MouseEventFlags.LEFTUP;
                    inputs = 2;

                    break;


                case ClickType.RightClick:
                    mouseEventFlagsDown = MouseEventFlags.RIGHTDOWN;
                    mouseEventFlagsUp = MouseEventFlags.RIGHTUP;
                    inputs = 2;

                    break;

                case ClickType.DoubleClick:
                    mouseEventFlagsDown = MouseEventFlags.LEFTDOWN;
                    mouseEventFlagsUp = MouseEventFlags.LEFTUP;
                    inputs = 4;

                    break;

                default:
                    return;
            }

            Point mouseCoords = Win32Functions.GetMouseCoordinatesOnScreen();

            //Perform button click.
            INPUT down = new INPUT();
            down.type = SendInputEventType.InputMouse;
            down.mkhi.mi.dx = mouseCoords.X;
            down.mkhi.mi.dy = mouseCoords.Y;
            down.mkhi.mi.dwFlags = mouseEventFlagsDown;

            INPUT up = new INPUT();
            up.type = SendInputEventType.InputMouse;
            up.mkhi.mi.dx = mouseCoords.X;
            up.mkhi.mi.dy = mouseCoords.Y;
            up.mkhi.mi.dwFlags = mouseEventFlagsUp;

            INPUT[] clickAction = { down, up };
            INPUT[] doubleClickAction = { down, up, down, up };

            Win32Functions.SendInput(inputs
                , inputs == 2 ? clickAction : doubleClickAction
                , Marshal.SizeOf(down));
        }
    }

}
