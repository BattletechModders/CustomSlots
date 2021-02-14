﻿using System.Collections.Generic;
using System.Linq;
using BattleTech;
using BattleTech.UI;
using CustomComponents;
using FluffyUnderware.DevTools.Extensions;

namespace CustomSlots
{
    [CustomComponent("SlotExtenstion")]
    public class CustomSlotExtenstion : SimpleCustomComponent, IUseSlots
    {
        public string SlotName { get; set; }
        public bool UseSupport { get; set; } = true;

        public int GetSlotsUsed(MechDef mech)
        {
            return 1;
        }

        public int GetSupportUsed(MechDef mech)
        {
            return UseSupport ? 1 : 0;
        }


        public int GetSlotsUsed(MechDef mech, IEnumerable<inventory_item> inventory)
        {
            return 1;
        }

        public int GetSupportUsed(MechDef mech, IEnumerable<inventory_item> inventory)
        {
            return UseSupport ? 1 : 0;
        }

    }

    public abstract class CustomSlotDynamic : CustomSlotInfo
    {
        public string ExtentionID { get; set; }
        public ComponentType ExtentionType { get; set; } = ComponentType.Upgrade;

        public abstract int ExtentionCount(MechDef mech, IEnumerable<inventory_item> inventory);
        public abstract int ExtentionCount(MechDef mech);
        public bool ForceAnotherLocation = false;


        public override void OnInstalled(WorkOrderEntry_InstallComponent order, SimGameState state, MechDef mech)
        {
            var inventory = mech.Inventory.ToList();
            
            var locations =
                CustomSlotControler.AdjustDynamics(mech, CustomSlotControler.GetExtentions(mech), state, inventory);
            locations.Set(order.DesiredLocation);
            locations.Set(order.PreviousLocation);
            var info = order.MechComponentRef.GetComponent<IUseSlots>();
            foreach (var location in CustomSlotControler.all_locations)
            {
                if (locations.HasFlag(location))
                    CustomSlotControler.AdjustMechDefaults(mech, state, info?.SlotName ?? "error", inventory, location);
            }
            mech.SetInventory(inventory.ToArray());
        }

        public override void OnItemGrabbed(IMechLabDraggableItem item, MechLabPanel mechLab, MechLabLocationWidget widget)
        {
            if (ExtentionCount(mechLab.activeMechDef) > 0)
            {

            }

            base.OnItemGrabbed(item, mechLab, widget);
        }
    }
}