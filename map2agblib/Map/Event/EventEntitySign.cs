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
        /// Gets the Layout, determined by the Type of the Sign Entity
        /// </summary>
        public SignType Layout
        {
            get
            {
                return Type >= 5 ? SignType.Script : SignType.Item;
            }
        }

        /// <summary>
        /// Gets or sets the Script of the Sign. Throws InvalidOperationException if the Layout is not SignType.Script
        /// </summary>
        public string Script
        {
            get
            {
                if (Layout == SignType.Script)
                {
                    return _script;
                }
                else
                {
                    throw new InvalidOperationException();
                }
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
                if (Layout == SignType.Item)
                    return _itemId;
                else
                    throw new InvalidOperationException();
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
                if (Layout == SignType.Item)
                    return _hiddenId;
                else
                    throw new InvalidOperationException();
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
                if (Layout == SignType.Item)
                    return _count;
                else
                    throw new InvalidOperationException();
            }
            set
            {
                if (Layout == SignType.Item)
                    _count = value;
                else
                    throw new InvalidOperationException();
            }
        }
        #endregion

        public EventEntitySign()
        {
            throw new NotImplementedException();
        }
    }
}
