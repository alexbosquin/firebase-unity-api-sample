using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Firebase
{
    public struct GAMES {
        public const string url2040 = "https://game.firebaseio.com/2040/scores/.json";
        public const string urlBACK2EARTH = "https://game.firebaseio.com/backtoearth/scores/.json";

        public string UrlScore2040(string username)
        {
            return string.Format("https://game.firebaseio.com/2040/scores/{0}/.json", username);
        }

        public string UrlBack2Earth(string username)
        {
            return string.Format("https://game.firebaseio.com/backtoearth/scores/{0}/.json", username);
        }

    }

    public enum GAME { _2040, _Back2Earth}

    class FireManager
    {
        private WWW web;
        private MonoBehaviour user;
        private GAMES games = new GAMES();

        public FireManager(MonoBehaviour me)
        {
            user = me;
        }

        public void GET(GAME game)
        {
            string url = "";

            switch(game)
            {
                case GAME._2040:
                    url = GAMES.url2040;
                    break;

                case GAME._Back2Earth:
                    url = GAMES.urlBACK2EARTH;
                    break;
            }
            web = new WWW(url);
            user.StartCoroutine(ExecuteREST(web, "GET"));
        }

        public void POST(GAME game, string username, int score)
        {
            string url = "";
            url = GetURLFromGame(game, username);
            web = new WWW(url, GetJSON(score));
            //WWW www = new WWW(url, form);

            user.StartCoroutine(ExecuteREST(web, "POST"));
        }


        public void PUT(GAME game, string username, int score)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("X-HTTP-Method-Override", "PUT");

            string url = GetURLFromGame(game, username);
            web = new WWW(url, GetJSON(score), headers);

            user.StartCoroutine(ExecuteREST(web, "PUT"));

        }

        IEnumerator ExecuteREST(WWW www, string log = "Action")
        {
            Debug.Log("executing: " + log);
            yield return www;

            // check for errors
            if (www.error == null)
            {
                Debug.Log("WWW Ok!: " + www.text);
            }
            else
            {
                Debug.Log("WWW Error: " + www.error);
            }
        }

        private byte[] GetJSON(int score)
        {
            string theJSON = string.Format("\"value\":\"{0}\"", score);
            return Encoding.UTF8.GetBytes(string.Concat("{", theJSON, "}"));
        }

        private string GetURLFromGame(GAME game, string username)
        {
            switch (game)
            {
                case GAME._2040:
                    return games.UrlScore2040(username);

                case GAME._Back2Earth:
                    return games.UrlBack2Earth(username);
            }

            return null;
        }

    }
}
