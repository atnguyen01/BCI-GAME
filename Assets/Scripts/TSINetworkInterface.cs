using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using static ip_input_script;

/// <summary>
/// Turbo Satori network interface
/// </summary>
namespace TSI2Unity
{
    public class TSINetworkInterface : MonoBehaviour
    {
        #region Public Variables
        [Header("Network")]
        public string ipAddress = ip_input_script.global_ip_address.text;
        public int port = 55555;
        public int waitingMessagesFrequency = 1;
        public TSIMessage tSIMessage;
        #endregion

        #region Private m_Variables
        private TcpClient m_Client;
        private NetworkStream m_NetStream = null;
        private byte[] m_Buffer = new byte[1024 * 1024 * 20];
        private int m_BytesReceived = 0;
        [SerializeField]
        private string m_ReceivedMessage = "";
        private byte[] m_message = new byte[0];
        
        [Tooltip("This value should be >= to Server waitingMessagesFrequency")]
        [Min(0)] private float m_DelayedCloseTime = 2f;
        #endregion

        #region Delegate Variables
        protected Action OnClientStarted = null;    //Delegate triggered when client start
        protected Action OnClientClosed = null;     //Delegate triggered when client close
        public Action OnMessageReceived = null;
        #endregion

        #region tsi values
        public int CurrentTimePoint;
        public int NrOfChannels;
        public int NrOfSelectedChannels;
        public int[] SelectedChannels;
        public Dictionary<int,float> DataOxy = new Dictionary<int, float>();
        public Dictionary<int, float> DataDeOxy = new Dictionary<int, float>();
        [HideInInspector]
        public string values;
        #endregion

        private void Start()
        {
            ipAddress = ip_input_script.global_ip_address.text;
            DontDestroyOnLoad(this);
            //SEND Request --- Connect to Request Socket
            OnClientStarted = delegate () { SendMessageToServer("Request Socket"); };
            OnMessageReceived = delegate () { UpdateTSIValues(); };
            StartClient();
            InvokeRepeating("FetchTSI", 1,1f);
        }
        //Start client and stablish connection with server

        private void Update()
        {

        }

        void FetchTSI()
        {
            Task.Run(() =>
            {
                tGetCurrentTimePoint();
                tGetNrOfChannels();
                tGetNrOfSelectedChannels();
                tGetSelectedChannels();
                tGetDataOxy();
                tGetDataDeOxy();
            });
        }
        public void StartClient()
        {
            //Early out
            if (m_Client != null)
            {
                ClientLog($"There is already a runing client on {ipAddress}::{port}", Color.red);
                return;
            }

            try
            {
                //Create new client
                m_Client = new TcpClient();
                //Set and enable client
                m_Client.Connect(ipAddress, port);
                ClientLog($"Client Started on {ipAddress}::{port}", Color.green);
                OnClientStarted?.Invoke();
            }
            catch (SocketException)
            {
                ClientLog("Socket Exception: Start Server first", Color.red);
                CloseClient();
            }          
        }

        #region Communication Client<->Turbo Satori Server
        private void GetMessage()
        {
            // [requestOk, response] = getMessage(obj)
            int HEADER_MSG_SIZE = 8;
            int counter = 0; // sign to stop coroutine
                             //early out if there is nothing connected

            //Stablish Client NetworkStream information
            m_NetStream = m_Client.GetStream();

            //Start Async Reading from Server and manage the response on MessageReceived function
            do
            {
               // ClientLog("Client is listening server msg...", Color.yellow);
                //Start Async Reading from Server and manage the response on MessageReceived function
                m_NetStream.BeginRead(m_Buffer, 0, m_Buffer.Length, MessageReceived, null);
                if (m_BytesReceived > HEADER_MSG_SIZE)
                {
                    //data must be validated!!!
                    tSIMessage = new TSIMessage(m_message);
                    m_ReceivedMessage = tSIMessage.byteToIntString(m_message);
                    m_BytesReceived = 0;
                    OnMessageReceived.Invoke();
                }
                counter = counter + 1;
                Thread.Sleep(waitingMessagesFrequency);
            } while (m_BytesReceived >= 0 && m_NetStream != null && m_Client != null && counter < 50);

        }


        //Send custom string msg to server
        protected virtual void SendMessageToServer(string messageToSend, int[] output = null)
        {
            // int[] output is an optional input
            try
            {
                m_NetStream = m_Client.GetStream();
            }
            catch (Exception)
            {
                ClientLog("Non-Connected Socket exception", Color.red);
                CloseClient();
                return;
            }

            //early out if there is nothing connected
            if (!m_Client.Connected)
            {
                ClientLog("Socket Error: Stablish Server connection first", Color.red);
                return;
            }

            //Build message to server
            byte[] encodedMessage = Encoding.ASCII.GetBytes(messageToSend); //Encode message as bytes
            byte[] request = new byte[encodedMessage.Length + 1];
            request[encodedMessage.Length] = 0;
            Array.Copy(encodedMessage, request, encodedMessage.Length);
            //Set length of the request
            byte[] requestLength = numToByte(request.Length, 4);

            byte[] outputVar;
            // add request parameters - output
            if (output != null)
            {
                outputVar = new byte[output.Length*4];
                for (int i = 0; i < output.Length; i++)
                {
                    byte[] subArray = numToByte(output[i], 4);
                    Array.Copy(subArray, 0, outputVar, i * 4, 4);
                }
            }
            else
            {
                outputVar = new byte[0];
            }

            //Set length of message
            byte[] messageSize = numToByte(request.Length + requestLength.Length + outputVar.Length, 8);

            byte[] tosend = new byte[messageSize.Length + requestLength.Length + request.Length + outputVar.Length];
            Array.Copy(messageSize, 0, tosend, 0, messageSize.Length);
            Array.Copy(requestLength, 0, tosend, messageSize.Length, requestLength.Length);
            Array.Copy(request, 0, tosend, messageSize.Length + requestLength.Length, request.Length);
            Array.Copy(outputVar, 0, tosend, messageSize.Length + requestLength.Length + request.Length, outputVar.Length);

            //Start Sync Writing
            m_NetStream.Write(tosend, 0, tosend.Length);
            m_NetStream.Flush();

            string s = "";
            for (int i = 0; i < tosend.Length; i++)
            {
                s += ((int)tosend[i]) + " ";
            }

            //ClientLog($"Msg sended to Server: <b>{messageToSend}</b>", Color.blue);
            //ClientLog($"Bytes sended to Server: <b>{s}</b>", Color.blue);

            //In case client informs the server that closes the connection
            if (messageToSend == "Close")
            {
                //It has to wait before closing, to ensure Close message is sended
                StartCoroutine(DelayedCloseClient(waitingMessagesFrequency + m_DelayedCloseTime));
            }
        }

        //AsyncCallback called when "BeginRead" is ended, waiting the message response from server
        private void MessageReceived(IAsyncResult result)
        {
            // 1-8 bytes: messageSize
            // 9-12 bytes: requestSize+1
            // 13-?: request
            // ~: values
            if (result.IsCompleted && m_Client.Connected)
            {
                //build message received from server
                m_BytesReceived = m_NetStream.EndRead(result);
                // m_ReceivedMessage = "";
                
                // m_ReceivedMessage = Encoding.BigEndianUnicode.GetString(m_Buffer, 0, m_BytesReceived);
                m_message = new byte[m_BytesReceived];
                Array.Copy(m_Buffer, m_message, m_BytesReceived);
                m_ReceivedMessage = tSIMessage.byteToIntString(m_message);
            }
        }
        #endregion

        #region Close Client
        //Close client connection
        public void CloseClient()
        {
            ClientLog("Client Closed", Color.red);

            //Reset everything to defaults
            if (m_Client.Connected)
                m_Client.Close();

            if (m_Client != null)
                m_Client = null;

            OnClientClosed?.Invoke();
        }
        private IEnumerator DelayedCloseClient(float delayedTime)
        {
            yield return new WaitForSeconds(delayedTime);
            CloseClient();
        }

        public void query(string request, int[] output = null)
        {
            SendMessageToServer(request, output);
            GetMessage();
        }

        public void query(string request)
        {
            SendMessageToServer(request);
            GetMessage();
        }

        //What to do with the received message on client
        void PrintMessageLog()
        {
            //ClientLog($"!!!Msg recived on Client: <b>{receivedMessage}</b>", Color.green);
            switch (m_ReceivedMessage)
            {
                case "Close": 
                    CloseClient();
                    break;
                default:
                    ClientLog($"Received message <b>{m_ReceivedMessage}</b>, has no special behaviuor", Color.red);
                    break;
            }
        }

        void UpdateTSIValues()
        {
            switch (tSIMessage.byteToString(tSIMessage.request))
            {
                case "tGetCurrentTimePoint": 
                    CurrentTimePoint = responseToInt();
                    break;
                case "tGetNrOfChannels": 
                    NrOfChannels = responseToInt();
                    break;
                case "tGetSelectedChannels": 
                    SelectedChannels = responseToIntArray();
                    break;
                case "tGetDataOxy": 
                    // responseToInt considers the array start counting from 1
                    int ch = responseToInt(1, 4); //channel
                    if (!DataOxy.ContainsKey(ch))
                    {
                        DataOxy.Add(ch,reponseReverseToFloat(tSIMessage.response,9, 4));
                    }
                    else DataOxy[ch] = reponseReverseToFloat(tSIMessage.response,9, 4);
                    break;
                case "tGetDataDeOxy":
                    // responseToInt considers the array start counting from 1
                    int chDe = responseToInt(1, 4); //channel
                    if (!DataDeOxy.ContainsKey(chDe))
                    {
                        DataDeOxy.Add(chDe, reponseReverseToFloat(tSIMessage.response, 9, 4));
                    }
                    else DataDeOxy[chDe] = reponseReverseToFloat(tSIMessage.response, 9, 4);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ClientLog
        //Custom Client Log - With Text Color
        protected virtual void ClientLog(string msg, Color color)
        {
            Debug.Log($"<b>Client:</b> {msg}");
        }
        //Custom Client Log - Without Text Color
        protected virtual void ClientLog(string msg)
        {
            Debug.Log($"<b>Client:</b> {msg}");
        }
        #endregion

        #region Decode Received Message
        // num = number to be converted; nbytes = number of byte available
        byte[] numToByte(int number, int nBytes)
        {
            byte[] result = new byte[nBytes];
            for (int i = 0; i < nBytes; i++)
            {
                result[nBytes - i - 1] = (byte)(number % 256);
                number = (int)Math.Floor(((double)number / 256));
            }
            return result;
        }

        public int responseToInt()
        {
          // Debug.Log("return value: " + tSIMessage.byteToNum(tSIMessage.response));
            return tSIMessage.byteToNum(tSIMessage.response);
        }

        public int responseToInt(int index, int length)
        {
            // Debug.Log("return value: " + tSIMessage.byteToNum(tSIMessage.response));
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                //reverse the byte array
                bytes[i] = tSIMessage.response[index + i - 1];
            }
            //Debug.Log("return value: " + tSIMessage.byteToIntArray(tSIMessage.response).ToString());
            return tSIMessage.byteToNum(bytes);
        }
        public int[] responseToIntArray()
        {
           
            //Debug.Log("return value: " + tSIMessage.byteToIntArray(tSIMessage.response).ToString());
            return tSIMessage.byteToIntArray(tSIMessage.response);
        }

        public float reponseReverseToFloat(byte[] source, int index, int length)
        {
           // convert endian from swaping the byte ordering
            byte[] temp = new byte[length];
            Array.Copy(source, index - 1, temp, 0, 4);
            Array.Reverse(temp);
            // Debug.Log("return value: " + tSIMessage.byteToIntArray(tSIMessage.response).ToString());
            return BitConverter.ToSingle(temp, 0);
        }

        #endregion

        #region Queries
        /* BASIC PROJECT QUERIES*/
        void tGetCurrentTimePoint()
        {
            //
            //   Send: tGetCurrentTimePoint
            //   Receive: int CurrentTimePoint
            //   Provides the number of the currently processed step during real-time processing as an
            //       integer. Note that this function is 1-based, i.e. when the first step is processed the function
            //       returns "1" not "0"; this is important when the return value is used to access time-related
            //       information; in this case subtract "1" from the returned value.           
            // send request and read message/response                                   
 
            query("tGetCurrentTimePoint");
        }
        void tGetNrOfChannels()
        {
            // Send: tGetNrOfChannels
            // Receive: int NrOfChannels
            // Provides the number of available channels
            query("tGetNrOfChannels");
        }
        /* SELECTED CHANNELS INFO */
        void tGetNrOfSelectedChannels()
        {
            // Send: tGetNrOfSelectedChannels
            // Receive: int NrOfSelectedChannels
            // Provides the number of channels that are currently selected in the GUI. When processing selected
            //         channels(e.g.to average their signals), this function must be called at each time point since it can
            //  change anytime.Inspect the provided ExamplePlugin code for more details.

            query("tGetNrOfSelectedChannels");
        }
        void tGetSelectedChannels()
        {

            //  send: tgetselectedchannels
            //  receive: int [nrofselectedchannels] selectedchannels
            //  provides the full time course data to a given time point that is also used internally in tsi.
            //  individual values are 2-byte short integers. note that the "timepoint" parameter must be
            //  smaller than the value returned by the "tgetcurrenttimepoint()" function. if a voxel with
            //  specific coordinates needs to be accessed, use the term "z_coord*dim_x*dim_y +
            //  y_coord*dim_x + x_coord". for details, see the provided example clients.


            query("tGetSelectedChannels");
        }
        void tGetDataOxy()
        {
            // Do not run this alone without these two: 
            int[] output = new int[2];
            // Validate selected channel before sending inquery
            if (SelectedChannels.Length==NrOfSelectedChannels)
            {
                for (int i = 0; i < SelectedChannels.Length; i++)
                {
                    output[0] = SelectedChannels[i];
                    output[1] = CurrentTimePoint;
                    query("tGetDataOxy", output);
                }
            }
        }
        void tGetDataDeOxy()
        {
            // Do not run this alone without these two: 
            int[] output = new int[2];
            // Validate selected channel before sending inquery
            if (SelectedChannels.Length == NrOfSelectedChannels)
            {
                for (int i = 0; i < SelectedChannels.Length; i++)
                {
                    output[0] = SelectedChannels[i];
                    output[1] = CurrentTimePoint;
                    query("tGetDataDeOxy", output);
                }
            }
        }
        #endregion
    }

}


