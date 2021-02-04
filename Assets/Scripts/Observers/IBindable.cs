using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Observers
{
    public interface IBindable
    {
        IEnumerator Bind();
    }
}
