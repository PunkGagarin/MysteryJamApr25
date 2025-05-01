using Jam.Scripts.Ritual.Inventory.Reagents;

namespace Jam.Scripts.Ritual.Desk
{
    public interface IDisk
    {
        bool TryInsertReagent(ReagentDefinition reagentDefinition, ReagentRoom returnRoom);
    }
}