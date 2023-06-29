using System.Collections.Generic;

public class Formation
{
    private ICharacter[] characters = new ICharacter[4];
    private EFaction faction;

    public List<ICharacter> AliveCharacters => GetAliveCharacters();
    public EFaction Faction => faction;

    public void Setup(CharacterConfig[] charactersSetup, EFaction faction)
    {
        this.faction = faction;

        EventBus.Send(new EventSetupFormation() { formation = this });

        for (int i = 0; i < 4; i++)
        {
            var characterConfig = charactersSetup.Length > i && charactersSetup[i] != null ? charactersSetup[i] : null;
            Character character = null;

            if (characterConfig != null)
            {
                character = new Character(characterConfig, this, i, faction);
                characters[i] = character;
            }

            EventBus.Send(new EventSetupFormationCharacter() { formation = this, character = character, location = i });
        }
    }

    public List<ICharacter> GetCharacters()
    {
        List<ICharacter> c = new List<ICharacter>();

        for (int i = 0; i < characters.Length; i++)
        {
            c.Add(characters[i]);   
        }

        return c;
    }

    public List<ICharacter> GetCharacters(bool[] locations)
    {
        List<ICharacter> c = new List<ICharacter>();

        for (int i = 0; i < characters.Length; i++)
        {
            if (locations[i])
                c.Add(characters[i]);
        }

        return c;
    }

    private List<ICharacter> GetAliveCharacters()
    {
        List<ICharacter> aliveCharacters = new();
        foreach (ICharacter character in characters)
        {
            if (character != null && character.IsAlive())
                aliveCharacters.Add(character);
        }

        return aliveCharacters;
    }
}