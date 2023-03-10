using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Horizon.XmlRpc.Core;

namespace Horizon.XmlRpc.Client
{
    public class XmlRpcClientProtocol : Component, IXmlRpcProxy {
        private bool _allowAutoRedirect = true;

        private string _connectionGroupName = null;

        private bool _expect100Continue = false;
        private bool _enableCompression = false;

        private ICredentials _credentials = null;
        private WebHeaderCollection _headers = new WebHeaderCollection();
        private int _indentation = 2;
        private bool _keepAlive = true;
        private XmlRpcNonStandard _nonStandard = XmlRpcNonStandard.None;
        private bool _preAuthenticate = false;
        private Version _protocolVersion = HttpVersion.Version11;
        private IWebProxy _proxy = null;
        private HttpResponseHeaders _responseHeaders;
        private int _timeout = 100000;
        private string _url = null;
        private string _userAgent = "XML-RPC.NET";
        private bool _useEmptyParamsTag = true;
        private bool _useIndentation = true;
        private bool _useIntTag = false;
        private bool _useStringTag = true;
        private Encoding _xmlEncoding = null;
        private string _xmlRpcMethod = null;

        private X509CertificateCollection _clientCertificates
          = new X509CertificateCollection();
        private CookieContainer _cookies = new CookieContainer();
        private Guid _id = Guid.NewGuid();


        public XmlRpcClientProtocol(System.ComponentModel.IContainer container) {
            container.Add(this);
            InitializeComponent();
        }

        public XmlRpcClientProtocol() {
            InitializeComponent();
        }

        public object Invoke(
          MethodBase mb,
          params object[] Parameters) {
            return Invoke(this, mb as MethodInfo, Parameters);
        }

        public object Invoke(
          MethodInfo mi,
          params object[] Parameters) {
            return Invoke(this, mi, Parameters);
        }

        public object Invoke(
          string MethodName,
          params object[] Parameters) {
            return Invoke(this, MethodName, Parameters);
        }

        public object Invoke(
          Object clientObj,
          string methodName,
          params object[] parameters) {
            MethodInfo mi = GetMethodInfoFromName(clientObj, methodName, parameters);
            return Invoke(this, mi, parameters);
        }
        public HttpClient _client = null;

        public HttpClient Client {
            get {
                if (_client == null) {



                    HttpClientHandler handler = new HttpClientHandler();
                    if (_proxy != null) {
                        handler.Proxy = _proxy;
                        handler.CookieContainer = _cookies;
                        handler.AllowAutoRedirect = _allowAutoRedirect;
                        handler.PreAuthenticate = PreAuthenticate;
                        handler.Credentials = Credentials;
                        if (ClientCertificates != null && ClientCertificates.Count > 0) {
                            handler.ClientCertificates.AddRange(ClientCertificates);
                            handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                        }
                        if (_enableCompression) {
                            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                        } else {
                            handler.AutomaticDecompression = DecompressionMethods.None;
                        }
                    }


                    _client = new HttpClient(handler);
                    _client.Timeout = TimeSpan.FromMilliseconds(Timeout);
                    if (_userAgent != null) {
                        _client.DefaultRequestHeaders.UserAgent.Clear();
                        _client.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
                    }
                    _client.DefaultRequestHeaders.AcceptEncoding.Clear();
                    if (_enableCompression) {
                        _client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip,deflate");
                    }
                    _client.DefaultRequestHeaders.ConnectionClose = !_keepAlive;
                }
                return _client;
            }
        }

        public object Invoke(
          Object clientObj,
          MethodInfo mi,
          params object[] parameters) {
            _responseHeaders = null;
            object reto = null;
            _lastResponseUri = null;
            Uri useUrl = new Uri(GetEffectiveUrl(clientObj));
            XmlRpcRequest req = MakeXmlRpcRequest(mi, parameters, clientObj, _xmlRpcMethod, _id);
            XmlRpcSerializer serializer = new XmlRpcSerializer();
            if (_xmlEncoding != null) {
                serializer.XmlEncoding = _xmlEncoding;
            }
            Stream serStream = new MemoryStream(2000);
            serializer.UseIndentation = _useIndentation;
            serializer.Indentation = _indentation;
            serializer.NonStandard = _nonStandard;
            serializer.UseStringTag = _useStringTag;
            serializer.UseIntTag = _useIntTag;
            serializer.UseEmptyParamsTag = _useEmptyParamsTag;
            serializer.SerializeRequest(serStream, req);
            bool logging = (RequestEvent != null);
            if (logging) {
                serStream.Position = 0;
                MemoryStream copy = new MemoryStream();
                serStream.CopyTo(copy);
                copy.Position = 0;
                try {
                    OnRequest(new XmlRpcRequestEventArgs(req.proxyId, req.number, copy));
                } finally {
                    copy.Close();
                }
            }
            serStream.Position = 0;
            StreamContent content = new StreamContent(serStream);
            if (_headers != null) {
                foreach (string key in _headers) {
                    content.Headers.Add(key, _headers[key]);
                }
            }
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            using (HttpResponseMessage response = Client.PostAsync(useUrl, content).Result) {
                Stream responseNetworkStream = response.Content.ReadAsStreamAsync().Result;
                _lastResponseUri = useUrl;
                Stream responseStream;
                _responseHeaders = response.Headers;
                logging = (ResponseEvent != null);
                if (logging) {
                    responseStream = new MemoryStream();
                    responseNetworkStream.CopyTo(responseStream);
                    responseStream.Position = 0;
                    MemoryStream copy = new MemoryStream();
                    responseStream.CopyTo(copy);
                    copy.Position = 0;
                    responseStream.Position = 0;
                    try {
                        OnResponse(new XmlRpcResponseEventArgs(req.proxyId, req.number, copy));
                    } finally {
                        copy.Close();
                    }
                } else {
                    responseStream = responseNetworkStream;
                }
                XmlRpcResponse resp = ReadResponse(req, response, responseStream, null);
                reto = resp.retVal;
            }
            return reto;
        }


        public bool AllowAutoRedirect {
            get { return _allowAutoRedirect; }
            set { _client?.Dispose(); _client = null; _allowAutoRedirect = value; }
        }


        [Browsable(false)]
        public X509CertificateCollection ClientCertificates {
            get { return _clientCertificates; }
        }

        public string ConnectionGroupName {
            get { return _connectionGroupName; }
            set { _connectionGroupName = value; }
        }

        [Browsable(false)]
        public ICredentials Credentials {
            get { return _credentials; }
            set { _client?.Dispose(); _client = null; _credentials = value; }
        }

        public bool EnableCompression {
            get { return _enableCompression; }
            set { _client.Dispose(); _client = null; _enableCompression = value; }
        }

        [Browsable(false)]
        public WebHeaderCollection Headers {
            get { return _headers; }
        }

        public bool Expect100Continue {
            get { return _expect100Continue; }
            set { if (_client != null) { _client.DefaultRequestHeaders.ExpectContinue = value; }; _expect100Continue = value; }
        }

        public CookieContainer CookieContainer {
            get { return _cookies; }
        }

        public Guid Id {
            get { return _id; }
        }

        public int Indentation {
            get { return _indentation; }
            set { _indentation = value; }
        }

        public bool KeepAlive {
            get { return _keepAlive; }
            set { if (_client != null) { _client.DefaultRequestHeaders.ConnectionClose = !value; }; _keepAlive = value; }
        }

        public XmlRpcNonStandard NonStandard {
            get { return _nonStandard; }
            set { _nonStandard = value; }
        }

        public bool PreAuthenticate {
            get { return _preAuthenticate; }
            set { _client?.Dispose(); _client = null; _preAuthenticate = value; }
        }

        [Browsable(false)]
        public System.Version ProtocolVersion {
            get { return _protocolVersion; }
            set { _protocolVersion = value; }
        }

        [Browsable(false)]
        public IWebProxy Proxy {
            get { return _proxy; }
            set { _client?.Dispose(); _client = null; _proxy = value; }
        }

        private Uri _lastResponseUri = null;

        public CookieCollection ResponseCookies {
            get {
                return _cookies.GetCookies(_lastResponseUri);
            }
        }

        public HttpResponseHeaders ResponseHeaders {
            get { return _responseHeaders; }
        }

        WebHeaderCollection IXmlRpcProxy.ResponseHeaders {
            get {
                WebHeaderCollection result = new WebHeaderCollection();
                foreach (var kv in ResponseHeaders) {
                    result.Add(kv.Key, kv.Value.ToString());
                }
                return result;
            }
    }

        public int Timeout
        {
            get { return _timeout; }
            set { if (_client != null) {
                    _client.Timeout = TimeSpan.FromMilliseconds(value);
                }
                _timeout = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public bool UseEmptyParamsTag
        {
            get { return _useEmptyParamsTag; }
            set { _useEmptyParamsTag = value; }
        }

        public bool UseIndentation
        {
            get { return _useIndentation; }
            set { _useIndentation = value; }
        }

        public bool UseIntTag
        {
            get { return _useIntTag; }
            set { _useIntTag = value; }
        }

        public string UserAgent
        {
            get { return _userAgent; }
            set { 
                if (_client != null) {
                    _client.DefaultRequestHeaders.UserAgent.ParseAdd(value);
                }
            _userAgent = value; }
        }

        public bool UseStringTag
        {
            get { return _useStringTag; }
            set { _useStringTag = value; }
        }

        [Browsable(false)]
        public Encoding XmlEncoding
        {
            get { return _xmlEncoding; }
            set { _xmlEncoding = value; }
        }

        public string XmlRpcMethod
        {
            get { return _xmlRpcMethod; }
            set { _xmlRpcMethod = value; }
        }

        XmlRpcRequest MakeXmlRpcRequest(MethodInfo mi,
          object[] parameters, object clientObj, string xmlRpcMethod,
          Guid proxyId)
        {
            string rpcMethodName = GetRpcMethodName(clientObj, mi);
            XmlRpcRequest req = new XmlRpcRequest(rpcMethodName, parameters, mi,
              xmlRpcMethod, proxyId);
            return req;
        }

        XmlRpcResponse ReadResponse(
          XmlRpcRequest req,
          HttpResponseMessage respose,
          Stream respStm,
          Type returnType)
        {
            if (respose.StatusCode != HttpStatusCode.OK)
            {
                // status 400 is used for errors caused by the client
                // status 500 is used for server errors (not server application
                // errors which are returned as fault responses)
                if (respose.StatusCode == HttpStatusCode.BadRequest)
                    throw new XmlRpcException(respose.ReasonPhrase);
                else
                    throw new XmlRpcServerException(respose.ReasonPhrase);
            }
            XmlRpcSerializer serializer = new XmlRpcSerializer();
            serializer.NonStandard = _nonStandard;
            Type retType = returnType;
            if (retType == null)
                retType = req.mi.ReturnType;
            XmlRpcResponse xmlRpcResp
              = serializer.DeserializeResponse(respStm, retType);
            return xmlRpcResp;
        }

        MethodInfo GetMethodInfoFromName(object clientObj, string methodName,
          object[] parameters)
        {
            Type[] paramTypes = new Type[0];
            if (parameters != null)
            {
                paramTypes = new Type[parameters.Length];
                for (int i = 0; i < paramTypes.Length; i++)
                {
                    if (parameters[i] == null)
                        throw new XmlRpcNullParameterException("Null parameters are invalid");
                    paramTypes[i] = parameters[i].GetType();
                }
            }
            Type type = clientObj.GetType();
            MethodInfo mi = type.GetMethod(methodName, paramTypes);
            if (mi == null)
            {
                try
                {
                    mi = type.GetMethod(methodName);
                }
                catch (System.Reflection.AmbiguousMatchException)
                {
                    throw new XmlRpcInvalidParametersException("Method parameters match "
                      + "the signature of more than one method");
                }
                if (mi == null)
                    throw new Exception(
                      "Invoke on non-existent or non-public proxy method");
                else
                    throw new XmlRpcInvalidParametersException("Method parameters do "
                      + "not match signature of any method called " + methodName);
            }
            return mi;
        }

        string GetRpcMethodName(object clientObj, MethodInfo mi)
        {
            string rpcMethod;
            string MethodName = mi.Name;
            Attribute attr = Attribute.GetCustomAttribute(mi,
              typeof(XmlRpcBeginAttribute));
            if (attr != null)
            {
                rpcMethod = ((XmlRpcBeginAttribute)attr).Method;
                if (rpcMethod == "")
                {
                    if (!MethodName.StartsWith("Begin") || MethodName.Length <= 5)
                        throw new Exception(String.Format(
                          "method {0} has invalid signature for begin method",
                          MethodName));
                    rpcMethod = MethodName.Substring(5);
                }
                return rpcMethod;
            }
            // if no XmlRpcBegin attribute, must have XmlRpcMethod attribute   
            attr = Attribute.GetCustomAttribute(mi, typeof(XmlRpcMethodAttribute));
            if (attr == null)
            {
                throw new Exception("missing method attribute");
            }
            XmlRpcMethodAttribute xrmAttr = attr as XmlRpcMethodAttribute;
            rpcMethod = xrmAttr.Method;
            if (rpcMethod == "")
            {
                rpcMethod = mi.Name;
            }
            return rpcMethod;
        }

        public IAsyncResult BeginInvoke(
          MethodBase mb,
          object[] parameters,
          AsyncCallback callback,
          object outerAsyncState)
        {
            return BeginInvoke(mb as MethodInfo, parameters, this, callback,
              outerAsyncState);
        }

        public IAsyncResult BeginInvoke(
          MethodInfo mi,
          object[] parameters,
          AsyncCallback callback,
          object outerAsyncState)
        {
            return BeginInvoke(mi, parameters, this, callback,
              outerAsyncState);
        }

        public IAsyncResult BeginInvoke(
          string methodName,
          object[] parameters,
          object clientObj,
          AsyncCallback callback,
          object outerAsyncState)
        {
            MethodInfo mi = GetMethodInfoFromName(clientObj, methodName, parameters);
            return BeginInvoke(mi, parameters, this, callback,
              outerAsyncState);
        }

        public IAsyncResult BeginInvoke(
          MethodInfo mi,
          object[] parameters,
          object clientObj,
          AsyncCallback callback,
          object outerAsyncState)
        {
            string useUrl = GetEffectiveUrl(clientObj);

            MemoryStream streamContent = new MemoryStream(2000);

            
            XmlRpcRequest xmlRpcReq = MakeXmlRpcRequest(mi, parameters, clientObj, _xmlRpcMethod, _id);

            XmlRpcSerializer serializer = new XmlRpcSerializer();
            if (_xmlEncoding != null) {
                serializer.XmlEncoding = _xmlEncoding;
            }
            serializer.UseIndentation = _useIndentation;
            serializer.Indentation = _indentation;
            serializer.NonStandard = _nonStandard;
            serializer.UseStringTag = _useStringTag;
            serializer.UseIntTag = _useIntTag;
            serializer.UseEmptyParamsTag = _useEmptyParamsTag;
            serializer.SerializeRequest(streamContent, xmlRpcReq);
            bool logging = (RequestEvent != null);
            if (logging) {
                streamContent.Position = 0;
                MemoryStream copy = new MemoryStream();
                streamContent.CopyTo(copy);
                copy.Position = 0;
                try {
                    OnRequest(new XmlRpcRequestEventArgs(xmlRpcReq.proxyId, xmlRpcReq.number, copy));
                } finally {
                    copy.Close();
                }
            }
            streamContent.Position = 0;
            StreamContent content = new StreamContent(streamContent);
            if (_headers != null) {
                foreach (string key in _headers) {
                    content.Headers.Add(key, _headers[key]);
                }
            }
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            Task<HttpResponseMessage> response = Client.PostAsync(useUrl, content);

            XmlRpcAsyncResult asr = new XmlRpcAsyncResult(this, xmlRpcReq, _xmlEncoding, _useEmptyParamsTag, _useIndentation, _indentation, _useIntTag, _useStringTag, response, callback, outerAsyncState, 0);
            return asr;
        }

        static void ReadAsyncResponse(XmlRpcAsyncResult result)
        {
            HttpResponseMessage responseMessage = result.Response;
            if (responseMessage.Content.Headers.ContentLength.GetValueOrDefault() == 0)
            {
                result.Dispose();
                return;
            }
            try
            {
                Stream networkStream = responseMessage.Content.ReadAsStreamAsync().Result;
                ReadAsyncResponseStream(result, networkStream);
            }
            catch (Exception ex)
            {
                ProcessAsyncException(result, ex);
            }
        }

        static void ReadAsyncResponseStream(XmlRpcAsyncResult result, Stream networkStream)
        {
            IAsyncResult asyncResult;
            do
            {
                byte[] buff = result.Buffer;
                long contLen = result.Response.Content.Headers.ContentLength.GetValueOrDefault(-1);
                if (buff == null)
                {
                    if (contLen == -1)
                        result.Buffer = new Byte[1024];
                    else
                        result.Buffer = new Byte[contLen];
                }
                else
                {
                    if (contLen != -1 && contLen > result.Buffer.Length)
                        result.Buffer = new Byte[contLen];
                }
                buff = result.Buffer;
                asyncResult = networkStream.BeginRead(buff, 0, buff.Length,
                  new AsyncCallback(ReadResponseCallback), result);
                if (!asyncResult.CompletedSynchronously)
                    return;
            }
            while (!(ProcessAsyncResponseStreamResult(networkStream, result, asyncResult)));
        }

        static bool ProcessAsyncResponseStreamResult(Stream networkStream, XmlRpcAsyncResult result,
          IAsyncResult asyncResult)
        {
            int endReadLen = networkStream.EndRead(asyncResult);
            long contLen = result.Response.Content.Headers.ContentLength.GetValueOrDefault(-1);
            bool completed;
            MemoryStream bufferedStream = null;
            if (endReadLen == 0)
                completed = true;
            else if (contLen > 0 && endReadLen == contLen)
            {
                bufferedStream = new MemoryStream(result.Buffer);
                completed = true;
            }
            else
            {
                if (bufferedStream == null)
                {
                    bufferedStream = new MemoryStream(result.Buffer.Length);
                }
                bufferedStream.Write(result.Buffer, 0, endReadLen);
                completed = false;
            }
            if (completed) {
                result.Dispose();
            }
            return completed;
        }


        static void ReadResponseCallback(IAsyncResult asyncResult)
        {
            XmlRpcAsyncResult result = (XmlRpcAsyncResult)asyncResult.AsyncState;
            result.CompletedSynchronously = asyncResult.CompletedSynchronously;
            Stream stream = result.Response.Content.ReadAsStreamAsync().Result;
            if (asyncResult.CompletedSynchronously)
                return;
            try
            {
                bool completed = ProcessAsyncResponseStreamResult(stream, result, asyncResult);
                if (!completed)
                    ReadAsyncResponseStream(result, stream);
            }
            catch (Exception ex)
            {
                ProcessAsyncException(result, ex);
            }
        }

        static void ProcessAsyncException(XmlRpcAsyncResult clientResult,
          Exception ex)
        {
            if (clientResult.IsCompleted)
                throw new Exception("error during async processing");
            clientResult.Exception = ex;
            clientResult.Dispose();
        }

        public object EndInvoke(
          IAsyncResult asr)
        {
            return EndInvoke(asr, null);
        }

        public object EndInvoke(
          IAsyncResult asr,
          Type returnType)
        {
            object reto = null;
            Stream responseStream = null;
            try
            {
                XmlRpcAsyncResult clientResult = (XmlRpcAsyncResult)asr;
                if (clientResult.Exception != null)
                    throw clientResult.Exception;
                if (clientResult.EndSendCalled)
                    throw new Exception("dup call to EndSend");
                clientResult.EndSendCalled = true;
                HttpResponseMessage msg = clientResult.Response;

                responseStream = msg.Content.ReadAsStreamAsync().Result;
                if (ResponseEvent != null)
                {
                    MemoryStream copy = new MemoryStream();
                    if (responseStream is MemoryStream copyable) {
                        copyable.Position = 0;
                        copyable.CopyTo(copy);
                        copyable.Position = 0;
                    } else {
                        MemoryStream temp = new MemoryStream();
                        responseStream.CopyTo(temp);
                        temp.Position = 0;
                        temp.CopyTo(copy);
                        temp.Position = 0;
                        try {
                            responseStream.Close();
                        } catch { }
                        temp.Position = 0;
                        responseStream = temp;
                    }
                    OnResponse(new XmlRpcResponseEventArgs(
                      clientResult.XmlRpcRequest.proxyId,
                      clientResult.XmlRpcRequest.number,
                      responseStream));
                    responseStream.Position = 0;
                }

                XmlRpcResponse resp = ReadResponse(clientResult.XmlRpcRequest,
                  msg, responseStream, returnType);
                reto = resp.retVal;
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Close();
            }
            return reto;
        }

        string GetEffectiveUrl(object clientObj)
        {
            Type type = clientObj.GetType();
            // client can either have define URI in attribute or have set it
            // via proxy's ServiceURI property - but must exist by now
            string useUrl = "";
            if (Url == "" || Url == null)
            {
                Attribute urlAttr = Attribute.GetCustomAttribute(type,
                  typeof(XmlRpcUrlAttribute));
                if (urlAttr != null)
                {
                    XmlRpcUrlAttribute xrsAttr = urlAttr as XmlRpcUrlAttribute;
                    useUrl = xrsAttr.Uri;
                }
            }
            else
            {
                useUrl = Url;
            }
            if (useUrl == "")
            {
                throw new XmlRpcMissingUrl("Proxy XmlRpcUrl attribute or Url "
                  + "property not set.");
            }
            return useUrl;
        }

        [XmlRpcMethod("system.listMethods")]
        public string[] SystemListMethods()
        {
            return (string[])Invoke("SystemListMethods", new Object[0]);
        }

        [XmlRpcMethod("system.listMethods")]
        public IAsyncResult BeginSystemListMethods(
          AsyncCallback Callback,
          object State)
        {
            return BeginInvoke("SystemListMethods", new object[0], this, Callback,
              State);
        }

        public string[] EndSystemListMethods(IAsyncResult AsyncResult)
        {
            return (string[])EndInvoke(AsyncResult);
        }

        [XmlRpcMethod("system.methodSignature")]
        public object[] SystemMethodSignature(string MethodName)
        {
            return (object[])Invoke("SystemMethodSignature",
              new Object[] { MethodName });
        }

        [XmlRpcMethod("system.methodSignature")]
        public IAsyncResult BeginSystemMethodSignature(
          string MethodName,
          AsyncCallback Callback,
          object State)
        {
            return BeginInvoke("SystemMethodSignature",
              new Object[] { MethodName }, this, Callback, State);
        }

        public Array EndSystemMethodSignature(IAsyncResult AsyncResult)
        {
            return (Array)EndInvoke(AsyncResult);
        }

        [XmlRpcMethod("system.methodHelp")]
        public string SystemMethodHelp(string MethodName)
        {
            return (string)Invoke("SystemMethodHelp",
              new Object[] { MethodName });
        }

        [XmlRpcMethod("system.methodHelp")]
        public IAsyncResult BeginSystemMethodHelp(
          string MethodName,
          AsyncCallback Callback,
          object State)
        {
            return BeginInvoke("SystemMethodHelp",
              new Object[] { MethodName }, this, Callback, State);
        }

        public string EndSystemMethodHelp(IAsyncResult AsyncResult)
        {
            return (string)EndInvoke(AsyncResult);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        public event XmlRpcRequestEventHandler RequestEvent;
        public event XmlRpcResponseEventHandler ResponseEvent;


        protected virtual void OnRequest(XmlRpcRequestEventArgs e)
        {
            if (RequestEvent != null)
            {
                RequestEvent(this, e);
            }
        }

        internal bool LogResponse
        {
            get { return ResponseEvent != null; }
        }



        protected virtual void OnResponse(XmlRpcResponseEventArgs e)
        {
            if (ResponseEvent != null)
            {
                ResponseEvent(this, e);
            }
        }

        internal void InternalOnResponse(XmlRpcResponseEventArgs e)
        {
            OnResponse(e);
        }
    }


    public delegate void XmlRpcRequestEventHandler(object sender,
      XmlRpcRequestEventArgs args);

    public delegate void XmlRpcResponseEventHandler(object sender,
      XmlRpcResponseEventArgs args);
}