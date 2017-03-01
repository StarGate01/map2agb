using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Tilesets;

namespace map2agbgui.Models.BlockEditor
{

    public class BlockBehaviourModel : IRomSerializable<BlockBehaviourModel, BlockBehaviour>, INotifyPropertyChanged
    {

        #region Properties

        private byte _behaviour, _hmUsage, _field2, _field3, _field4, _field5, _field6;
        public byte Behaviour
        {
            get
            {
                return _behaviour;
            }
            set
            {
                _behaviour = value;
                RaisePropertyChanged("Behaviour");
            }
        }
        public byte HMUsage
        {
            get
            {
                return _hmUsage;
            }
            set
            {
                _hmUsage = value;
                RaisePropertyChanged("HMUsage");
            }
        }
        public byte Field2
        {
            get
            {
                return _field2;
            }
            set
            {
                _field2 = value;
                RaisePropertyChanged("Field2");
            }
        }
        public byte Field3
        {
            get
            {
                return _field3;
            }
            set
            {
                _field3 = value;
                RaisePropertyChanged("Field3");
            }
        }
        public byte Field4
        {
            get
            {
                return _field4;
            }
            set
            {
                _field4 = value;
                RaisePropertyChanged("Field4");
            }
        }
        public byte Field5
        {
            get
            {
                return _field5;
            }
            set
            {
                _field5 = value;
                RaisePropertyChanged("Field5");
            }
        }
        public byte Field6
        {
            get
            {
                return _field6;
            }
            set
            {
                _field6 = value;
                RaisePropertyChanged("Field6");
            }
        }

        #endregion

        #region Constructor

        public BlockBehaviourModel(BlockBehaviour entry) : base(entry)
        {
            _behaviour = entry.Behavior;
            _hmUsage = entry.HmUsage;
            _field2 = entry.Field2;
            _field3 = entry.Field3;
            _field4 = entry.Field4;
            _field5 = entry.Field5;
            _field6 = entry.Field6;
        }

        #endregion

        #region Methods

        public override BlockBehaviour ToRomData()
        {
            BlockBehaviour data = new BlockBehaviour();
            data.Behavior = _behaviour;
            data.HmUsage = _hmUsage;
            data.Field2 = _field2;
            data.Field3 = _field3;
            data.Field4 = _field4;
            data.Field5 = _field5;
            data.Field6 = _field6;
            return data;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
