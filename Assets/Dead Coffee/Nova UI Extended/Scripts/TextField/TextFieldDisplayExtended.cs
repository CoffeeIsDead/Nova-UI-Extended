using Nova;
using NovaSamples.UIControls;
using UnityEngine;

public class TextFieldDisplayExtended : MonoBehaviour
{
    [SerializeField]
    private TextFieldExtended sourceInputField = null;
    [SerializeField]
    private TextBlock displayTextBlock = null;
    [SerializeField]
    public bool protectedText;

    private void OnEnable()
    {
        sourceInputField.OnTextChanged += HandleTextChanged;
    }

    private void OnDisable()
    {
        sourceInputField.OnTextChanged -= HandleTextChanged;
    }


    // Coffee: The text shows '*' in place of characters if it is Protected, otherwise just the same text body
    private void HandleTextChanged()
    {
        if (protectedText)
        {

            displayTextBlock.Text = sourceInputField.GetDisplayTags() + new string('*', sourceInputField.Text.Length);

        }
        else
        {

            displayTextBlock.Text = sourceInputField.GetDisplayTags() + sourceInputField.Text;

        }
    }
}
