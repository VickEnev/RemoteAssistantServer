using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mediator
{
    public struct Resolution
    {
        public int Width;
        public int Heigth;

        public Resolution(int w, int h)
        {
            Width = w;
            Heigth = h;
        }

        public override bool Equals(object obj)
        {
            return ((Resolution)obj).Heigth == Heigth && ((Resolution)obj).Width == Width;
        }

        public override int GetHashCode() // -> да махнем гадния Warning
        {
            return base.GetHashCode();
        }

    }

    public static class GlobalRepository
    {  
        public static char _Delimiter { get; private set; } = ':';
        public static char _EndMessageSymbol { get; private set; } = '!';
        public static short _MaxConnectionsBacklog = 2;
        public static string XmlConfigurationFilePath = 
            System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "RemoteAssistantConfig.xml");

        public static class Actions
        {
            public const char _MouseMoveAction               = 'm';
            public const char _DeviceInfo                    = 'i';
            public const char _MouseLayoutInfo               = 'l';
            public const char _MouseLeftClickAction          = 'c';
            public const char _MouseDoubleClickAction        = 'd';
            public const char _MouseRightClickAction         = 'r';
            public const char _Login                         = 'o';
            public const char _LoginWithUUID                 = 'u';
            public const char _LoginAuthernticationSuccess   = 'e';
            public const char _LoginAuthernticationFailed    = 'f';
        }
    }
}
