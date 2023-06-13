using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text.Json;

//Note to future me: Need to add loading/saving from json
namespace DissertationProject
{
    public class Sheet
    {
        public Sheet() 
        {
            components = new List<BaseComponent>();
            sheetName = "characterSheetDefaultName";
        } 

        public List<BaseComponent> components { get; set; }
        public string sheetName { get; set; }


        public Dictionary<string, string> GetAttributes(string fileLocation)
        {
            string json = System.IO.File.ReadAllText(fileLocation);
            if (json != null)
            {
                return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            else
                return null;
        }



        //Note to self: In the page itself, when loading the HTML from the sheet, convert anything with "@Viewdata[]"
        public virtual Dictionary<String, String> PreLoadComponents(string attributeFileLocation)
        {
            Dictionary<String, String> extraViewData = new Dictionary<string, string>();
            foreach (BaseComponent component in components)
            {
                Dictionary<string, string> newViewData = component.PreLoad(GetAttributes(attributeFileLocation));
                if (newViewData != null)
                {
                    foreach (KeyValuePair<string, string> pair in newViewData)
                    {
                        //Note to self: need to identify a way to deal with duplicates? (Probably just add an error message)
                        extraViewData.Add(pair.Key, pair.Value);
                    }
                }
            }
            return extraViewData;
        }

        public string GetComponentsHTML()
        {
            string toDisplay = "";
            foreach (BaseComponent component in components)
            {
                toDisplay = toDisplay + "\n" + component.htmlContents;
            }
            return toDisplay;
        }

        public void SaveAttributes(Dictionary<string, string> attributes, string fileLocation)
        {
            string json = JsonSerializer.Serialize(attributes);
            System.IO.File.WriteAllText(fileLocation, json);
        }
    }

    public class BaseComponent
    {
        //Note to self: Add constructors with just html or attributes (though I think you could pass in an empty string or null)
        public BaseComponent() {
            attributeNames = new List<string>();
            htmlContents = "";
            componentIdentifier = "BaseComponent";
            persistentData = new Dictionary<string, string>();
        }

        public BaseComponent(string HTML, List<string> attributes)
        {
            htmlContents = HTML;
            attributeNames = attributes;
            componentIdentifier = "BaseComponent";
            persistentData = new Dictionary<string, string>();
        }

        public List<string> attributeNames { get; set; } //This should be set to the key of every attribute used by the component (Use of @ViewData["attributeName"])

        public string htmlContents { get; set; } //The HTML contents of the component

        public string componentIdentifier { get; set; }

        public Dictionary<string, string> persistentData { get; set; } //For data required for the COMPONENT which isn't data of the SHEET (For example which stat a stat component is (Not the value of the stat, which is an attribute, but which stat this component is))

        public virtual Dictionary<string, string> PreLoad(Dictionary<string, string> attributes) //Contains anything which needs to occur before loading HTML contents, such as any maths needed
        {
            return null;
        }
    }
}
