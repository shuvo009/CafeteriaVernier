using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Procesta.CvServer
{
    class ParmissionIequality : IEqualityComparer<ModelEmployPermissions>
    {
        public bool Equals(ModelEmployPermissions x, ModelEmployPermissions y)
        {
            if (x.Setting!=null)
            {
                return x.Setting == y.Setting;
            }
            else
            {
                return x.Item == y.Item;
            }
        }

        public int GetHashCode(ModelEmployPermissions obj)
        {
            if (obj.Setting!=null)
            {
                return obj.Setting.GetHashCode();
            }
            else
            {
                return obj.Item.GetHashCode();
            }
        }
    }
}
