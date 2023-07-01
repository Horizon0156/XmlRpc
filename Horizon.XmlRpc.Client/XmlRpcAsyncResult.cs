using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Horizon.XmlRpc.Core;

namespace Horizon.XmlRpc.Client
{

    public class XmlRpcAsyncResult : IAsyncResult, IDisposable
    {


        // IAsyncResult members
        public object AsyncState
        {
            get { return userAsyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get {
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
            get { return response.IsCompleted; }
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
            if (response != null && !response.IsCompleted && cancelSource != null) {
                cancelSource.Cancel();
            }
        }

        public Exception Exception
        {
            get { return exception; }
            internal set { exception = value; }
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
          Task<HttpResponseMessage> Response,
          AsyncCallback UserCallback,
          object UserAsyncState,
          int retryNumber)
        {
            xmlRpcRequest = XmlRpcReq;
            clientProtocol = ClientProtocol;
            userAsyncState = UserAsyncState;
            completedSynchronously = true;
            xmlEncoding = XmlEncoding;
            _useEmptyParamsTag = useEmptyParamsTag;
            _useIndentation = useIndentation;
            _indentation = indentation;
            _useIntTag = UseIntTag;
            _useStringTag = UseStringTag;
            response = Response;
            cancelSource = new CancellationTokenSource();
            tcs = new TaskCompletionSource<HttpResponseMessage>(UserAsyncState);
            manualResetEvent = new ManualResetEvent(response.IsCompleted);
            response.ContinueWith((t, o) => {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.InnerExceptions);
                else if (t.IsCanceled)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);
                manualResetEvent.Set();
                if (UserCallback != null)
                    UserCallback(this);
            }, TaskScheduler.Default, cancelSource.Token);
        }

        internal HttpResponseMessage WaitForResponse()
        {
            return response.Result;
        }

        public void Dispose() {
            cancelSource?.Dispose();
            response?.Dispose();
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

        internal HttpResponseMessage Response
        {
            get { return response.Result; }
        }


        internal XmlRpcRequest XmlRpcRequest
        {
            get { return xmlRpcRequest; }
            set { xmlRpcRequest = value; }
        }

        internal Encoding XmlEncoding
        {
            get { return xmlEncoding; }
        }
        private TaskCompletionSource<HttpResponseMessage> tcs;
        XmlRpcClientProtocol clientProtocol;
        object userAsyncState;
        bool completedSynchronously;
        bool endSendCalled = false;
        ManualResetEvent manualResetEvent;
        Exception exception;
        byte[] buffer;
        XmlRpcRequest xmlRpcRequest;
        Encoding xmlEncoding;
        bool _useEmptyParamsTag;
        bool _useIndentation;
        int _indentation;
        bool _useIntTag;
        bool _useStringTag;
        private readonly Task<HttpResponseMessage> response;
        private readonly CancellationTokenSource cancelSource;

    }
}