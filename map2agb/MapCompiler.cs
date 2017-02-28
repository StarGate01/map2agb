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
        public string ToAssemblyString(MapHeader mapHeader, string baseSymbol)
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
        private string MapHeaderToString(MapHeader mapHeader, string baseSymbol)
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

            return b.ToString();
        }

        /// <summary>
        /// Transforms a MapFooter into an assembly string
        /// </summary>
        /// <param name="mapFooter"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private string MapFooterToString(MapFooter mapFooter, string baseSymbol)
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
            b.Append("\t.word 0"); b.Append(Environment.NewLine);//Todo export tileset identifier b.App
            b.Append("\t.word 0"); b.Append(Environment.NewLine);//Todo export tileset identifier b.App
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
        private String MapBorderBlocksToString(MapFooter mapFooter, string baseSymbol)
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
        private String MapBlocksToString(MapFooter mapFooter, string baseSymbol)
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
        private String MapEventsToString(EventHeader eventHeader, string baseSymbol)
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

            //Append the Persons
            b.Append(Environment.NewLine);
            b.Append(MapEventsPersonsToString(eventHeader.Persons, baseSymbol));

            return b.ToString();
        }

        /// <summary>
        /// Transforms a list of EventEntityPerson into an assembly string
        /// </summary>
        /// <param name="persons"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private String MapEventsPersonsToString(List<EventEntityPerson> persons, string baseSymbol)
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

    }
}
