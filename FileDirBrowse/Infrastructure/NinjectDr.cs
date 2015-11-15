using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileDirBrowse.DAL;
using Ninject;

namespace FileDirBrowse.Infrastructure
{
    public class NinjectDr : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDr(IKernel kernel)
        {
            this._kernel = kernel;
            AddBindings();
        }

        private void AddBindings()
        {
            _kernel.Bind<IApplicationFileManager>()
                .To<ApplicationFileManager>();
        }

        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);
        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);
    }
}