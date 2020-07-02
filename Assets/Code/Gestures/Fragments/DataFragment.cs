using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dimar.Gestures.Fragments
{
    /// <summary>
    /// Фрагмент, использующий какие-либо данные источника данных.
    /// </summary>
    public abstract class DataFragment : GestFragment
    {
        protected DataFragment(GestureBase.IDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
