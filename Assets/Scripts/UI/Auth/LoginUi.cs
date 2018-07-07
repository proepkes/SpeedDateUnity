using SpeedDate.ClientPlugins.Peer.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Barebones.MasterServer
{
    /// <summary>
    ///     Represents a basic view for login form
    /// </summary>
    public class LoginUi : MonoBehaviour
    {
        public InputField Username;
        public InputField Password;

        protected string UsernamePrefKey = "sd.auth.username";

        // Use this for initialization
        private void Start()
        {
            RestoreRememberedValues();
        }

        private void OnEnable()
        {
            gameObject.transform.localPosition = Vector3.zero;
        }

        /// <summary>
        ///     Tries to restore previously held values
        /// </summary>
        protected virtual void RestoreRememberedValues()
        {
            Username.text = PlayerPrefs.GetString(UsernamePrefKey, Username.text);
        }

        /// <summary>
        ///     Checks if inputs are valid
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateInput()
        {
            var error = "";

            if (Username.text.Length < 3)
                error += "Username is too short \n";

            if (Password.text.Length < 3)
                error += "Password is too short \n";

            if (error.Length > 0)
            {
                // We've got an error
                error = error.Remove(error.Length - 1);
                Debug.Log(error);
                return false;
            }

            return true;
        }


        /// <summary>
        ///     Called after clicking login button
        /// </summary>
        protected virtual void HandleRemembering()
        {
            // Remember is on
            PlayerPrefs.SetString(UsernamePrefKey, Username.text);
        }

        public virtual void OnLoginClick()
        {
            if (Client.Instance.GetPlugin<AuthPlugin>().IsLoggedIn)
            {
                Debug.Log("You're already logged in");
                return;
            }

            // Ignore if didn't pass validation
            if (!ValidateInput())
                return;

            HandleRemembering();

            Client.Instance.GetPlugin<AuthPlugin>().LogIn(Username.text, Password.text, info =>
            {
                Debug.Log($"Logged in as {info.Username}");
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SceneManager.LoadScene("Lobby");
                });
            }, Debug.Log);
        }

        public void OnCloseClick()
        {
            gameObject.SetActive(false);
        }
    }
}
