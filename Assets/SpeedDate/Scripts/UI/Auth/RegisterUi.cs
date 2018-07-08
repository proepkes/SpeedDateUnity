using System;
using System.Collections.Generic;
using SpeedDate.ClientPlugins.Peer.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Barebones.MasterServer
{
    /// <summary>
    ///     Represents a basic view of registration form
    /// </summary>
    public class RegisterUi : MonoBehaviour
    {
        public InputField Username;
        public InputField Email;
        public InputField Password;
        public InputField RepeatPassword;

        public Button RegisterButton;

        private void Awake()
        {
            Email = Email ?? transform.Find("Email").GetComponent<InputField>();
            RegisterButton = RegisterButton ?? transform.Find("Button").GetComponent<Button>();
            Password = Password ?? transform.Find("Password").GetComponent<InputField>();
            RepeatPassword = RepeatPassword ?? transform.Find("RepeatPassword").GetComponent<InputField>();
            Username = Username ?? transform.Find("Username").GetComponent<InputField>();
        }

        public bool ValidateInput()
        {
            var error = "";

            if (Username.text.Length <= 3)
                error += "Username too short\n";

            if (Password.text.Length <= 3)
                error += "Password is too short\n";

            if (!Password.text.Equals(RepeatPassword.text))
                error += "Passwords don't match\n";

            if (Email.text.Length <= 3)
                error += "Email too short\n";

            if (error.Length > 0)
            {
                error = error.Remove(error.Length - 1);
                ShowError(error);
                return false;
            }

            return true;
        }

        private void OnEnable()
        {
            gameObject.transform.localPosition = Vector3.zero;
        }

        private void ShowError(string message)
        {
            Debug.Log(message);
        }

        public void OnRegisterClick()
        {
            // Ignore if didn't pass validation
            if (!ValidateInput())
                return;

            var data = new Dictionary<string, string>
            {
                {"username", Username.text},
                {"password", Password.text},
                {"email", Email.text}
            };

            Client.Instance.GetPlugin<AuthPlugin>().Register(data, () =>
            {
                UnityMainThreadDispatcher.Instance().Enqueue(OnSuccess);
            }, Debug.Log);
        }

        protected void OnSuccess()
        {
            // Hide registration form
            gameObject.SetActive(false);
        }

        public void OnCloseClick()
        {
            gameObject.SetActive(false);
        }
    }
}
