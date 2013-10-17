using System;
using System.Data.Entity;

namespace PittyLove.Model
{
    public sealed class EfUnitOfWork : IUnitOfWork, IDisposable
    {
        #region Private Data Members

        /// <summary>
        /// Data Context
        /// </summary>
        private readonly DbContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfUnitOfWork"/> class.
        /// </summary>
        public EfUnitOfWork()
        {
            _context = new PittyLoveEntities();

            //Turning off proxy generation
            _context.Configuration.ProxyCreationEnabled = false;

            //Turn off lazy loading support
            _context.Configuration.LazyLoadingEnabled = false;

            Pitbulls = new EfRepository<Pitbull>(_context);
            Users = new UserRepository(_context);
            Devices = new DeviceRepository(_context);
        }

        #endregion

        #region IUnitOfWork Members

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IUserRepository Users
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public IDeviceRepository Devices
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <value>The jobs.</value>
        public IRepository<Pitbull> Pitbulls
        {
            get;
            private set;
        }

        /// <summary>
        /// Commits this instance.
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        #endregion
    }
}