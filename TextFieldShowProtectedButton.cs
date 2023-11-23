using UnityEngine;
using Nova;
using NovaSamples.UIControls;

public class TextFieldShowProtectedButton : MonoBehaviour
{

    [SerializeField] private TextBlock displayText;
    [SerializeField] private TextBlock inputText;
    [SerializeField] private TextFieldExtended textField;

    private bool switchText = false;
    private string monoSpaceTag;

    public void SwitchText()
    {

        switchText = !switchText;

        if (switchText)
        {

            displayText.Visible = false;
            inputText.Visible = true;
            textField.UpdateTags(monoSpaceTag, false);

        }
        else
        {

            displayText.Visible = true;
            inputText.Visible = false;
            textField.UpdateTags(monoSpaceTag, true);

        }
    }


    // Coffee: Get the monospace tag set by the Input Provider
    public void SetMonospaceTag(string s)
    {

        monoSpaceTag = s;

    }
}
