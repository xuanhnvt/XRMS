using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using Csla;

using XRMS.Data.EntityFramework;
using XRMS.Business.Models;
using XRMS.Business.Repositories;
using XRMS.Business.Services;

using XRMS.Business.Repositories.DomainModel;
using XRMS.Business.UnitOfWorks;

using MEFedMVVM.ViewModelLocator;
using Cinch;
namespace XRMS.Business.Services
{
    /// <summary>
    /// This class implements the IReportOrderItemEditionManager for WPF purposes.
    /// </summary>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportService(ServiceType.Runtime, typeof(IReportOrderItemEditionManager))]
    public class ReportOrderItemEditionManager : GenericManager<ReportOrderItemEdition>, IReportOrderItemEditionManager
    {
        private UnitOfWork _uow;
        //private IRepositoryProvider _provider = new RepositoryProvider(RepositoryFactories.Instance());

        public ReportOrderItemEditionManager()
        {
            //_uow = new UnitOfWork(new RepositoryProvider(RepositoryFactories.Instance()));
        }

        public override ReportOrderItemEdition GetByKey(ReportOrderItemEdition itemWithKeys)
        {
            ReportOrderItemEdition item = null;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    item = _uow.ReportOrderItemEditionRepository
                        .GetBy(o => o.ReportCounter == itemWithKeys.ReportCounter
                                    && o.EditionCounter == itemWithKeys.EditionCounter
                                    && o.Sequence == itemWithKeys.Sequence)
                        .FirstOrDefault();
                    if (item != null)
                    {
                        // get data of inside class
                        item.OrderEdition = _uow.ReportOrderEditionRepository
                            .GetBy(o => o.ReportCounter == item.ReportCounter && o.EditionCounter == item.EditionCounter)
                            .FirstOrDefault(); ;
                        item.MarkOld();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return item;
        }

        public override List<ReportOrderItemEdition> GetList()
        {
            List<ReportOrderItemEdition> list = null;

            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    list = _uow.ReportOrderItemEditionRepository.GetAll().ToList();
                    if (list != null)
                    {
                        foreach (ReportOrderItemEdition item in list)
                        {
                            item.OrderEdition = _uow.ReportOrderEditionRepository
                            .GetBy(o => o.ReportCounter == item.ReportCounter && o.EditionCounter == item.EditionCounter)
                            .FirstOrDefault(); ;
                            item.MarkOld();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return list;
        }


        public List<ReportOrderItemEdition> GetEdititonReportOfOrder(Order order)
        {
            List<ReportOrderItemEdition> list = null;

            try
            {
                if (order == null)
                {
                    throw new ArgumentNullException("order");
                }

                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    // get report of order
                    ReportOrder report = _uow.ReportOrderRepository.GetByOrderId(order.Id);

                    // get edition report of order base on report counter
                    list = _uow.ReportOrderItemEditionRepository.GetBy(o => o.ReportCounter == report.ReportCounter).ToList();
                    if (list != null)
                    {
                        foreach (ReportOrderItemEdition item in list)
                        {
                            item.OrderEdition = _uow.ReportOrderEditionRepository
                            .GetBy(o => o.ReportCounter == item.ReportCounter && o.EditionCounter == item.EditionCounter)
                            .FirstOrDefault(); ;
                            item.MarkOld();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// Find item from list
        /// </summary>
        public override ReportOrderItemEdition FindItem(ReportOrderItemEdition item, List<ReportOrderItemEdition> list)
        {
            ReportOrderItemEdition resultItem = null;
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                if (list == null)
                    throw new ArgumentNullException("list");

                resultItem = list.SingleOrDefault<ReportOrderItemEdition>(o => o.ReportCounter == item.ReportCounter && o.EditionCounter == item.EditionCounter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultItem;
        }

        public override bool Create(ReportOrderItemEdition item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.ReportOrderItemEditionRepository.Add(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }

        public override bool Update(ReportOrderItemEdition item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.ReportOrderItemEditionRepository.Update(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }

        public override bool Delete(ReportOrderItemEdition item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            bool result = false;
            try
            {
                using (XRMSEntities context = new XRMSEntities())
                {
                    _uow = new UnitOfWork(context, new RepositoryProvider(RepositoryFactories.Instance()));
                    _uow.ReportOrderItemEditionRepository.Remove(item);
                    _uow.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(this.GetType().FullName + System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + ex.Message);
            }
            return result;
        }
    }
}
