using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElleWorld.Core.Costants.Net
{
    public enum ClientMessage : ushort
    {
        ConnectToFailed = 0x2000,
        GenerateRandomCharacterName = 0x02F4,
        EnumCharacters = 0x09FB,
        ReorderCharacters = 0x2000,
        LoadingScreenNotify = 0x1428,
        CreateCharacter = 0x143B,
        CharCustomize = 0x2000,
        CharRaceOrFactionChange = 0x2000,
        CharDelete = 0x093C,
        LiveRegionGetAccountCharacterList = 0x2000,
        LiveRegionCharacterCopy = 0x2000,
        LiveRegionAccountRestore = 0x2000,
        CharacterRenameRequest = 0x2000,
        Tutorial = 0x2000,
        EnumCharactersDeletedByClient = 0x2000,
        UndeleteCharacter = 0x2000,
        GetUndeleteCharacterCooldownStatus = 0x2000,
        QueuedMessageEnd = 0x0878,
        CastSpell = 0x00F5,
    }
}
