using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServerTest
{    
    class Program
    {
        //TODO: validation delegates        
        
        #region ----------------------------------- Private constants ----------------------------------------------

        private const int minPort = 0;
        private const int maxPort = 65537;
        private const int minConnections = 1;
        // let's cap the max backlog connections to 10, for now
        private const int maxConnections = 10;

        #endregion

        #region ----------------------------------- Private members ------------------------------------------------

        private static int portNumber = 80;
        private static int backlogConnections = 5;
        private static string localFilePath = System.IO.Directory.GetCurrentDirectory();

        #endregion

        static void Main(string[] args)
        {
            GetSettings();

            WriteSettingsMessage();

            var server = new MyTestServer(portNumber, backlogConnections, localFilePath, ConsoleWriter);

            ConsoleWriter(Environment.NewLine);
            ConsoleWriter("    Server started!");
            ConsoleWriter(Environment.NewLine);

            server.StartAccepting();
            Console.ReadLine();
        }

        #region ----------------------------------- Private helper methods -----------------------------------------

        private static void GetSettings()
        {
            portNumber = AskForValue(
                string.Format("Please enter port number (default is {0})", portNumber.ToString()),
                string.Format("Incorect value passed for port number, must be number between {0} and {1}", minPort, maxPort),
                minPort, maxPort);

            backlogConnections = AskForValue(
                string.Format("Please enter maximum allowed backlog connections (default value is {0})", backlogConnections),
                string.Format("Incorect value passed for max backlog connections, value must be between {0} and {1}", minConnections, maxConnections),
                minConnections, maxConnections);

            localFilePath = AskForValue(
                string.Format("Please enter destination directory (default is {0})", localFilePath),
                "Cannot locate directory");
        }

        private static void WriteSettingsMessage()
        {
            Console.Clear();
            ConsoleWriter("===========================================================");
            ConsoleWriter("                                                         ");
            ConsoleWriter("    Server configuration complete!                       ");
            ConsoleWriter("                                                         ");
            ConsoleWriter(string.Format("    Start listening on port: {0}                         ", portNumber));
            ConsoleWriter(string.Format("    Maximum backlog connections: {0}                     ", backlogConnections));
            ConsoleWriter(string.Format("    Current local file path: {0}                         ", localFilePath));
            ConsoleWriter("                                                         ");
            ConsoleWriter("===========================================================");
        }

        private static int AskForValue(
            string askingMessage, string messageIfIncorect, int minValue, int maxValue)
        {
            int toReturn = -1;

            while (toReturn < 0)
            {
                ConsoleWriter(askingMessage);
                string entry = Console.ReadLine();

                if (!String.IsNullOrEmpty(entry))
                {                    
                    toReturn = ValidationHelper.ValidateInt(entry, ConsoleWriter, minValue, maxValue);
                    if (toReturn < 0) ConsoleWriter(messageIfIncorect);
                }
                else
                {
                    toReturn = portNumber;
                }
            }

            return toReturn;
        }

        private static string AskForValue(
            string askingMessage, string messageIfIncorect)
        {
            string toReturn = string.Empty;

            while (String.IsNullOrEmpty(toReturn))
            {
                ConsoleWriter(askingMessage);
                string entry = Console.ReadLine();
                if (!String.IsNullOrEmpty(entry))
                {
                    toReturn = ValidationHelper.ValidateDirectoryPath(entry, ConsoleWriter);
                    if (String.IsNullOrEmpty(toReturn)) ConsoleWriter(messageIfIncorect);
                }
                else
                {
                    toReturn = localFilePath;
                }
            }

            return toReturn;
        }

        #endregion

        #region ----------------------------------- Public methods -------------------------------------------------

        public static void ConsoleWriter(string message)
        {
            Console.WriteLine(message);
        }

        #endregion
    }
}
