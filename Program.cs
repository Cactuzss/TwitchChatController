using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace stearm
{
    public static class Token
    {
        public static String oauth = "";
    }

    public class EventProcessor
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MousePoint
        {
            public int X;
            public int Y;
            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }


        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);


        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out MousePoint lpMousePoint);


        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        private const int KEYEVENTF_EXTENDEDKEY = 0x1;
        private const int KEYEVENTF_KEYUP = 0x2;

        public static void PressKey(byte keyCode)
        {
            keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void HoldKey(byte keyCode)
        {
            keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
        }

        public static void ReleaseKey(byte keyCode)
        {
            keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SetCursorPosition(MousePoint point)
        {
            SetCursorPos(point.X, point.Y);
        }

        public static MousePoint GetCursorPosition()
        {
            MousePoint currentMousePoint;
            var gotPoint = GetCursorPos(out currentMousePoint);
            if (!gotPoint) { currentMousePoint = new MousePoint(0, 0); }
            return currentMousePoint;
        }

        public static void MouseEvent(MouseEventFlags value, int dx, int dy)
        {
            MousePoint position = GetCursorPosition();

            mouse_event
                ((int)value,
                 dx,
                 dy,
                 0,
                 0)
                ;
        }
    }


    public class Program
    {
        static TwitchClient client;

        static bool shouldRun = true;
        static bool timeout = false;

        static Timer timer;

        private static void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            const int offsetX = 1024;
            const int offsetY = 1024;

            switch(e.Command.CommandText)
            {
                case "stop":
                    shouldRun = false;
                    return;

                case "help":
                    client.SendMessage(e.Command.ChatMessage.Channel, "Забавные приколы: !w !a !s !d !tab !b !q !e !f !left !up !down !lmb !rmb !spin ");
                    break;
            }

            if (!shouldRun || timeout) return;

            EventProcessor.MousePoint point = EventProcessor.GetCursorPosition();

            switch (e.Command.CommandText)
            {
                case "tab":
                    EventProcessor.HoldKey(0x09);
                    Thread.Sleep(1000 * 2);
                    EventProcessor.ReleaseKey(0x09);
                    break;

                case "b":
                    EventProcessor.PressKey(0x42);
                    break;

                case "w":
                    EventProcessor.HoldKey(0x57); 
                    Thread.Sleep(1000 * 2);
                    EventProcessor.ReleaseKey(0x57);
                    break;

                case "a":
                    EventProcessor.HoldKey(0x41);
                    Thread.Sleep(1000 * 2);
                    EventProcessor.ReleaseKey(0x41);
                    break;

                case "s":
                    EventProcessor.HoldKey(0x53);
                    Thread.Sleep(1000 * 2);
                    EventProcessor.ReleaseKey(0x53);
                    break;

                case "d":
                    EventProcessor.HoldKey(0x44);
                    Thread.Sleep(1000 * 2);
                    EventProcessor.ReleaseKey(0x44);
                    break;


                case "f":
                    EventProcessor.PressKey(0x46);
                    break;

                case "q":
                    EventProcessor.PressKey(0x51);
                    break;

                case "e":
                    EventProcessor.PressKey(0x45);
                    break;

                case "left":
                    EventProcessor.SetCursorPosition(new EventProcessor.MousePoint(offsetX, 0));
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.Move, -offsetX * 2, 0);
                    break;

                case "right":
                    EventProcessor.SetCursorPosition(new EventProcessor.MousePoint(0, 0));
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.Move, offsetX * 2, 0);
                    break;

                case "up":
                    EventProcessor.SetCursorPosition(new EventProcessor.MousePoint(0, offsetY));
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.Move, 0, -offsetY * 2);
                    break;

                case "down":
                    EventProcessor.SetCursorPosition(new EventProcessor.MousePoint(0, 0));
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.Move, 0, offsetY * 2);
                    break;

                case "lmb":
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftDown, point.X, point.Y);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftUp, point.X, point.Y);
                    break;

                case "rmb":
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.RightDown, point.X, point.Y);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.RightUp, point.X, point.Y);
                    break;

                case "esc":
                    EventProcessor.PressKey(0x1B);
                    Thread.Sleep(3000);
                    EventProcessor.PressKey(0x1B);
                    break;

                case "space":
                    EventProcessor.PressKey(0x20); 
                    break;

                case "buy":
                    EventProcessor.PressKey(0x49);

                    Thread.Sleep(100);
                    EventProcessor.SetCursorPosition(360, 525);

                    Thread.Sleep(100);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftDown, point.X, point.Y);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftUp, point.X, point.Y);

                    Thread.Sleep(25);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftDown, point.X, point.Y);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftUp, point.X, point.Y);

                    Thread.Sleep(25);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftDown, point.X, point.Y);
                    EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftUp, point.X, point.Y);

                    //Thread.Sleep(100);
                    //EventProcessor.SetCursorPosition(1715, 415);
                    //Thread.Sleep(100);
                    //EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftDown, point.X, point.Y);
                    //EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.LeftUp, point.X, point.Y);

                    Thread.Sleep(100);
                    EventProcessor.PressKey(0x1B);

                    break;

                case "spin":
                    for (int i = 0; i < 15; i++)
                    {
                        EventProcessor.SetCursorPosition(new EventProcessor.MousePoint(0, 0));
                        EventProcessor.MouseEvent(EventProcessor.MouseEventFlags.Move, offsetX / 2, 0);
                        Thread.Sleep(10);
                    }
                    break;
            }

            timeout = true;
        }

        static void Main(string[] args)
        {
            if (Token.oauth == "")
            {
                Console.WriteLine("You need to get the oauth token");
                return;
            }

            int period = 10 * 1000;
            string channel = "panda__ol";

            for (int i = 0; i < args.Length; i++)
            {
                switch(args[i])
                {
                    case "--timeout":
                        try
                        {
                            period = int.Parse(args[i + 1]);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Specify timeout duration in millisecs after --timeout");
                            return;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Duration must be an integer");
                            return;
                        }

                        break;

                    case "--channel":
                        try
                        {
                            channel = args[i + 1];
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Specify channel after the --channel");
                            return;
                        }

                        break;
                }
            }


            TimerCallback tm = new TimerCallback((object obj) => { timeout = false; });
            timer = new Timer(tm, timeout, 0, period);

            client = new TwitchClient();
            ConnectionCredentials credentials = new ConnectionCredentials("cactuzss", Token.oauth);

            client.OnLog += (object sender, OnLogArgs e) => { Console.WriteLine(e.Data); };
            client.OnJoinedChannel += (object sender, OnJoinedChannelArgs e) => { client.SendMessage(e.Channel, "meow meow meow"); };
            client.OnChatCommandReceived += Client_OnChatCommandReceived;

            client.Initialize(credentials, channel);
            client.Connect();

            while (true)
            {
                Console.WriteLine("Press enter to continue listening...");
                Console.ReadLine();

                shouldRun = true;
            }
            
        }

    }
}
