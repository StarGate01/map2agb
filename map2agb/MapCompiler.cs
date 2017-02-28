using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Data;
using map2agblib.Map;
using map2agblib.Map.Event;

namespace map2agb
{
    /// <summary>
    /// Static lass to compile a MapHeader instance
    /// </summary>
    class MapCompiler
    {
        /// <summary>
        /// Creates an assembly string of a given mapheader instance
        /// </summary>
        /// <param name="mapHeader"></param>
        /// <returns></returns>
        public static string ToAssemblyString(MapHeader mapHeader, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            b.Append("@ Map Assembly created by map2agb at ");
            b.Append(DateTime.Now.ToString());
            b.Append(Environment.NewLine);
            
            // Append the MapHeader structure
            b.Append(MapHeaderToString(mapHeader, baseSymbol));
            b.Append(Environment.NewLine);

            return b.ToString();
        }

        /// <summary>
        /// Transforms a MapHeader into an assembly string
        /// </summary>
        /// <param name="mapHeader"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static string MapHeaderToString(MapHeader mapHeader, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();

            // Create MapHeader structure
            b.Append("@ Section: MapHeader"); b.Append(Environment.NewLine);
            b.Append(".global mapheader_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapheader_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            b.Append("\t.word mapfooter_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapevents_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapscripts_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapconnections_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.hword ");  b.Append(Environment.NewLine);
            b.Append("\t.hword "); b.Append(mapHeader.Index.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapHeader.Name.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapHeader.Flash.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapHeader.Weather.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapHeader.Type.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapHeader.Unknown.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapHeader.BattleStyle.ToString()); b.Append(Environment.NewLine);

            // Append the MapFooter structure
            b.Append(Environment.NewLine);
            b.Append(MapFooterToString(mapHeader.Footer, baseSymbol));

            // Append the MapEvents structure
            b.Append(Environment.NewLine);
            b.Append(MapEventsToString(mapHeader.Events, baseSymbol));

            return b.ToString();
        }

        /// <summary>
        /// Transforms a MapFooter into an assembly string
        /// </summary>
        /// <param name="mapFooter"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static string MapFooterToString(MapFooter mapFooter, string baseSymbol)
        {

            StringBuilder b = new StringBuilder();
            // Create MapFooter structure
            b.Append("@ Section: MapFooter"); b.Append(Environment.NewLine);
            b.Append(".global mapfooter_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapfooter_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            b.Append("\t.word "); b.Append(mapFooter.Width.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.word "); b.Append(mapFooter.Height.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.word mapborderblocks_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapblocks_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word "); b.Append(mapFooter.FirstTilesetID); b.Append(Environment.NewLine);
            b.Append("\t.word 0"); b.Append(mapFooter.FirstTilesetID); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapFooter.BorderWidth.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(mapFooter.BorderHeight.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.hword "); b.Append(mapFooter.Padding); b.Append(Environment.NewLine);

            // Append the BorderBlocks
            b.Append(Environment.NewLine);
            b.Append(MapBorderBlocksToString(mapFooter, baseSymbol));

            //Append the MapBlocks
            b.Append(Environment.NewLine);
            b.Append(MapBlocksToString(mapFooter, baseSymbol));

            return b.ToString();
        }

        /// <summary>
        /// Transforms a MapFooter's BorderBlocks into an assembly string
        /// </summary>
        /// <param name="mapFooter"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static String MapBorderBlocksToString(MapFooter mapFooter, string baseSymbol)
        {

            StringBuilder b = new StringBuilder();
            b.Append("@ Section: BorderBlocks"); b.Append(Environment.NewLine);
            b.Append(".global mapborderblocks_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapborderblocks_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            for (int i = 0; i < mapFooter.BorderHeight; i++)
            {
                for(int j = 0; j < mapFooter.BorderWidth; j++)
                {
                    b.Append("\t.hword "); b.Append(mapFooter.BorderBlock[i][j].ToString()); b.Append(Environment.NewLine);
                }
            }

            return b.ToString();
        }

        /// <summary>
        /// Transforms a MapFooter's MapBlocks into an assembly string
        /// </summary>
        /// <param name="mapFooter"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static String MapBlocksToString(MapFooter mapFooter, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            b.Append("@ Section: MapBlocks"); b.Append(Environment.NewLine);
            b.Append(".global mapblocks_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapblocks_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            for (int i = 0; i < mapFooter.Height; i++)
            {
                for (int j = 0; j < mapFooter.Width; j++)
                {
                    b.Append("\t.hword "); b.Append(mapFooter.MapBlock[i][j].ToString()); b.Append(Environment.NewLine);
                }
            }

            return b.ToString();
        }

        /// <summary>
        /// Transforms an EventHeader into an assembly string
        /// </summary>
        /// <param name="eventHeader"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static String MapEventsToString(EventHeader eventHeader, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            // Create EventHeader structure
            b.Append("@ Section: MapEvents"); b.Append(Environment.NewLine);
            b.Append(".global mapevents_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapevents_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(eventHeader.Persons.Count.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(eventHeader.Warps.Count.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(eventHeader.Signs.Count.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.byte "); b.Append(eventHeader.ScriptTriggers.Count.ToString()); b.Append(Environment.NewLine);
            b.Append("\t.word mapevents_persons_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapevents_warps_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapevents_script_triggers_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("\t.word mapevents_signs_"); b.Append(baseSymbol); b.Append(Environment.NewLine);

            // Append the Persons
            b.Append(Environment.NewLine);
            b.Append(MapEventsPersonsToString(eventHeader.Persons, baseSymbol));

            // Append the Warps
            b.Append(Environment.NewLine);
            b.Append(MapEventsWarpsToString(eventHeader.Warps, baseSymbol));

            // Append the Triggers
            b.Append(Environment.NewLine);
            b.Append(MapEventsScriptsToString(eventHeader.ScriptTriggers, baseSymbol));

            // Append the Signs
            b.Append(Environment.NewLine);
            b.Append(MapEventsSignsToString(eventHeader.Signs, baseSymbol));

            return b.ToString();
        }

        /// <summary>
        /// Transforms a list of EventEntityPerson into an assembly string
        /// </summary>
        /// <param name="persons"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static String MapEventsPersonsToString(List<EventEntityPerson> persons, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            b.Append("@ Section: MapEvents, Persons"); b.Append(Environment.NewLine);
            b.Append(".global mapevents_persons_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapevents_persons_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            foreach (EventEntityPerson person in persons)
            {
                b.Append("@//new structure"); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Id.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Picture.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Field2.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Field3.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(person.X.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(person.Y.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Height.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Facing.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.Behaviour.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.FieldB.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.IsTrainer.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(person.FieldD.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(person.AlertRadius.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.word "); b.Append(person.Script); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(person.Flag.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(person.Padding.ToString()); b.Append(Environment.NewLine);
            }

            return b.ToString();
        }

        /// <summary>
        /// Transforms a list of EventEntityWarp into an assembly string
        /// </summary>
        /// <param name="warps"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static String MapEventsWarpsToString(List<EventEntityWarp> warps, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            b.Append("@ Section: MapEvents, Warps"); b.Append(Environment.NewLine);
            b.Append(".global mapevents_warps_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapevents_warps_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            foreach (EventEntityWarp warp in warps)
            {
                b.Append("@//new structure"); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(warp.X.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(warp.Y.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(warp.Height.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(warp.TargetWarp.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(warp.TargetBank.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(warp.TargetMap.ToString()); b.Append(Environment.NewLine);
            }

            return b.ToString();
        }

        /// <summary>
        /// Transforms a list of EventEntityTrigger into
        /// </summary>
        /// <param name="triggers"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static String MapEventsScriptsToString(List<EventEntityTrigger> triggers, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            b.Append("@ Section: MapEvents, Warps"); b.Append(Environment.NewLine);
            b.Append(".global mapevents_script_triggers_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapevents_script_triggers_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            foreach (EventEntityTrigger trigger in triggers)
            {
                b.Append("@//new structure"); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(trigger.X.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(trigger.Y.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(trigger.Height.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(trigger.Field5.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(trigger.Variable.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(trigger.Value.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(trigger.FieldA.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(trigger.FieldB.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.word "); b.Append(trigger.Script); b.Append(Environment.NewLine);
            }

            return b.ToString();
        }

        private static String MapEventsSignsToString(List<EventEntitySign> sings, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            b.Append("@ Section: MapEvents, Signs"); b.Append(Environment.NewLine);
            b.Append(".global mapevents_sings_"); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append("mapevents_sings_"); b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);
            foreach (EventEntitySign sing in sings)
            {
                b.Append("@//new structure"); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(sing.X.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(sing.Y.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(sing.Height.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.byte "); b.Append(sing.Type.ToString()); b.Append(Environment.NewLine);
                b.Append("\t.hword "); b.Append(sing.Unknown.ToString()); b.Append(Environment.NewLine);
                if (sing.Layout == EventEntitySign.SignType.Item)
                {
                    // Sing layout, so the item related fields are used for exporting
                    b.Append("\t.hword "); b.Append(sing.ItemId.ToString()); b.Append(Environment.NewLine);
                    b.Append("\t.byte "); b.Append(sing.HiddenId.ToString()); b.Append(Environment.NewLine);
                    // The last byte is a bitfield
                    byte lastMember = (byte)(sing.ItemCount | (sing.IsCoin ? 0x40 : 0x0) | (sing.DetectorDisabled ? 0x80 : 0x0));
                    b.Append("\t.byte "); b.Append(lastMember.ToString()); b.Append(Environment.NewLine);
                }
                else
                {
                    // Script layout, so the script related fields are used for exporting
                    b.Append("\t.hword "); b.Append(sing.ItemId.ToString()); b.Append(Environment.NewLine);
                    b.Append("\t.word "); b.Append(sing.Script); b.Append(Environment.NewLine);
                }
            }

            return b.ToString();
        }

    }
}
