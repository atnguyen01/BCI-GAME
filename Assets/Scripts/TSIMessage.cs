using UnityEngine;
using System;

namespace TSI2Unity
{
    public class TSIMessage
    {
        public byte[] header_message; //8 bytes
        public byte[] requestSize;//4 bytes
        public byte[] request;//byteToNum(requestSize)
        public byte[] response;//depends on request
        //constructor 
        public TSIMessage(byte[] message)
        {
            header_message = new byte[8];
            Array.Copy(message, 0, header_message, 0, 8);

            requestSize = new byte[4];
            Array.Copy(message, header_message.Length, requestSize, 0, 4);

            request = new byte[byteToNum(requestSize)];
            Array.Copy(message, header_message.Length + requestSize.Length, request, 0, request.Length);

            response = new byte[message.Length - header_message.Length - requestSize.Length - request.Length];
            Array.Copy(message, header_message.Length + requestSize.Length + request.Length, response, 0, response.Length);
        }

        public int byteToNum(byte[] bytes)
        {
            int result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result += (int)(bytes[bytes.Length - i-1] * Math.Pow(256, i));
            }
            return result;
        }

        public double byteToDouble(byte[] bytes)
        {
            double result = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                result += (bytes[bytes.Length - i - 1] * Math.Pow(256, i));
            }
            return result;
        }

        public string byteToString(byte[] bytes)
        {
            string result = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                if ((int)bytes[i]!=0)
                    result += ((char)bytes[i]).ToString() + "";
            }
            // James: This outputs the numerous/annoying Oxy and Deoxy messages in console
            //Debug.Log(result);
            return result;
        }

        public string byteToIntString(byte[] bytes)
        {
            string result = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                result += (int)bytes[i] + " ";
            }
            return result;
        }

        public int[] byteToIntArray(byte[] bytes)
        {
            int[] result = new int[bytes.Length / 4];
            for(int i = 0; i < bytes.Length / 4;i++)
            {
                byte[] subArray = new byte[4];
                Array.Copy(bytes, i * 4, subArray, 0, 4);
                result[i] = byteToNum(subArray);
            }
            return result;
        }
    }
}