using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DissertationProject.Pages
{
    //Note to self: Need to make this all generic so I can create a page to load a sheet from a list/by giving file directory/with a file input
    public class IndexModel : PageModel
    {
        DnDSheet characterSheet;
        string attributeFileLocation = @".\testSheetAttributes.json"; //Include file name and extension
        string sheetFileLocation = @".\testSheet.json"; //Include file name and extension

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            GenerateCharacterSheet5e(); //Need to automate this happening if necessary
            LoadSheet(sheetFileLocation);
            if (characterSheet != null)
            {
                LoadAttributes(characterSheet.PreLoadComponents(attributeFileLocation), attributeFileLocation);
                string HTML = characterSheet.GetComponentsHTML();
                HTML = ReplaceViewData(HTML);
                ViewData["CharacterSheet"] = HTML;
            }
        }

        public void OnPost() 
        {
            SaveAttributes(sheetFileLocation, attributeFileLocation);
            OnGet();
        }

        //Note to self: Need to establish a version of this and LoadSheet which just uses the name of the sheet to automatically find it
        public void LoadAttributes(Dictionary<string, string> extraViewData, string fileLocationNameAndExtension)
        {
            string json = System.IO.File.ReadAllText(fileLocationNameAndExtension);
            Dictionary<string, string> attributes = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            foreach (KeyValuePair<string, string> pair in attributes)
            {
                ViewData[pair.Key] = pair.Value;
            }

            if (extraViewData != null)
            {
                foreach (KeyValuePair<string, string> pair in extraViewData)
                {
                    ViewData[pair.Key] = pair.Value;
                }
            }
        }

        public void LoadSheet(string fileLocationNameAndExtension)
        {
            string json = System.IO.File.ReadAllText(fileLocationNameAndExtension);
            characterSheet = JsonSerializer.Deserialize<DnDSheet>(json);
        }

        public string ReplaceViewData(string HTML)
        {
            string newHTML = "";
            string viewDataKey = "";
            int previousChunkStart = 0;

            int viewDataStart = -1;
            bool collectingViewData = false;
            for (int i = 0; i < HTML.Length; i++)
            {
                if (collectingViewData)
                {
                    if (HTML[i] == '"')
                    {
                        collectingViewData = false;
                        previousChunkStart = i + 2;
                        newHTML = newHTML + ViewData[viewDataKey];
                        viewDataKey = "";
                    }
                    else
                    {
                        viewDataKey = viewDataKey + HTML[i];
                    }
                }
                else if (i == viewDataStart)
                {
                    collectingViewData = true;
                    viewDataKey = viewDataKey + HTML[i];
                }
                else if (HTML[i] == '@'
                    && HTML[i + 1] == 'V'
                    && HTML[i + 2] == 'i'
                    && HTML[i + 3] == 'e'
                    && HTML[i + 4] == 'w'
                    && HTML[i + 5] == 'D'
                    && HTML[i + 6] == 'a'
                    && HTML[i + 7] == 't'
                    && HTML[i + 8] == 'a'
                    && !collectingViewData)
                {
                    for (int j = previousChunkStart; j < i; j++)
                    {
                        newHTML = newHTML + HTML[j];
                    }
                    viewDataStart = i + 11;
                }
                else if (i == HTML.Length - 1)
                {
                    for (int j = previousChunkStart; j < i; j++)
                    {
                        newHTML = newHTML + HTML[j];
                    }
                }
            }

            return newHTML;
        }

        public void SaveAttributes(string sheetLocationAndFileName, string attributesLocationAndFileName)
        {
            LoadSheet(sheetLocationAndFileName);
            Dictionary<string, string> stringAttributes = new Dictionary<string, string>();

            foreach (BaseComponent comp in characterSheet.components)
            {
                if (comp.attributeNames != null)
                {
                    foreach (string attribute in comp.attributeNames)
                    {
                        string value = Request.Form[attribute];
                        if (value != null)
                        {
                            stringAttributes.Add(attribute, value);
                        }
                    }
                }
            }

            string json = JsonSerializer.Serialize(stringAttributes);
            System.IO.File.WriteAllText(attributesLocationAndFileName, json);
        }

        public void GenerateCharacterSheet5e()
        {
            characterSheet = new DnDSheet();

            string json = JsonSerializer.Serialize(characterSheet);
            System.IO.File.WriteAllText(sheetFileLocation, json);
        }
    }
}