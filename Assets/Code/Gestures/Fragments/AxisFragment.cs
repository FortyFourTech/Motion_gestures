using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    public abstract class AxisFragment : GestFragment
    {
        protected Vector3 _axis;

        public AxisFragment(GestureBase.IDataSource dataSource, Vector3 axis)
        {
            _dataSource = dataSource;
            _axis = axis;
        }
    }
}
