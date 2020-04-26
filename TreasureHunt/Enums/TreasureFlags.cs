using System;

namespace TreasureHunt.Enums
{
    [Flags]
    public enum TreasureFlags
    {
        None            = 0,
        RevealedNote    = 1 << 0,
        FoundNote       = 1 << 1,
        FoundCorpse     = 1 << 2,
        FoundShovel     = 1 << 3,
        FoundEmptyChest = 1 << 4,
        FoundFinalChest = 1 << 5
    }
}
