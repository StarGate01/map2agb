using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Map.Event
{
    /// <summary>
    /// Represents a Sign, sometimes called Signpost and specifies its attributes
    /// </summary>
    public class EventEntitySign : IEventEntity
    {
        #region Enums
        public enum SignType
        {
            Script,
            Item
        }
        #endregion

        #region Fields
        private string _script;

        private ushort _itemId;
        private byte _hiddenId;
        private byte _count;
        private bool _detectorDisabled;
        private bool _isCoin;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Height of the Sign (That is, the "Level", used for collision detection)
        /// </summary>
        public byte Height { get; set; }

        /// <summary>
        /// Gets or sets the X Coordinate of the Sign
        /// </summary>
        public short X { get; set; }

        /// <summary>
        /// Gets or sets the Y Coordinate of the Sign
        /// </summary>
        public short Y { get; set; }

        /// <summary>
        /// Gets or sets the Type of the Sign
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// Gets or sets an Unknown Value, probably a Padding
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// Gets or sets the InternalScript of the Signpost, currently only used while importing
        /// from a ROM File
        /// </summary>
        public uint InternalScript { get; set; }


        /// <summary>
        /// Gets the Layout, determined by the Type of the Sign Entity
        /// </summary>
        public SignType Layout
        {
            get
            {
                return Type >= 5 ? SignType.Item : SignType.Script;
            }
        }

        /// <summary>
        /// Gets or sets the Script of the Sign. Throws InvalidOperationException if the Layout is not SignType.Script
        /// </summary>
        public string Script
        {
            get
            {
                return _script;
            }
            set
            {
                if (Layout == SignType.Script)
                {
                    _script = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the ItemId of the Sign. Throws InvalidOperationException if the Layout is not SignType.Item
        /// </summary>
        public ushort ItemId
        {
            get
            {
                return _itemId;
            }
            set
            {
                if (Layout == SignType.Item)
                    _itemId = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the HiddenId of the Sign. Throws InvalidOperationException if the Layout is not SignType.Item
        /// </summary>
        public byte HiddenId
        {
            get
            {
                return _hiddenId;
            }
            set
            {
                if (Layout == SignType.Item)
                    _hiddenId = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the ItemCount of the Sign. Throws InvalidOperationException if the Layout is not SignType.Item
        /// </summary>
        public byte ItemCount
        {
            get
            {
                return _count;
            }
            set
            {
                if (Layout == SignType.Item)
                    _count = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the ItemCount of the Sign. Throws InvalidOperationException if the Layout is not SignType.Item
        /// </summary>
        public bool DetectorDisabled
        {
            get
            {
                return _detectorDisabled;
            }
            set
            {
                if (Layout == SignType.Item)
                    _detectorDisabled = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the ItemCount of the Sign. Throws InvalidOperationException if the Layout is not SignType.Item
        /// </summary>
        public bool IsCoin
        {
            get
            {
                return _isCoin;
            }
            set
            {
                if (Layout == SignType.Item)
                    _isCoin = value;
                else
                    throw new InvalidOperationException();
            }
        }
        #endregion

        public EventEntitySign()
        {
        }
    }
}
