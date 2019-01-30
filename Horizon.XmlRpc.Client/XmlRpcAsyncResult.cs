using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Horizon.XmlRpc.Core;

namespace Horizon.XmlRpc.Client
{
    public class XmlRpcAsyncResult : IAsyncResult
    {
        // IAsyncResult members
        public object AsyncState
        {
            get { return userAsyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                bool completed = isCompleted;
                if (manualResetEvent == null)
                {
                    lock (this)
                    {
                        if (manualResetEvent == null)
                            manualResetEvent = new ManualResetEvent(completed);
                    }
                }
                if (!completed && isCompleted)
                    manualResetEvent.Set();
                return manualResetEvent;
            }
        }

        public bool CompletedSynchronously
        {
            get { return completedSynchronously; }
            set
            {
                if (completedSynchronously)
                    completedSynchronously = value;
            }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public CookieCollection ResponseCookies
        {
            get { return _responseCookies; }
        }

        public WebHeaderCollection ResponseHeaders
        {
            get { return _responseHeaders; }
        }

        public bool UseEmptyParamsTag
        {
            get { return _useEmptyParamsTag; }
        }

        public bool UseIndentation
        {
            get { return _useIndentation; }
        }

        public int Indentation
        {
            get { return _indentation; }
        }

        public bool UseIntTag
        {
            get { return _useIntTag; }
        }

        public bool UseStringTag
        {
            get { return _useStringTag; }
        }

        // public members
        public void Abort()
        {
            if (request != null)
                request.Abort();
        }

        public Exception Exception
        {
            get { return exception; }
        }

        public XmlRpcClientProtocol ClientProtocol
        {
            get { return clientProtocol; }
        }

        //internal members
        internal XmlRpcAsyncResult(
          XmlRpcClientProtocol ClientProtocol,
          XmlRpcRequest XmlRpcReq,
          Encoding XmlEncoding,
          bool useEmptyParamsTag,
          bool useIndentation,
          int indentation,
          bool UseIntTag,
          bool UseStringTag,
          WebRequest Request,
          AsyncCallback UserCallback,
          object UserAsyncState,
          int retryNumber)
        {
            xmlRpcRequest = XmlRpcReq;
            clientProtocol = ClientProtocol;
            request = Request;
            userAsyncState = UserAsyncState;
            userCallback = UserCallback;
            completedSynchronously = true;
            xmlEncoding = XmlEncoding;
            _useEmptyParamsTag = useEmptyParamsTag;
            _useIndentation = useIndentation;
            _indentation = indentation;
            _useIntTag = UseIntTag;
            _useStringTag = UseStringTag;
        }

        internal void Complete(
          Exception ex)
        {
            exception = ex;
            Complete();
        }

        internal void Complete()
        {
            try
            {
                if (responseStream != null)
                {
                    responseStream.Close();
                    responseStream = null;
                }
                if (responseBufferedStream != null)
                    responseBufferedStream.Position = 0;
            }
            catch (Exception ex)
            {
                if (exception == null)
                    exception = ex;
            }
            isCompleted = true;
            try
            {
                if (manualResetEvent != null)
                    manualResetEvent.Set();
            }
            catch (Exception ex)
            {
                if (exception == null)
                    exception = ex;
            }
            if (userCallback != null)
                userCallback(this);
        }

        internal WebResponse WaitForResponse()
        {
            if (!isCompleted)
                AsyncWaitHandle.WaitOne();
            if (exception != null)
                throw exception;
            return response;
        }

        internal bool EndSendCalled
        {
            get { return endSendCalled; }
            set { endSendCalled = value; }
        }

        internal byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        internal WebRequest Request
        {
            get { return request; }
        }

        internal WebResponse Response
        {
            get { return response; }
            set { response = value; }
        }

        internal Stream ResponseStream
        {
            get { return responseStream; }
            set { responseStream = value; }
        }

        internal XmlRpcRequest XmlRpcRequest
        {
            get { return xmlRpcRequest; }
            set { xmlRpcRequest = value; }
        }

        internal Stream ResponseBufferedStream
        {
            get { return responseBufferedStream; }
            set { responseBufferedStream = value; }
        }

        internal Encoding XmlEncoding
        {
            get { return xmlEncoding; }
        }

        XmlRpcClientProtocol clientProtocol;
        WebRequest request;
        AsyncCallback userCallback;
        object userAsyncState;
        bool completedSynchronously;
        bool isCompleted;
        bool endSendCalled = false;
        ManualResetEvent manualResetEvent;
        Exception exception;
        WebResponse response;
        Stream responseStream;
        Stream responseBufferedStream;
        byte[] buffer;
        XmlRpcRequest xmlRpcRequest;
        Encoding xmlEncoding;
        bool _useEmptyParamsTag;
        bool _useIndentation;
        int _indentation;
        bool _useIntTag;
        bool _useStringTag;

        internal CookieCollection _responseCookies;
        internal WebHeaderCollection _responseHeaders;

    }
}