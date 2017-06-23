using System;
using System.Collections.Generic;
using System.Linq;

using Csla.Server;

using XRMS.Libraries.BaseObjects;
using XRMS.Libraries.CslaBase;

namespace XRMS.Business.Services
{
    public abstract class GenericManager<T> : ObjectFactory, IGenericManager<T> where T : CslaBusinessBase<T>
    {
        public virtual T New()
        {
            T item = Csla.DataPortal.Create<T>();
            return item;
        }

        public abstract T FindItem(T item, List<T> list);

        public abstract T GetByKey(T itemWithKeys);

        public abstract List<T> GetList();

        public abstract bool Create(T item);

        public abstract bool Update(T item);

        public abstract bool Delete(T item);
    }

    public abstract class GenericIdBaseObjectManager<T> : GenericManager<T> where T : IdBaseObject<T>
    {
        /// <summary>
        /// Find item from list
        /// </summary>
        public override T FindItem(T item, List<T> list)
        {
            T resultItem = null;
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (list == null)
                    throw new ArgumentNullException("list");

                resultItem = list.SingleOrDefault<T>(x => x.Id == item.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultItem;
        }
    }
}
