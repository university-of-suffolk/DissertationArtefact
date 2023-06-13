using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace DissertationProject
{
    //Note for future me: Need to add a system to automatically go through the string and find the attribute(s) for ease of use
    public class TextInputComponent:BaseComponent
    {
        public TextInputComponent(string textAttributeName)
        {
            //htmlContents = """<input type="text" class="form-control" name=" \b""" + textAttributeName + """ \b" value= @ViewData[" """ + textAttributeName + """ "]>""";
            htmlContents = "<input type=\"text\" class=\"form-control\" name=\"" + textAttributeName + "\" value= @ViewData[\"" + textAttributeName + "\"]>";
            attributeNames = new List<string>();
            attributeNames.Add(textAttributeName); 
        }
    }

    public class IntInputComponent:BaseComponent
    {
        public IntInputComponent(string intAttributeName)
        {
            htmlContents = "<input type=\"number\" class=\"form-control\" name=\"" + intAttributeName + "\" value= @ViewData[\"" + intAttributeName + "\"] step=\"1\">";
            attributeNames = new List<string>();
            attributeNames.Add(intAttributeName);
        }

        public IntInputComponent(string intAttributeName, int min, int max)
        {
            htmlContents = "<input type=\"number\" class=\"form-control\" name=\"" + intAttributeName + "\" value= @ViewData[\"" + intAttributeName + "\"] step=\"1\" min=\"" + min +"\" max = \"" + max + " \">";
            attributeNames = new List<string>();
            attributeNames.Add(intAttributeName);
        }
    }

    public class DropDownComponent:BaseComponent
    {

        public DropDownComponent(string dropDownAttributeName, List<string> options_) 
        {
            attributeNames = new List<string>();
            attributeNames.Add(dropDownAttributeName);
            //need to check which one is selected on preload
            string json = JsonSerializer.Serialize(options_);
            persistentData.Add("Name", dropDownAttributeName);
            persistentData.Add("OptionsJson", json);
            componentIdentifier = "DropDown";
            htmlContents = "@ViewData[\"" + persistentData["Name"] + "Contents\"]";
        }

        public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
        {
            List<string> options = JsonSerializer.Deserialize<List<string>>(persistentData["OptionsJson"]);

            //Adds all of the options to the HTML selection
            //Selects what the current value is
            string value = "<select name =\"" + persistentData["Name"] + "\" id=\"" + persistentData["Name"] + "\">";
            foreach (string option in options)
            {
                if (attributes.ContainsKey(persistentData["Name"]) && option == attributes[persistentData["Name"]])
                {
                    value = value + "<option value=\"" + option + "\"selected>" + option + "</option>";
                }
                else
                {
                    value = value + "<option value=\"" + option + "\">" + option + "</option>";
                }
            }
            value = value + "</select>";
            Dictionary<string, string> returnValue = new Dictionary<string, string>();
            returnValue.Add(persistentData["Name"] + "Contents", value);

            return returnValue;
        }
    }

    public class BoolInputComponent:BaseComponent
    {
        public BoolInputComponent(string boolAttributeName) 
        {
            persistentData.Add("Name", boolAttributeName);
            htmlContents = "<input type=\"checkbox\" id= \"" + persistentData["Name"] + "\" name=\"" + persistentData["Name"] + "\" @ViewData[\"" + persistentData["Name"] + "Value\"]>";
            attributeNames = new List<string>();
            attributeNames.Add(boolAttributeName);
            componentIdentifier = "Bool";
        }
        public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
        {
            Dictionary<string, string> returnValue = new Dictionary<string, string>();

            if (attributes.ContainsKey(persistentData["Name"]) && attributes[persistentData["Name"]] == "on")
                returnValue.Add(persistentData["Name"] + "Value", "checked");
            else
                returnValue.Add(persistentData["Name"] + "Value", "");
            return returnValue;
        }
    }
}
