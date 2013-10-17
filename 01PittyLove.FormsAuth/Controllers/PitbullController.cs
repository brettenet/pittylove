using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PittyLove.Model;

namespace PittyLove.FormsAuth.Controllers
{
    public class PitbullController : ApiController
    {
        #region Private Data Members

        /// <summary>
        /// Unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PitbullController"/> class.
        /// </summary>
        /// <remarks></remarks>
        public PitbullController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a list of dogs this is a public facing method.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage Get()
        {
            //Step #1: Get the list of dogs from db
            var pits = _unitOfWork.Pitbulls.GetAll().ToList();

           /*
            * Step #2: Push the pits on to the response 
            * and return the ok (200) status code
            */
            return Request.CreateResponse(HttpStatusCode.OK, pits);
        }

        /// <summary>
        /// Gets a dog by the supplied id, this is a public facing method
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int id)
        {
            //Step #1: Get specific dog from db
            var pit = _unitOfWork.Pitbulls.GetById(id);

           /*
            * Step #2: Push dog on to the response 
            * and return the ok (200) status code
            */
            return Request.CreateResponse(HttpStatusCode.OK, pit);
        }

        /// <summary>
        /// Allows administrators edit dog info
        /// </summary>
        /// <param name="pitbull">The pitbull.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public HttpResponseMessage Save(Pitbull pitbull)
        {
            //On update lets get the current instance and apply changes
            var pit = _unitOfWork.Pitbulls.GetById(pitbull.Id);

            //Apply changes
            pit.Description = pitbull.Description;
            pit.ImageUrl = pitbull.ImageUrl;
            pit.Name = pitbull.Name;

            //Commit changes to data store
            _unitOfWork.Commit();

            //return the updated resource to the client with status code of OK (200)
            return Request.CreateResponse(HttpStatusCode.OK, pit);
        }

        [HttpPost]
        [Authorize]
        public HttpResponseMessage Share(Pitbull pitbull)
        {
            _unitOfWork.Pitbulls.Add(pitbull);
            _unitOfWork.Commit();
            return Request.CreateResponse(HttpStatusCode.OK, pitbull);
        }

        [HttpPut]
        [Authorize]
        public HttpResponseMessage FeedDog(int id, DateTime lastFed)
        {
            //On update lets get the current instance and apply changes
            var pit = _unitOfWork.Pitbulls.GetById(id);

            //Apply changes
            pit.LastFed = lastFed;

            //Commit changes to data store
            _unitOfWork.Commit();

            //return the updated resource to the client with status code of OK (200)
            return Request.CreateResponse(HttpStatusCode.OK, pit);
        }

        #endregion
    }
}