/*
 * Simple Twitch OAuth flow example
 * by HELLCAT
 *
 * At first glance, this looks like more than it actually is.
 * It's really no rocket science, promised! ;-)
 * And for any further questions contact me directly or on the Twitch-Developers discord.
 *
 * 🐦 https://twitter.com/therealhellcat
 * 📺 https://www.twitch.tv/therealhellcat
 */

/*
 * This has been heavily edited by salvner, for the original code check out hellcats github
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Oculus.Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace TwitchInteraction
{
    public class TwitchOAuth : MonoBehaviour
    {
        [SerializeField] private string twitchAuthUrl = "https://id.twitch.tv/oauth2/authorize";
        [SerializeField] internal string twitchClientId = "ncuqzdk0l37ovue2xn69o9matk8vw9"; //This looks scary but client_id is public, confirmed by Twitch Docs
        [SerializeField] private string twitchRedirectUrl = "http://localhost:8080/";
        private string _twitchAuthStateVerify;
        internal string _code;
        private string _state;
        internal Config _config;

        private void Start()
        {
            _code = "";
            _state = "";
        }

        /// <summary>
        /// Starts the Twitch OAuth flow by constructing the Twitch auth URL based on the scopes you want/need.
        /// </summary>
        internal void InitiateTwitchAuth(Config config)
        {
            _config = config;
            string[] scopes;
            string s;

            // list of scopes we want
            scopes = new[]
            {
            "user:read:email",
            "bits:read",
            "channel:read:redemptions"
        };

            // generate something for the "state" parameter.
            // this can be whatever you want it to be, it's gonna be "echoed back" to us as is and should be used to
            // verify the redirect back from Twitch is valid.
            _twitchAuthStateVerify = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

            // query parameters for the Twitch auth URL
            s = "client_id=" + twitchClientId + "&" +
                "redirect_uri=" + UnityWebRequest.EscapeURL(twitchRedirectUrl) + "&" +
                "state=" + _twitchAuthStateVerify + "&" +
                "response_type=token&" +
                "scope=" + String.Join("+", scopes);

            // start our local webserver to receive the redirect back after Twitch authenticated
            StartLocalWebserver();
            Console.WriteLine("Twitch Interaction Oauth:: Started Webserver");

            // open the users browser and send them to the Twitch auth URL
            Application.OpenURL(twitchAuthUrl + "?" + s);
        }

        /// <summary>
        /// Opens a simple "webserver" like thing on localhost:8080 for the auth redirect to land on.
        /// Based on the C# HttpListener docs: https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener
        /// </summary>
        private void StartLocalWebserver()
        {
            HttpListener httpListener = new HttpListener();

            httpListener.Prefixes.Add(twitchRedirectUrl);
            httpListener.Start();
            httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);
        }

        /// <summary>
        /// Handles the incoming HTTP request
        /// </summary>
        /// <param name="result"></param>
        private void IncomingHttpRequest(IAsyncResult result)
        {
            HttpListener httpListener;
            HttpListenerContext httpContext;
            HttpListenerRequest httpRequest;
            HttpListenerResponse httpResponse;
            string responseString;

            // get back the reference to our http listener
            httpListener = (HttpListener)result.AsyncState;

            // fetch the context object
            httpContext = httpListener.EndGetContext(result);

            httpRequest = httpContext.Request;

            _code = httpRequest.QueryString.Get("access_token");
            _state = httpRequest.QueryString.Get("state");


            httpResponse = httpContext.Response;

            if (String.IsNullOrEmpty(_code)) { //If no code we want users to click a button to redirect so we can get the hash
                Console.WriteLine("Twitch Interaction Oauth:: Waiting For User Redirect");
                responseString = "<html><body><button onclick='redirect()'>Click Me To Save Creds!</button></body><script>function redirect(){const o=window.location.hash;window.location.href='http://localhost:8080/?'+o.substring(1)}</script></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                // send the output to the client browser
                httpResponse.ContentLength64 = buffer.Length;
                System.IO.Stream output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                httpListener.BeginGetContext(new AsyncCallback(IncomingHttpRequest), httpListener);
            }else
            { //If code we want to tell users to close browser and restart subnautica. Also close and save resources
                Console.WriteLine("Twitch Interaction Oauth:: Got User Redirect, informing them they can close browser");
                responseString = "<html><body><b>DONE!</b><br>(Please close this tab/window and restart SUBNAUTICA)<br>Remember to never share your config file</body></html>";

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                // send the output to the client browser
                httpResponse.ContentLength64 = buffer.Length;
                System.IO.Stream output = httpResponse.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                Console.WriteLine("Twitch Interaction Oauth:: Done");
                // the HTTP listener has served it's purpose, shut it down
                httpListener.Stop();
                _config.ClientId = twitchClientId;
                _config.UsernameToken = _code;
                GetUserId();
            }
        }

        /// <summary>
        /// Makes the API call to exchange the received code for the actual auth token
        /// </summary>
        /// <param name="code">The code parameter received in the callback HTTP reuqest</param>
        internal void GetUserId()
        {

            Console.WriteLine("Twitch Interaction Oauth:: Getting User Id");

            // check that we got a code value and the state value matches our remembered one
            if ((_code.Length > 0) && (_state == _twitchAuthStateVerify))
            {
                string apiUrl;

                // construct full URL for API call
                apiUrl = "https://api.twitch.tv/helix/users";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET";
                request.Headers.Add("Authorization", "Bearer " + _code);
                request.Headers.Add("client-id", twitchClientId);
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string jsonResponse = reader.ReadToEnd();



                    var objectResponse = JObject.Parse(jsonResponse);
                    _config.UsernameId = (string)objectResponse["data"][0]["id"];
                    _config.Username = (string)objectResponse["data"][0]["login"];
                } catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                // make the call!
                // parse the return JSON into a more usable data object
            }
        }
    }
}
