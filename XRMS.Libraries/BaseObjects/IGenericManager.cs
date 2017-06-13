using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRMS.Libraries.BaseObjects
{
    public interface IGenericManager<T> where T : class
    {
        T New();

        T GetByKey(T itemWithKeys);

        T FindItem(T item, List<T> list);

        List<T> GetList();

        bool Create(T item);

        bool Update(T item);

        bool Delete(T item);
    }

    public interface IGenericIdBaseObjectManager<T> where T : IdBaseObject<T>
    {
        T GetById(int id);
    }
}
