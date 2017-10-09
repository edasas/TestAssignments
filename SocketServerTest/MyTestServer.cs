using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SocketServerTest
{
    public class MyTestServer
    {
        #region ----------------------------------- Private members ------------------------------------------------

        //TODO: limiting request size?
        private byte[] _buffer = new byte[512];
        private List<Socket> _clientSockets = new List<Socket>();
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private int _port;
        private int _backlogConnections;
        private string _localFilePath;
        private Action<string> _logger;

        #endregion

        #region ----------------------------------- Constructors ---------------------------------------------------

        public MyTestServer() { }

        public MyTestServer(int port, int backlogConnections, string localFilePath, Action<string> logger)
        {
            _port = port;
            _backlogConnections = backlogConnections;
            _localFilePath = localFilePath;
            _logger = logger;
            ServerSetup();
        }

        #endregion

        #region ----------------------------------- Public methods -------------------------------------------------
        public void ServerSetup()
        {
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _serverSocket.Listen(_backlogConnections);
        }

        public void StartAccepting()
        {
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        #endregion

        #region ----------------------------------- Async Callbacks ------------------------------------------------

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(asyncResult);
                _clientSockets.Add(socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                StartAccepting();
            }
            catch (Exception ex)
            {
                _logger(ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            try
            {
                Socket socket = asyncResult.AsyncState as Socket;
                int received = socket.EndReceive(asyncResult);

                //null bytes trunkate
                byte[] dataBuffer = new byte[received];
                Array.Copy(_buffer, dataBuffer, received);

                string requestMessage = Encoding.UTF8.GetString(dataBuffer);
                if (String.IsNullOrEmpty(requestMessage))
                {
                    SendResponse("InvalidRequest", socket);
                    Console.WriteLine("Text received: EMPTY string");
                }
                else
                {
                    Console.WriteLine("Text received: " + requestMessage);

                    //TODO: raw stream data to magical httpRequestObject?
                    string[] requestMessages = requestMessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    string[] requestGetParameters = requestMessages[0].Split(' ');

                    if (requestGetParameters.Length > 2)
                    {
                        //TODO: accepting "GET" only?
                        if (requestGetParameters[0].ToLower().Contains("get"))
                        {
                            //TODO: accepting HTTP requests only?
                            if (requestGetParameters[2].ToLower().Contains("http"))
                            {
                                if (requestGetParameters[1] != @"/")
                                {
                                    string convertedFilePath = requestGetParameters[1].Replace('/', '\\');
                                    if (FileSystemHelper.FileExists(_localFilePath, convertedFilePath, _logger))
                                    {
                                        SendResponse(FileSystemHelper.FileAsByteArray(_localFilePath, convertedFilePath), socket);
                                    }
                                    else
                                    {
                                        SendResponse("<html><body><h1>404 Page</h1></body></html>", socket);
                                    }
                                }
                                else
                                {
                                    SendResponse("<html><body><h1>Landing page</h1></body></html>", socket);
                                }
                            }
                            else
                            {
                                SendResponse("Something is wrong with request, try another browser", socket);
                            }
                        }
                        else
                        {
                            SendResponse("Wrong operation, only accepting get", socket);
                        }
                    }
                    else
                    {
                        SendResponse("InvalidRequest", socket);
                        Console.WriteLine("Text received: invalid string: {0}" + requestMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger(ex.Message);
            }
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            try
            {
                var socket = asyncResult.AsyncState as Socket;
                try
                {
                    socket.EndSend(asyncResult);
                }
                catch (Exception ex)
                {
                    _logger(ex.Message);
                }
                finally
                {
                    socket.Close();
                }
            }
            finally
            {
                StartAccepting();
            }
        }

#endregion

#region ----------------------------------- Helper methods -------------------------------------------------

        private void SendResponse(string responseText, Socket socket)
        {
            byte[] responseData = Encoding.UTF8.GetBytes(responseText);
            SendResponse(responseData, socket);
        }

        private void SendResponse(byte[] responseData, Socket socket)
        {
            socket.BeginSend(responseData, 0, responseData.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
        }

#endregion
    }
}
