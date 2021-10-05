﻿namespace Rollbar.Common
{
    using System;
    using System.Text;
    using System.IO;
    using Rollbar.Serialization.Json;

    /// <summary>
    /// Class StreamUtil.
    /// </summary>
    public static class StreamUtil
    {
        /// <summary>
        /// Converts a stream to a string using specified encoding.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string? ConvertToString(Stream? stream, Encoding? encoding = null)
        {
            if (stream == null || !stream.CanSeek || !stream.CanRead)
            {
                    return null;
            }

            var reader = new StreamReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            string content = reader.ReadToEnd();
            stream.Seek(0, SeekOrigin.Begin);
            
            // stream.Seek(0, SeekOrigin.Begin);

            //if (encoding == null)
            //{
            //    encoding = Encoding.UTF8;
            //}

            //using (StreamReader reader = new StreamReader(stream: stream,
            //    encoding: encoding,
            //    detectEncodingFromByteOrderMarks: true,
            //    bufferSize: Convert.ToInt32(stream.Length),
            //    leaveOpen: true)
            //)
            //{
            //    return reader.ReadToEnd();
            //}

            //var streamReader = new StreamReader(stream);
            //string content = streamReader.ReadToEnd();

            //byte[] buffer = new byte[stream.Length];
            //stream.Read(buffer, 0, buffer.Length);
            //string content = Encoding.UTF8.GetString(buffer);

            return content;
        }

        /// <summary>
        /// Interprets a stream as a JSON object (if any).
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Object.</returns>
        public static object? InterpretAsJsonObject(Stream? stream)
        {
            string? jsonString = StreamUtil.ConvertToString(stream);
            object? jsonObject = JsonUtil.InterpretAsJsonObject(jsonString);
            return jsonObject;
        }
    }
}
