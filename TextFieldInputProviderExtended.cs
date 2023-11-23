using Nova;
using UnityEngine;

namespace NovaSamples.UIControls
{
    public abstract class TextFieldInputProviderExtended : MonoBehaviour
    {

        protected enum CharacterValidationType
        {

            None,
            Standard,
            Alphanumeric,
            NumbersOnly,
            CharactersOnly,
            Username,
            EmailAddress,
            Password

        }

        protected enum InputDisplayType
        {

            Standard,
            Protected

        }

        const string usernameSpecialCharacters = ",-@_";
        const string emailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";
        const string passwordSpecialCharacters = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

        [Header("Text field options.")]
        [Tooltip("The character validation type.")]
        [SerializeField] 
        protected CharacterValidationType validationType;
        [Tooltip("The maximium length of the input string.")]
        [SerializeField] 
        protected int maxInputLength;
        [Tooltip("The display type, protected shoudl be used for any \"secret\" inputs.")]
        [SerializeField] 
        protected InputDisplayType displayType;
        protected string monospaceTag;
        [Tooltip("The monospace size for the protected display field so the cursor lines up correctly.")]
        [SerializeField]
        protected float monoSpaceSize;

        [Tooltip("The input field to which this class provides input.")]
        [SerializeField]
        protected TextFieldExtended inputField = null;
        [Tooltip("The focuser, used to determine when this class should start providing input.")]
        [SerializeField]
        protected TextFieldSelectorExtended selector = null;
        [Tooltip("The display TextBlock.")]
        [SerializeField]
        protected TextFieldDisplayExtended display = null;
        [Tooltip("The display TextBlock.")]
        [SerializeField]
        protected TextFieldShowProtectedButton displayButton = null;
        [SerializeField]
        [Tooltip("If true, the user can input newlines into the field.")]
        protected bool allowNewlines = true;

        protected abstract void HandleFocused();
        protected abstract void HandleFocusLost();

        protected virtual void OnEnable()
        {
            // Subscribe to focus change events
            selector.OnFocused += HandleFocused;
            selector.OnFocusLost += HandleFocusLost;

            /// <summary>
            /// Coffee:
            /// Update the monospace tag with the value provided, only add it on enable if this field starts a protected
            /// Update the "Show Protected" button's tag and if it should be shown and update the Display Tags on the TextField
            /// </summary>
            monospaceTag = $"<mspace={monoSpaceSize}em>";
            displayButton.SetMonospaceTag(monospaceTag);

            if (displayType == InputDisplayType.Standard)
            {

                displayButton.gameObject.SetActive(false);
                display.protectedText = false;
                inputField.UpdateTags(monospaceTag, false);

            }
            else
            {

                displayButton.gameObject.SetActive(true);
                display.protectedText = true;
                inputField.UpdateTags(monospaceTag, true);

            }
        }

        protected virtual void OnDisable()
        {
            // Unsubscribe from focus change events
            selector.OnFocused -= HandleFocused;
            selector.OnFocusLost -= HandleFocusLost;
        }

        /// <summary>
        /// Stub function for text/character validation. Can be extended to 
        /// support different scenarios, e.g. numbers only, alphanumerics only, etc.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool IsValidChar(char c)
        {

            // Don't allow return chars or tabulator key to be entered into single line fields.
            if (!allowNewlines && (c == '\t' || c == '\r' || c == 10))
            {
                return false;
            }

            // Null character
            if (c == 0)
            {
                return false;
            }

            // Delete key on mac
            if (c == 127)
            {
                return false;
            }

            if (validationType == CharacterValidationType.None || validationType == CharacterValidationType.Standard) 
            {

                // Coffee: Use no validation
                return true;

            }
            else if (validationType == CharacterValidationType.Alphanumeric)
            {

                // Coffee: Only alow Alphnumeric characters
                if (c >= 'A' && c <= 'Z') return true;
                else if (c >= 'a' && c <= 'z') return true;
                else if (c >= '0' && c <= '9') return true;
                else return false;

            }
            else if (validationType == CharacterValidationType.NumbersOnly)
            {

                // Coffee: Only allow numbers characters
                if (c >= '0' && c <= '9') return true;
                else return false;

            }
            else if (validationType == CharacterValidationType.CharactersOnly)
            {

                // Coffee: Only allow Alphabet characters
                if (c >= 'A' && c <= 'Z') return true;
                else if (c >= 'a' && c <= 'z') return true;
                else return false;

            }
            else if (validationType == CharacterValidationType.Username)
            {
                // Coffee: Username only allows Alpanumeric and a handful of special characters              

                if (c >= 'A' && c <= 'Z') return true;
                else if (c >= 'a' && c <= 'z') return true;
                else if (c >= '0' && c <= '9') return true;
                else if (usernameSpecialCharacters.IndexOf(c) != -1) return true;
                else return false;
            }
            else if (validationType == CharacterValidationType.EmailAddress)
            {
                // Coffee:
                // From StackOverflow about allowed characters in email addresses:
                // Uppercase and lowercase English letters (a-z, A-Z)
                // Digits 0 to 9
                // Characters ! # $ % & ' * + - / = ? ^ _ ` { | } ~
                // Character . (dot, period, full stop) provided that it is not the first or last character,
                // and provided also that it does not appear two or more times consecutively.

                if (c >= 'A' && c <= 'Z') return true;
                else if (c >= 'a' && c <= 'z') return true;
                else if (c >= '0' && c <= '9') return true;
                else if (c == '@' && inputField.Text.IndexOf('@') == -1) return true;
                else if (emailSpecialCharacters.IndexOf(c) != -1) return true;
                else if (c == '.')
                {
                    char lastChar = (inputField.Text.Length > 0) ? inputField.Text[Mathf.Clamp(inputField.CursorPosition.Index, 0, inputField.Text.Length - 1)] : ' ';
                    char nextChar = (inputField.Text.Length > 0) ? inputField.Text[Mathf.Clamp(inputField.CursorPosition.Index + 1, 0, inputField.Text.Length - 1)] : '\n';
                    if (lastChar != '.' && nextChar != '.') return true; 
                    else return false;
                }
                else return false; 
            }
            else if (validationType == CharacterValidationType.Password)
            {
                // Coffee: Password only allows Alpanumeric and special characters              

                if (c >= 'A' && c <= 'Z') return true;
                else if (c >= 'a' && c <= 'z') return true;
                else if (c >= '0' && c <= '9') return true;
                else if (passwordSpecialCharacters.IndexOf(c) != -1) return true;
                else return false;
            }

            return true;
        }
    }
}
