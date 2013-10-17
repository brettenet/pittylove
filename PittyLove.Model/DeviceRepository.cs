using System;
using System.Data.Entity;
using System.Linq;

namespace PittyLove.Model
{
    public class DeviceRepository : EfRepository<Device>, IDeviceRepository
    {
         #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        public DeviceRepository(DbContext context)
            : base(context)
        {
          
        }

        #endregion

        public Device GetByPublicKey(string publicKey)
        {
            return Set.FirstOrDefault(item => item.PublicKey.Equals(publicKey, StringComparison.OrdinalIgnoreCase));
        }
    }
}