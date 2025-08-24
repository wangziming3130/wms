using Core.Grpc.Protos;
using Core.Service;
using Core.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Service
{
    public class ServiceFactory
    {
        private static readonly SiasunLogger Logger = SiasunLogger.GetInstance(typeof(ServiceFactory));

        IDBRepository _repository;
        IHttpContextAccessor _httpContextAccessor;
        IServiceProvider _serviceProvider;

        public ServiceFactory(IDBRepository repository,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }
        public IDBRepository DBRepository
        {
            get
            {
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
        private T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        public ICellService CellService
        {
            get
            {
                return GetService<ICellService>();
            }
        }
        public IWHService WHService
        {
            get
            {
                return GetService<IWHService>();
            }
        }
        public GUser.GUserClient _gUserClient
        {
            get
            {
                return GetService<GUser.GUserClient>();
            }
        }



    }
}
