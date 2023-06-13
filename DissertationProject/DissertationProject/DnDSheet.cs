using DissertationProject;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Xml.Linq;

namespace DissertationProject
{
    //Note to self: Might change all the attribute names into an ENUM to ensure I don't make a typo somewhere
    //Note to self: Should add a way for components to check if other components exist (eg you should never have proficiency without level, or passive perception without a skill set to perception, etc)

    //Note to self: Need to do a "try catch" for anywhere I try and parse data
    //Note to self: Need to add a thing which adds empty values from the form into the json
    public class DnDSheet:Sheet
    {
        //Note to self: Need to add a function to generate a new sheet - creating save data jsons
        //Note to self: Need to look into if there's a more efficient/safe way of accessing attributes of non-string type than parsing them every time (Should probably save several attribute files - not sure why I'm not doing this already but that's a problem for future me)

        public DnDSheet()
        {
            //Below is just adding a bunch of attributes to the sheet
            //player name
            components.Add(new BaseComponent("<h3>Player Name</h3>", null));
            components.Add(new TextInputComponent("PlayerName"));
            //character name
            components.Add(new BaseComponent("<h3>Character Name</h3>", null));
            components.Add(new TextInputComponent("CharacterName"));
            //background
            components.Add(new BaseComponent("<h3>Background</h3>", null));
            components.Add(new TextInputComponent("Background"));
            //race
            components.Add(new BaseComponent("<h3>Race</h3>", null));
            components.Add(new TextInputComponent("Race"));
            //alignment
            List<string> alignmentOptions = new List<string>() { "Lawful Good", "Neutral Good", "Chaotic Good", "Lawful Neutral", "True Neutral", "Chaotic Neutral", "Lawful Evil", "Neutral Evil", "Chaotic Evil" };
            components.Add(new BaseComponent("<h3>Alignment</h3>", null));
            components.Add(new DropDownComponent("Alignment", alignmentOptions));
            //class
            List<string> classOptions = new List<string>() { "Barbarian", "Bard", "Cleric", "Druid", "Fighter", "Monk", "Paladin", "Ranger", "Rogue", "Sorcerer", "Warlock", "Wizard" };
            components.Add(new BaseComponent("<h3>Class</h3>", null));
            components.Add(new DropDownComponent("Class", classOptions));
            //level
            components.Add(new BaseComponent("<h3>Level</h3>", null));
            components.Add(new IntInputComponent("Level", 1, 20));
            //experience
            components.Add(new BaseComponent("<h3>EXP</h3>", null));
            components.Add(new IntInputComponent("EXP"));
            //proficiency bonus
            components.Add(new BaseComponent("<h3>Proficiency Bonus</h3>", null));
            components.Add(new ProficiencyComponent());
            //inspiration
            components.Add(new BaseComponent("<h3>Inspiration</h3>", null));
            components.Add(new BoolInputComponent("Inspiration"));

            //speed
            components.Add(new BaseComponent("<h3>Speed (in feet)</h3>", null));
            components.Add(new IntInputComponent("Speed"));
            //initiative
            components.Add(new BaseComponent("<h3>Initiative Modifier</h3>", null));
            components.Add(new InitiativeComponent());
            //armour class
            //components.Add(new BaseComponent("<h3>Armour Class</h3>", null));
            //components.Add(new ArmourClassComponent());
            //current hp
            components.Add(new BaseComponent("<h3>Current HP</h3>", null));
            components.Add(new IntInputComponent("HP"));
            //max hp
            components.Add(new BaseComponent("<h3>Max HP</h3>", null));
            components.Add(new IntInputComponent("MaxHP"));
            //temp hp
            components.Add(new BaseComponent("<h3>Temporary HP</h3>", null));
            components.Add(new IntInputComponent("TempHP"));
            //current hit dice
            components.Add(new BaseComponent("<h3>Current hit dice number</h3>", null));
            components.Add(new IntInputComponent("HitDice")); //Note to self: Should this be changed to be an int input version of the hit dice component?
            //max hit dice/hit dice type
            components.Add(new BaseComponent("<h3>Total Hit Dice</h3>", null));
            components.Add(new HitDiceComponent());
            //death saves
            components.Add(new BaseComponent("<h3>Death Saving Throws</h3>", null));
            components.Add(new DeathSavesComponent());
            //saving throws
            components.Add(new BaseComponent("<h3>Saving Throws</h3>", null));
            components.Add(new SkillsAndSavesComponent("Strength", "Strength"));
            components.Add(new SkillsAndSavesComponent("Dexterity", "Dexterity"));
            components.Add(new SkillsAndSavesComponent("Constitution", "Constitution"));
            components.Add(new SkillsAndSavesComponent("Intelligence", "Intelligence"));
            components.Add(new SkillsAndSavesComponent("Wisdom", "Wisdom"));
            components.Add(new SkillsAndSavesComponent("Charisma", "Charisma"));
            //weapons
            components.Add(new BaseComponent("<h3>Weapons</h3>", null));
            components.Add(new WeaponComponent("Weapon1", this, new List<string> { "Weapon"}));
            //stats
            components.Add(new BaseComponent("<h3>Stats</h3>", null));
            components.Add(new StatComponent("Strength"));
            components.Add(new StatComponent("Dexterity"));
            components.Add(new StatComponent("Constitution"));
            components.Add(new StatComponent("Intelligence"));
            components.Add(new StatComponent("Wisdom"));
            components.Add(new StatComponent("Charisma"));
            //skills
            components.Add(new BaseComponent("<h3>Skills</h3>", null));
            components.Add(new SkillsAndSavesComponent("Acrobatics (Dex)", "Dexterity"));
            components.Add(new SkillsAndSavesComponent("Animal Handling (Wis)", "Wisdom"));
            components.Add(new SkillsAndSavesComponent("Arcana (Int)", "Intelligence"));
            components.Add(new SkillsAndSavesComponent("Athletics (Str)", "Strength"));
            components.Add(new SkillsAndSavesComponent("Deception (Cha)", "Deception"));
            components.Add(new SkillsAndSavesComponent("History (Int)", "History"));
            components.Add(new SkillsAndSavesComponent("Insight (Wis)", "Wisdom"));
            components.Add(new SkillsAndSavesComponent("Intimidation (Cha)", "Charisma"));
            components.Add(new SkillsAndSavesComponent("Investigation (Int)", "Intelligence"));
            components.Add(new SkillsAndSavesComponent("Medicine (Wis)", "Wisdom"));
            components.Add(new SkillsAndSavesComponent("Nature (Int)", "Intelligence"));
            components.Add(new SkillsAndSavesComponent("Perception (Wis)", "Wisdom"));
            components.Add(new SkillsAndSavesComponent("Performance (Cha)", "Charisma"));
            components.Add(new SkillsAndSavesComponent("Persuasion (Cha)", "Charisma"));
            components.Add(new SkillsAndSavesComponent("Religion (Int))", "Intelligence"));
            components.Add(new SkillsAndSavesComponent("Sleight of Hand (Dex)", "Dexterity"));
            components.Add(new SkillsAndSavesComponent("Stealth (Dex)", "Dexterity"));
            components.Add(new SkillsAndSavesComponent("Survival (Wis)", "Wisdom"));
            //passive perception
            components.Add(new BaseComponent("<h3>Passive Perception</h3>", null));
            components.Add(new PassivePerceptionComponent());


        }

        //Note to self: For construction of classes JUST for the preload should maybe create a new constructer - or even make the preload disconnected from the component?
        //Basically need to rethink how it currently works, but just getting this to the point where it *does* work before trying to restructure
        public override Dictionary<String, String> PreLoadComponents(string attributeFileLocation)
        {
            Dictionary<string, string> attributes = GetAttributes(attributeFileLocation);
            Dictionary<string, string> extraViewData = new Dictionary<string, string>();
            Dictionary<string, string> newViewData;
            foreach (BaseComponent component in components)
            {
                if (component.componentIdentifier == "Proficiency")
                {
                    newViewData = new ProficiencyComponent().PreLoad(attributes);
                }
                else if (component.componentIdentifier == "HitDice")
                {
                    newViewData = new HitDiceComponent().PreLoad(attributes);
                }
                else if (component.componentIdentifier == "SkillAndSave")
                {
                    newViewData = new SkillsAndSavesComponent(component.persistentData["Name"], component.persistentData["Stat"]).PreLoad(attributes);
                }
                else if (component.componentIdentifier == "Stat")
                {
                    newViewData = new StatComponent(component.persistentData["Name"]).PreLoad(attributes);
                }
                else if (component.componentIdentifier == "PassivePerception")
                {
                    newViewData = new PassivePerceptionComponent().PreLoad(attributes);
                }
                else if (component.componentIdentifier == "Initiative")
                {
                    newViewData = new InitiativeComponent().PreLoad(attributes);
                }
                else if (component.componentIdentifier == "Bool")
                {
                    newViewData = new BoolInputComponent(component.persistentData["Name"]).PreLoad(attributes);
                }
                else if (component.componentIdentifier == "DropDown")
                {
                    List<string> options = JsonSerializer.Deserialize<List<string>>(component.persistentData["OptionsJson"]);
                    newViewData = new DropDownComponent(component.persistentData["Name"], options).PreLoad(attributes);
                }
                else if (component.componentIdentifier == "DeathSaves")
                {
                    newViewData = new DeathSavesComponent().PreLoad(attributes);
                }
                else if (component.componentIdentifier == "Weapon")
                {
                    List<string> weapons = JsonSerializer.Deserialize<List<string>>(component.persistentData["Weapons"]);
                    newViewData = new WeaponComponent(component.persistentData["Name"], this, weapons).PreLoad(attributes);
                }
                else
                {
                    newViewData = new BaseComponent(component.htmlContents, component.attributeNames).PreLoad(attributes);
                }

                //Add all the gathered viewdata to the dictionary to be returned
                if (newViewData != null)
                {
                    foreach (KeyValuePair<string, string> pair in newViewData)
                    {
                        extraViewData.Add(pair.Key, pair.Value);
                    }
                }
                newViewData = new Dictionary<string, string>();
            }
            return extraViewData;
        }
    }
}

//Unique components - ideally need a way to script this without having the whole project
public class ProficiencyComponent:BaseComponent
{
    public ProficiencyComponent()
    {
        htmlContents = "<p>@ViewData[\"ProficiencyBonus\"]</p>";
        componentIdentifier = "Proficiency";
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        //Converts the level attribute into an integer
        int level = Int32.Parse(attributes["Level"]);
        //Created the dictionary for
        Dictionary<string, string> returnData = new Dictionary<string, string>();

        //Proficiency bonus is determined based on table, this information is from the D&D5e player's handbook and can also be viewed for free on Roll20: https://roll20.net/compendium/dnd5e/Character%20Advancement#content
        //If the attribute is not an int, or is not present, no if statements will occur and the function will return an empty dictionary
        if (level >= 17)
        {
            returnData.Add("ProficiencyBonus", "+6");
        }
        else if (level >= 13)
        {
            returnData.Add("ProficiencyBonus", "+5");
        }
        else if (level >= 9)
        {
            returnData.Add("ProficiencyBonus", "+4");
        }
        else if (level >= 5)
        {
            returnData.Add("ProficiencyBonus", "+3");
        }
        else if (level >= 1)
        {
            returnData.Add("ProficiencyBonus", "+2");
        }
        else
        {
            returnData.Add("ProficiencyBonus", "+2");
        }
        return returnData;
    }
}


public class HitDiceComponent:BaseComponent
{
    public HitDiceComponent()
    {
        //D&D 5e characters have a number of hit dice equal to their level (x) and a type of hit dice deternmined by their class (y)
        //This is displayed in the style "xdy"
        htmlContents = "<p> @ViewData[\"Level\"]d@ViewData[\"HitDiceType\"]</p>";
        componentIdentifier = "HitDice";
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        Dictionary<string, string> returnData = new Dictionary<string, string>();

        //As hit dice type is determined by class this can be determined automatically based on the inputs for the class field
        if (attributes["Class"] == "Sorcerer" || attributes["Class"] == "Wizard")
        {
            returnData.Add("HitDiceType", "6");
        }
        else if (attributes["Class"] == "Bard" || attributes["Class"] == "Cleric" || attributes["Class"] == "Druid" || attributes["Class"] == "Monk" || attributes["Class"] == "Rogue" || attributes["Class"] == "Warlock")
        {
            returnData.Add("HitDiceType", "8");
        }
        else if (attributes["Class"] == "Fighter" || attributes["Class"] == "Paladin" || attributes["Class"] == "Ranger")
        {
            returnData.Add("HitDiceType", "10");
        }
        else if (attributes["Class"] == "Barbarian")
        {
            returnData.Add("HitDiceType", "12");
        }
        return returnData;
    }
}

public class DeathSavesComponent : BaseComponent
{
    public DeathSavesComponent()
    {
        //Two sets of death savings throw markers (One for successes one for failures) each on their own line
        //The final empty paragraph is in order to reset the style and therefore put continued stuff on seperate lines (There may be a more elegant solution to this but it works)
        htmlContents = """
            <p style = "margin:0; display: inline; 1:left">Successes</p>
            <input type="checkbox" id= "SucceededDeathSave1" name = "SucceededDeathSave1" @ViewData["SucceededDeathSave1Checked"] style ="margin:0; display: inline; 1:left">
            <input type="checkbox" id= "SucceededDeathSave2" name = "SucceededDeathSave2" @ViewData["SucceededDeathSave2Checked"] style ="margin:0; display: inline; 1:left">
            <input type="checkbox" id= "SucceededDeathSave3" name = "SucceededDeathSave3" @ViewData["SucceededDeathSave3Checked"] style ="margin:0; display: inline; 1:left">
            <p style = "margin:0; display: inline; 1:left">Failures</p>
            <input type="checkbox" id= "FailedDeathSave1" name = "FailedDeathSave1" @ViewData["FailedDeathSave1Checked"] style ="margin:0; display: inline; 1:left">
            <input type="checkbox" id= "FailedDeathSave2" name = "FailedDeathSave2" @ViewData["FailedDeathSave2Checked"] style ="margin:0; display: inline; 1:left">
            <input type="checkbox" id= "FailedDeathSave3" name = "FailedDeathSave3" @ViewData["FailedDeathSave3Checked"] style ="margin:0; display: inline; 1:left">
            <p></p>
            """;
        attributeNames = new List<string>();
        attributeNames.Add("SucceededDeathSave1");
        attributeNames.Add("SucceededDeathSave2");
        attributeNames.Add("SucceededDeathSave3");
        attributeNames.Add("FailedDeathSave1");
        attributeNames.Add("FailedDeathSave2");
        attributeNames.Add("FailedDeathSave3");
        componentIdentifier = "DeathSaves";
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        if (attributes.ContainsKey("SucceededDeathSave1") && attributes["SucceededDeathSave1"] == "on")
            returnValue.Add("SucceededDeathSave1Checked", "checked");
        else
            returnValue.Add("SucceededDeathSave1Checked", "");

        if (attributes.ContainsKey("SucceededDeathSave2") && attributes["SucceededDeathSave2"] == "on")
            returnValue.Add("SucceededDeathSave2Checked", "checked");
        else
            returnValue.Add("SucceededDeathSave2Checked", "");

        if (attributes.ContainsKey("SucceededDeathSave3") && attributes["SucceededDeathSave3"] == "on")
            returnValue.Add("SucceededDeathSave3Checked", "checked");
        else
            returnValue.Add("SucceededDeathSave3Checked", "");

        if (attributes.ContainsKey("FailedDeathSave1") && attributes["FailedDeathSave1"] == "on")
            returnValue.Add("FailedDeathSave1Checked", "checked");
        else
            returnValue.Add("FailedDeathSave1Checked", "");

        if (attributes.ContainsKey("FailedDeathSave2") && attributes["FailedDeathSave2"] == "on")
            returnValue.Add("FailedDeathSave2Checked", "checked");
        else
            returnValue.Add("FailedDeathSave2Checked", "");

        if (attributes.ContainsKey("FailedDeathSave3") && attributes["FailedDeathSave3"] == "on")
            returnValue.Add("FailedDeathSave3Checked", "checked");
        else
            returnValue.Add("FailedDeathSave3Checked", "");

        return returnValue;
    }
}

public class SkillsAndSavesComponent:BaseComponent
{

    public SkillsAndSavesComponent(string name, string stat) //THIS IS WRONG CURRENTLY IT IS ADDING THE WHOLE STAT NOT THE STAT MODIFIER
    {
        //Note to self: Need a way to verify a valid stat is entered, maybe an error message or maybe use an enum or maybe a dictionary? Dictionary probably makes the most sense but I'm keeping like this until I check everything works

        //Displays the name of the skill/saving throw, a checkbox for whether you are "proficient" in it or not, and then displays the value

        persistentData = new Dictionary<string, string>();
        persistentData.Add("Name", name);
        persistentData.Add("Stat", stat);
        attributeNames = new List<string>();
        attributeNames.Add(name + "Proficiency");
        componentIdentifier = "SkillAndSave";
        htmlContents = "<input type=\"checkbox\" id= \"" + persistentData["Name"] + "Proficiency\" name = \"" + persistentData["Name"] + "Proficiency\" style =\"margin:0; display: inline; 1:left\" @ViewData[\"" + name + "IsChecked\"]>"
    + "<p style = \"margin:0; display: inline; 1:left\"> " + persistentData["Name"] + " </p>"
            + "<p style = \"margin:0; display: inline; 1:left\">@ViewData[\"" + persistentData["Name"] + "Value\"]</p><p></p>";
    }


    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        bool isProficient;
        //Calculate value and return it
        if (attributes.ContainsKey(persistentData["Name"] + "Proficiency") && attributes[persistentData["Name"] + "Proficiency"] == "on")
            isProficient = true;
        else
            isProficient = false;
        int value = 0;

        if (isProficient)
        {
            //If the character is proficient in a skill/saving throw it's value is equal to their proficiency bonus + the relevant stat modifier (calculated by subtracting 10 and dividing by 2 (rounding down))
            //Proficiency is calculated from level
            int level = Int32.Parse(attributes["Level"]);
            int proficiencyBonus = 0;
            if (level >= 17)
            {
                proficiencyBonus = 6;
            }
            else if (level >= 13)
            {
                proficiencyBonus = 5;
            }
            else if (level >= 9)
            {
                proficiencyBonus = 4;
            }
            else if (level >= 5)
            {
                proficiencyBonus = 3;
            }
            else if (level >= 1)
            {
                proficiencyBonus = 2;
            }
            if (attributes.ContainsKey(persistentData["Stat"]))
                value = (int)MathF.Floor((Int32.Parse(attributes[persistentData["Stat"]]) - 10) / 2) + proficiencyBonus;
        }
        else
        {
            //If the character is not proficient in a skill/saving throw the value is equal to the relevant stat
            if (attributes.ContainsKey(persistentData["Stat"]))
                value = (int)MathF.Floor((Int32.Parse(attributes[persistentData["Stat"]]) - 10) / 2);
        }
        returnValue.Add(persistentData["Name"] + "Value", value.ToString());

        if (attributes.ContainsKey(persistentData["Name"] + "Proficiency") && attributes[persistentData["Name"] + "Proficiency"] == "on")
            returnValue.Add(persistentData["Name"] + "IsChecked", "checked");
        else
            returnValue.Add(persistentData["Name"] + "IsChecked", "");

        return returnValue;
    }
}

public class WeaponComponent : BaseComponent
{
    //Might need to pass in the sheet so it can add a component, issue with this is the component will be in the wrong place so might need to sort through the list and find the current component's position then insert the new one after
    public WeaponComponent(string componentName, Sheet sheet, List<string> weaponsAndNames)
    {
        persistentData.Add("Name", componentName);
        persistentData.Add("Sheet", JsonSerializer.Serialize(sheet));
        persistentData.Add("Weapons", JsonSerializer.Serialize(weaponsAndNames));

        //Need to add style to make this one line
        htmlContents = "@ViewData[\"Test\"]";

        foreach (string weapons in weaponsAndNames)
        {
            attributeNames.Add(componentName + weapons + "Name");
            attributeNames.Add(componentName + weapons + "Finesse");
            attributeNames.Add(componentName + weapons + "DiceNumber");
            attributeNames.Add(componentName + weapons + "DiceType");
            attributeNames.Add(componentName + weapons + "New");
            attributeNames.Add(persistentData["Name"] + weapons + "Modifier");
            attributeNames.Add(persistentData["Name"] + weapons + "Checked");
        }

        componentIdentifier = "Weapon";
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
         string newHtml = "";
        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        foreach (string weapon in JsonSerializer.Deserialize<List<String>>(persistentData["Weapons"]))
        {
            string modifier = "";
            string finesseChecked = "";
            string nameAttribute = "";
            string diceNumber = "";
            string diceType = "";

            if (attributes.ContainsKey(persistentData["Name"] + weapon + "Finesse") && attributes[persistentData["Name"] + weapon + "Finesse"] == "on")
            {
                if (attributes.ContainsKey("Dexterity"))
                {
                    returnValue.Add(persistentData["Name"] + weapon + "Modifier", attributes["Dexterity"]);
                    modifier = attributes["Dexterity"];
                }
                finesseChecked = "checked";
            }
            else if (attributes.ContainsKey("Strength"))
                modifier = attributes["Strength"];
            else
                modifier = "0";

            if (attributes.ContainsKey(persistentData["Name"] + weapon + "Name"))
                nameAttribute = attributes[persistentData["Name"] + weapon + "Name"];
            if (attributes.ContainsKey(persistentData["Name"] + weapon + "DiceNumber"))
                diceNumber = attributes[persistentData["Name"] + weapon + "DiceNumber"];
            if (attributes.ContainsKey(persistentData["Name"] + weapon + "DiceType"))
                diceType = attributes[persistentData["Name"] + weapon + "DiceType"];

            //Need to get each attribute one at a time and check they exist



            newHtml = newHtml + "<input type=\"text\" class=\"form-control\" name=\"" + persistentData["Name"] + weapon + "Name\" value= " + nameAttribute + ">" +
            "<input type=\"checkbox\" id= \"" + persistentData["Name"] + weapon + "Finesse\" name=\"" + persistentData["Name"] + weapon + "Finesse\"" + finesseChecked+ ">" +
            "<input type=\"number\" class=\"form-control\" name=\"" + persistentData["Name"] + weapon + "DiceNumber\" value= " + diceNumber + " step=\"1\">" +
            "<p>d</p>" +
            "<input type=\"number\" class=\"form-control\" name=\"" + persistentData["Name"] + weapon + "DiceType\" value= " + diceType + "step=\"1\">" +
            "<p> + " + modifier + "</p>" +
            "<input type=\"checkbox\" id= \"" + persistentData["Name"] + weapon + "New\" name=\"" + persistentData["Name"] + weapon + "New\" checked\"]>";




            if (attributes.ContainsKey(persistentData["Name"] + weapon + "New") && attributes[persistentData["Name"] + weapon + "New"] == "on")
                CreateNewWeapon();
        }

        returnValue.Add("Test", newHtml);
        return returnValue;
    }

    public void CreateNewWeapon()
    {
        List<string> weapons = JsonSerializer.Deserialize<List<string>>(persistentData["Weapons"]);
        string newName = weapons[weapons.Count - 1] + "1";
        weapons.Add(weapons[weapons.Count - 1] + "1");
        persistentData["Weapons"] = JsonSerializer.Serialize(weapons);

        attributeNames.Add(persistentData["Name"] + newName + "Name");
        attributeNames.Add(persistentData["Name"] + newName + "Finesse");
        attributeNames.Add(persistentData["Name"] + newName + "DiceNumber");
        attributeNames.Add(persistentData["Name"] + newName + "DiceType");
        attributeNames.Add(persistentData["Name"] + newName + "New");
    }
}


public class StatComponent:BaseComponent
{
    public StatComponent(string name)
    {
        //Displays the name of the stat, then a number input for the stat with a minimum of 0, then displays the "modifier" of the stat
        //The <p></p> at the end is to reset the style so the rest of the sheet is on seperate lines
        htmlContents = "<p style = \"margin:0; display: inline; 1:left\">" + name + " value: </p>"
            + "<input type =\"text\" class=\"form-control\" name =\"" + name + "\" value = @ViewData[\"" + name + "\"] step=\"1\" min=\"0\">"
            + "<p style = \"margin:0; display: inline; 1:left\">" + name + " modifier: @ViewData[\"" + name + "Modifier\"]</p>"
            + "<p></p>";
        attributeNames = new List<string>();
        attributeNames.Add(name);
        componentIdentifier = "Stat";
        persistentData = new Dictionary<string, string>();
        persistentData.Add("Name", name);
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        //The modifier of a stat is calculated by dividing the value by subtracting 10 and dividing by 2 (rounding down) - this is the actual value added to most abilities
        int value = 0;
        if (attributes.ContainsKey(persistentData["Name"]))
        {
            value = Int32.Parse(attributes[persistentData["Name"]]);
            value = (int)MathF.Floor((value - 10) / 2);
        }

        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        returnValue.Add(persistentData["Name"] + "Modifier", value.ToString());
        return returnValue;
    }
}

public class PassivePerceptionComponent:BaseComponent
{
    public PassivePerceptionComponent()
    {
        //Displays number calculated from whether perception is proficient, proficiency bonus and wisdom bonus

        //A character's passive perception is calculated by adding 10 to their perception bonus
        //Perception bonus is equal to their wisdom modifier + their proficiency bonus (if they are proficient in perception)
        htmlContents = "<p> @ViewData[\"PassivePerception\"]</p>";
        componentIdentifier = "PassivePerception";
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        int value;

        //To see why these values are calculated like this please see the skill component
        int proficiencyBonus = 0;
        int statBonus = 0;

        if (attributes.ContainsKey("Perception (Wis)Proficiency") && attributes["Perception (Wis)Proficiency"] == "on")
        {
            if (attributes.ContainsKey("Level"))
            {
                int level = Int32.Parse(attributes["Level"]);
                
                if (level >= 17)
                {
                    proficiencyBonus = 6;
                }
                else if (level >= 13)
                {
                    proficiencyBonus = 5;
                }
                else if (level >= 9)
                {
                    proficiencyBonus = 4;
                }
                else if (level >= 5)
                {
                    proficiencyBonus = 3;
                }
                else if (level >= 1)
                {
                    proficiencyBonus = 2;
                }
            }
        }
        if (attributes.ContainsKey("Wisdom"))
            statBonus = (int)MathF.Floor((Int32.Parse(attributes["Wisdom"]) - 10) / 2);

        value = 10 + proficiencyBonus + statBonus;
        returnValue.Add("PassivePerception", value.ToString());
        return returnValue;
    }
}

public class ExtraProficiencyComponent:BaseComponent
{
    public ExtraProficiencyComponent()
    {
        //Text input only
        //Button to add a new extrapfociencycomponent
    }
}

public class EquipmentComponent : BaseComponent
{
    public EquipmentComponent()
    {
        //Name
        //Bool input for if it's armour
        //IF IT IS ARMOUR int input for AC bonus
        //Bool for equipped
        //int for weight
        //Button to add new component
    }
}

public class InitiativeComponent : BaseComponent
{
    public InitiativeComponent()
    {
        htmlContents = "<p>@ViewData[\"Initiative\"]</p>";
        componentIdentifier = "Initiative";
    }

    public override Dictionary<string, string> PreLoad(Dictionary<string, string> attributes)
    {
        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        int value = 0;
        if (attributes.ContainsKey("Dexterity"))
            value = (int)MathF.Floor((Int32.Parse(attributes["Dexterity"]) - 10) / 2);
        returnValue.Add("Initiative", value.ToString());
        return returnValue;
    }
}