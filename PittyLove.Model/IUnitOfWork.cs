namespace PittyLove.Model
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the devices.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IDeviceRepository Devices { get; }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        IUserRepository Users { get; }

        /// <summary>
        /// Gets the sort portraits.
        /// </summary>
        IRepository<Pitbull> Pitbulls { get; }

        /// <summary>
        /// Commits the unity of work.
        /// </summary>
        void Commit();
    }
}